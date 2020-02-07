using MoneyManager.Models.Domain;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _uow;
        private readonly int _currentUserId;

        public TransactionService(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _uow = unitOfWork;
            _currentUserId = currentUser.Id;
        }

        public async Task<Response<IEnumerable<Transaction>>> ListAsync()
        {
            var transactions = (await _uow.TransactionRepo.GetAllByUserAsync(_currentUserId))
                .Where(t => t.TransactionDetails.FirstOrDefault().FromAccount != null);
            return new Response<IEnumerable<Transaction>>(transactions);
        }

        public async Task<Response<Transaction>> CreateAsync(Transaction transaction)
        {
            
            try
            {
                DateTime createdAt = DateTime.UtcNow;

                transaction.CreatedAt = createdAt;
                int transactionId = await _uow.TransactionRepo.InsertAsync(transaction);

                var transactionDetails = await Task.WhenAll(transaction.TransactionDetails.Select(async (transactionDetail) =>
                {
                    transactionDetail.CreatedAt = createdAt;
                    transactionDetail.TransactionId = transactionId;
                    var detailId = await _uow.TransactionDetailsRepo.InsertAsync(transactionDetail);
                    return await _uow.TransactionDetailsRepo.GetAsync(detailId);
                }));

                _uow.Commit();

                var createdTransaction = new Transaction
                {
                    Id = transactionId,
                    UserId = _currentUserId,
                    Description = transaction.Description,
                    Date = transaction.Date,
                    TransactionDetails = transactionDetails,
                };
                return new Response<Transaction>(createdTransaction);
            }
            catch (Exception ex)
            {
                return new Response<Transaction>($"An error occurred when saving the category: {ex.Message}");
            }
        }

        public async Task<Response<Transaction>> UpdateAsync(int id, Transaction transaction)
        {
            var existingTransactionData = await _uow.TransactionRepo.GetAsync(id);
            if (existingTransactionData == null)
            {
                return new Response<Transaction>("Transaction not found");
            }

            var existingTransaction = existingTransactionData;

            existingTransaction.UpdatedAt = DateTime.UtcNow;
            existingTransaction.Description = transaction.Description;
            existingTransaction.Date = transaction.Date;

            var comparator = new TransactionDetailsIDComparator();
            var existingDetails = from existing in existingTransaction.TransactionDetails
                                  join given in transaction.TransactionDetails on existing.Id equals given.Id
                                  select new { Existing = existing, Given = given };
            var newDetails = transaction.TransactionDetails.Except(existingTransaction.TransactionDetails, comparator);
            var removedDetails = existingTransaction.TransactionDetails.Except(transaction.TransactionDetails, comparator);

            foreach (var split in existingDetails)
            {
                split.Existing.Amount = split.Given.Amount;
                split.Existing.FromAccountId = split.Given.FromAccountId;
                split.Existing.ToAccountId = split.Given.ToAccountId;
                split.Existing.CategoryId = split.Given.CategoryId;
            }

            foreach (var split in newDetails)
            {
                existingTransaction.TransactionDetails.Add(split);
            }

            foreach (var split in removedDetails.ToList())
            {
                existingTransaction.TransactionDetails.Remove(split);
            }

            try
            {
                var existingTransactionNewData = existingTransaction;
                await _uow.TransactionRepo.UpdateAsync(existingTransactionNewData);
                _uow.Commit();
                var updatedTransaction = await _uow.TransactionRepo.GetAsync(id);

                return new Response<Transaction>(updatedTransaction);
            }
            catch (Exception ex)
            {
                return new Response<Transaction>($"An error occurred when updating the transaction: {ex.Message}");
            }
        }

        public async Task<Response<Transaction>> DeleteAsync(int id)
        {
            var existingTransaction = await _uow.TransactionRepo.GetAsync(id);
            if (existingTransaction == null)
            {
                return new Response<Transaction>("Transaction not found");
            }

            try
            {
                await _uow.TransactionRepo.DeleteAsync(id);
                _uow.Commit();

                return new Response<Transaction>(existingTransaction);
            }
            catch (Exception ex)
            {
                return new Response<Transaction>($"An error occurred when deleting the transaction: {ex.Message}");
            }
        }
    }
}
