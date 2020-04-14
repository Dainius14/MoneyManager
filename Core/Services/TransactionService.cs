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

        public TransactionService(IUnitOfWork unitOfWork, CurrentUserService currentUserService)
        {
            _uow = unitOfWork;
            _currentUserId = currentUserService.Id;
        }

        public async Task<IEnumerable<Transaction>> ListAsync()
        {
            return await _uow.TransactionRepo.GetAllAsync();
        }

        public async Task<Transaction> CreateAsync(Transaction transaction)
        {

            try
            {
                var fromAccount = await _uow.AccountRepo.GetAsync(transaction.TransactionDetails[0].FromAccountId);
                var toAccount = await _uow.AccountRepo.GetAsync(transaction.TransactionDetails[0].ToAccountId);
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

                _uow.Commit();
                return (await _uow.TransactionRepo.GetAllAsync()).First(t => t.Id == transaction.Id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when saving the transaction: {ex.Message}");
            }
        }

        public async Task<Transaction> UpdateAsync(int id, Transaction transaction)
        {
            // TODO SQL
            var existingTransaction = await _uow.TransactionRepo.GetAsync(id);

            if (existingTransaction == null)
            {
                throw new NotFoundException("Transaction not found");
            }

            var newFromAccount = await _uow.AccountRepo.GetAsync(transaction.TransactionDetails[0].FromAccountId);
            var newToAccount = await _uow.AccountRepo.GetAsync(transaction.TransactionDetails[0].ToAccountId);

            existingTransaction.UpdatedAt = DateTime.UtcNow;
            existingTransaction.Description = transaction.Description;
            existingTransaction.Date = transaction.Date;
            existingTransaction.Type = GetTransactionType(newFromAccount, newToAccount);
            existingTransaction.TransactionDetails[0].Amount = transaction.TransactionDetails[0].Amount;
            existingTransaction.TransactionDetails[0].FromAccountId = transaction.TransactionDetails[0].FromAccountId;
            existingTransaction.TransactionDetails[0].ToAccountId = transaction.TransactionDetails[0].ToAccountId;
            existingTransaction.TransactionDetails[0].CategoryId = transaction.TransactionDetails[0].CategoryId;

            try
            {
                await _uow.TransactionRepo.UpdateAsync(existingTransaction);
                await _uow.TransactionDetailsRepo.UpdateAsync(existingTransaction.TransactionDetails[0]);
                _uow.Commit();
                return await _uow.TransactionRepo.GetAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when updating the transaction: {ex.Message}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (!await _uow.TransactionRepo.ExistsAsync(id))
            {
                throw new NotFoundException("Transaction not found");
            }

            try
            {
                await _uow.TransactionRepo.DeleteAsync(id);
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

    }
}
