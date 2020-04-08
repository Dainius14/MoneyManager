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
            var transactions = (await _uow.TransactionRepo.GetAllByUserAsync(_currentUserId))
                .Where(t => t.TransactionDetails.FirstOrDefault().FromAccount != null);
            return transactions;
        }

        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            
            try
            {
                DateTime createdAt = DateTime.UtcNow;

                transaction.CreatedAt = createdAt;
                transaction.UserId = _currentUserId;
                transaction.Id = await _uow.TransactionRepo.InsertAsync(transaction);

                transaction.TransactionDetails = await Task.WhenAll(transaction.TransactionDetails.Select(async (transactionDetail) =>
                {
                    transactionDetail.CreatedAt = createdAt;
                    transactionDetail.TransactionId = (int)transaction.Id;
                    var detailId = await _uow.TransactionDetailsRepo.InsertAsync(transactionDetail);
                    return await _uow.TransactionDetailsRepo.GetAsync(detailId);
                }));

                _uow.Commit();
                return transaction;
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
            
            existingTransaction.UpdatedAt = DateTime.UtcNow;
            existingTransaction.Description = transaction.Description;
            existingTransaction.Date = transaction.Date;
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
                await _uow.TransactionRepo.DeleteAsync(id);
                _uow.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when deleting the transaction: {ex.Message}");
            }
        }
    }
}
