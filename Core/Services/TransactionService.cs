using MoneyManager.Models.Domain;
using MoneyManager.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyManager.Core.Services.Exceptions;

namespace MoneyManager.Core.Services
{
    public class TransactionService
    {
        private readonly IUnitOfWork _uow;
        private readonly int _currentUserId;

        public TransactionService(IUnitOfWork unitOfWork, CurrentUserService currentUser)
        {
            _uow = unitOfWork;
            _currentUserId = currentUser.Id;
        }

        public async Task<IEnumerable<Transaction>> ListAsync()
        {
            var historyEntries = await _uow.BalanceHistoryRepo.GetAllByUserAsync(_currentUserId);
            var transactions = (await _uow.TransactionRepo.GetAllByUserAsync(_currentUserId))
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.CreatedAt);
            foreach (var t in transactions)
            {
                t.FromAccountBalance = historyEntries.First(x => x.TransactionId == t.Id && x.AccountId == t.TransactionDetails[0].FromAccountId).Balance;
                t.ToAccountBalance = historyEntries.First(x => x.TransactionId == t.Id && x.AccountId == t.TransactionDetails[0].ToAccountId).Balance;
            }

            return transactions;
        }

        public async Task<Transaction> CreateAsync(Transaction transaction)
        {

            try
            {
                var fromAccount = await _uow.AccountRepo.GetByUserAsync(_currentUserId, transaction.TransactionDetails[0].FromAccountId);
                var toAccount = await _uow.AccountRepo.GetByUserAsync(_currentUserId, transaction.TransactionDetails[0].ToAccountId);
                DateTime createdAt = DateTime.UtcNow;

                transaction.Type = GetTransactionType(fromAccount, toAccount);
                transaction.CreatedAt = createdAt;
                transaction.UserId = _currentUserId;
                transaction.Id = await _uow.TransactionRepo.InsertAsync(transaction);

                transaction.TransactionDetails = await Task.WhenAll(transaction.TransactionDetails.Select(async (transactionDetail) =>
                {
                    transactionDetail.TransactionId = (int)transaction.Id;
                    transactionDetail.Id = await _uow.TransactionDetailsRepo.InsertAsync(transactionDetail);
                    return transactionDetail;
                }));

                await CreateBalanceHistoryEntryAsync(transaction, fromAccount, toAccount);
                await UpdateNewerBalanceHistoryEntriesAsync(transaction, fromAccount, toAccount);


                _uow.Commit();
                return (await _uow.TransactionRepo.GetAllByUserAsync(_currentUserId)).First(t => t.Id == transaction.Id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when saving the transaction: {ex.Message}");
            }
        }

        public async Task<Transaction> UpdateAsync(int id, Transaction transaction)
        {
            // TODO SQL
            var existingTransaction = (await _uow.TransactionRepo.GetAllByUserAsync(_currentUserId))
                .FirstOrDefault(t => t.Id == id);

            if (existingTransaction == null)
            {
                throw new NotFoundException("Transaction not found");
            }

            var fromAccount = await _uow.AccountRepo.GetByUserAsync(_currentUserId, transaction.TransactionDetails[0].FromAccountId);
            var toAccount = await _uow.AccountRepo.GetByUserAsync(_currentUserId, transaction.TransactionDetails[0].ToAccountId);

            existingTransaction.UpdatedAt = DateTime.UtcNow;
            existingTransaction.Description = transaction.Description;
            existingTransaction.Date = transaction.Date;
            existingTransaction.Type = GetTransactionType(fromAccount, toAccount);
            existingTransaction.TransactionDetails[0].Amount = transaction.TransactionDetails[0].Amount;
            existingTransaction.TransactionDetails[0].FromAccountId = transaction.TransactionDetails[0].FromAccountId;
            existingTransaction.TransactionDetails[0].ToAccountId = transaction.TransactionDetails[0].ToAccountId;
            existingTransaction.TransactionDetails[0].CategoryId = transaction.TransactionDetails[0].CategoryId;

            //var comparator = new TransactionDetailsIDComparator();
            //var existingDetails = from existing in existingTransaction.TransactionDetails
            //                      join given in transaction.TransactionDetails on existing.Id equals given.Id
            //                      select new { Existing = existing, Given = given };
            //var newDetails = transaction.TransactionDetails.Except(existingTransaction.TransactionDetails, comparator);
            //var removedDetails = existingTransaction.TransactionDetails.Except(transaction.TransactionDetails, comparator);

            //foreach (var split in existingDetails)
            //{
            //    split.Existing.Amount = split.Given.Amount;
            //    split.Existing.FromAccountId = split.Given.FromAccountId;
            //    split.Existing.ToAccountId = split.Given.ToAccountId;
            //    split.Existing.CategoryId = split.Given.CategoryId;
            //}

            //foreach (var split in newDetails)
            //{
            //    existingTransaction.TransactionDetails.Add(split);
            //}

            //foreach (var split in removedDetails.ToList())
            //{
            //    existingTransaction.TransactionDetails.Remove(split);
            //}



            try
            {
                await _uow.TransactionRepo.UpdateAsync(existingTransaction);
                await _uow.TransactionDetailsRepo.UpdateAsync(existingTransaction.TransactionDetails[0]);

                await _uow.BalanceHistoryRepo.DeleteByTransaction(id);
                await CreateBalanceHistoryEntryAsync(transaction, fromAccount, toAccount);
                await UpdateNewerBalanceHistoryEntriesAsync(existingTransaction, fromAccount, toAccount);


                _uow.Commit();
                var updatedTransaction = (await _uow.TransactionRepo.GetAllByUserAsync(_currentUserId))
                    .Where(t => t.Id == id)
                    .First();  // TODO get one with SQL
                return updatedTransaction;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when updating the transaction: {ex.Message}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            var existingTransaction = await _uow.TransactionRepo.GetAsync(id);
            if (existingTransaction == null)
            {
                throw new NotFoundException("Transaction not found");
            }

            try
            {
                // TODO SQL
                var transaction = (await _uow.TransactionRepo.GetAllByUserAsync(_currentUserId))
                    .FirstOrDefault(t => t.Id == id);
                var fromAccount = await _uow.AccountRepo.GetByUserAsync(_currentUserId, transaction.TransactionDetails[0].FromAccountId);
                var toAccount = await _uow.AccountRepo.GetByUserAsync(_currentUserId, transaction.TransactionDetails[0].ToAccountId);


                await _uow.TransactionRepo.DeleteAsync(id);
                await UpdateNewerBalanceHistoryEntriesAsync(existingTransaction, fromAccount, toAccount);
                _uow.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when deleting the transaction: {ex.Message}");
            }
        }



        private string GetTransactionType(Account fromAccount, Account toAccount)
        {
            bool isFromPersonal = fromAccount.IsPersonal;
            bool isToPersonal = toAccount.IsPersonal;

            if (isFromPersonal && !isToPersonal)
            {
                return TransactionType.Expense;
            }
            else if (!isFromPersonal && isToPersonal)
            {
                return TransactionType.Income;
            }
            else if (isFromPersonal && isToPersonal)
            {
                return TransactionType.Transfer;
            }
            throw new Exception($"Invalid transaction type");
        }


        private async Task UpdateNewerBalanceHistoryEntriesAsync(Transaction transaction, Account fromAccount, Account toAccount)
        {
            var newerTransactions = (await _uow.TransactionRepo.GetAllByUserAsync(_currentUserId))
                .Where(x => (x.TransactionDetails[0].FromAccountId == fromAccount.Id || x.TransactionDetails[0].ToAccountId == fromAccount.Id
                || x.TransactionDetails[0].FromAccountId == toAccount.Id || x.TransactionDetails[0].ToAccountId == toAccount.Id)
                    && x.Date > transaction.Date)
                .OrderBy(x => x.Date)
                .ThenBy(x => x.CreatedAt);
            foreach (var newerTransction in newerTransactions)
            {
                await _uow.BalanceHistoryRepo.DeleteByTransaction((int)newerTransction.Id!);
            }
            foreach (var newerTransction in newerTransactions)
            {
                await CreateBalanceHistoryEntryAsync(newerTransction, newerTransction.TransactionDetails[0].FromAccount, newerTransction.TransactionDetails[0].ToAccount);
            }
        }

        private async Task CreateBalanceHistoryEntryAsync(Transaction transaction, Account fromAccount, Account toAccount)
        {
            var historyEntries = await _uow.BalanceHistoryRepo.GetAllByUserAsync(_currentUserId);
            var fromAccountHistoryEntry = historyEntries
                .Where(x => x.AccountId == fromAccount.Id && x.Transaction.Date <= transaction.Date)
                .OrderBy(x => x.Transaction.Date)
                .ThenBy(x => x.Transaction.CreatedAt)
                .FirstOrDefault();
            var fromAccountLatestBalance = fromAccountHistoryEntry?.Balance ?? fromAccount.OpeningBalance;

            var toAccountHistoryEntry = historyEntries
                .Where(x => x.AccountId == toAccount.Id && x.Transaction.Date <= transaction.Date)
                .OrderBy(x => x.Transaction.Date)
                .ThenBy(x => x.Transaction.CreatedAt)
                .FirstOrDefault();
            var toAccountLatestBalance = toAccountHistoryEntry?.Balance ?? toAccount.OpeningBalance;


            var fromAccountBalance = new BalanceHistory
            {
                TransactionId = (int)transaction.Id!,
                AccountId = (int)fromAccount.Id!,
                Balance = fromAccountLatestBalance - transaction.Amount
            };
            var toAccountBalance = new BalanceHistory
            {
                TransactionId = (int)transaction.Id!,
                AccountId = (int)toAccount.Id!,
                Balance = toAccountLatestBalance + transaction.Amount
            };

            await _uow.BalanceHistoryRepo.InsertAsync(fromAccountBalance);
            await _uow.BalanceHistoryRepo.InsertAsync(toAccountBalance);
        }
    }
}
