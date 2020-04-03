using MoneyManager.Core.Repositories;
using MoneyManager.Core.Services.Communication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Models.Domain;
using MoneyManager.Models.ViewModels;
using System.Linq;

namespace MoneyManager.Core.Services
{
    public class AccountService
    {
        private readonly IUnitOfWork _uow;
        private readonly int _currentUserId;

        public AccountService(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _uow = unitOfWork;
            _currentUserId = currentUser.Id;
        }

        public async Task<IEnumerable<AccountVm>> ListAsyncNew()
        {
            var accounts = await _uow.AccountRepo.GetAllByUserAsync(_currentUserId);
            return accounts.Select(account =>
            {
                return new AccountVm(account)
                {
                    CurrentBalance = 5
                };
            });
        }

        public async Task<AccountVm> CreateAccountAsync(CreateAccountVm vm)
        {
            try
            {
                var createdAt = DateTime.UtcNow;

                var account = new Account
                {
                    Name = vm.Name!,
                    IsPersonal = (bool)vm.IsPersonal!,
                    OpeningBalance = (double)vm.OpeningBalance!,
                    OpeningDate = (DateTime)vm.OpeningDate!,
                    CreatedAt = createdAt,
                    UserId = _currentUserId,
                };
                int accountId = await _uow.AccountRepo.InsertAsync(account);

                _uow.Commit();

                var accountInserted = await _uow.AccountRepo.GetAsync(accountId);

                var createdVm = new AccountVm(accountInserted);

                return createdVm;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when saving the account: {ex.Message}");
            }
        }

        public async Task<AccountVm> EditAccountAsync(int accountId, EditAccountVm vm)
        {
            try
            {
                var account = await _uow.AccountRepo.GetByUserAsync(_currentUserId, accountId);
                if (account == null)
                {
                    throw new Exception($"User with Id {accountId} does not exist");
                }

                if (vm.Name != null) account.Name = vm.Name!;
                if (vm.OpeningBalance != null) account.OpeningBalance = (double)vm.OpeningBalance!;
                if (vm.OpeningDate != null) account.OpeningDate = (DateTime)vm.OpeningDate!;
                account.UpdatedAt = DateTime.UtcNow;

                await _uow.AccountRepo.UpdateAsync(account);

                _uow.Commit();

                var createdVm = new AccountVm(account);

                return createdVm;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when saving the account: {ex.Message}");
            }
        }

        private async Task<double> GetCurrentBalanceAsync(int accountId)
        {
            var transactions = await _uow.TransactionRepo.GetAllByUserAsync(_currentUserId);
            return transactions.Aggregate(0.0, (result, transaction) =>
            {
                double transactionSum = transaction.TransactionDetails.Aggregate(0.0, (transactionSum, td) =>
                {
                    if (td.FromAccount?.Id == accountId)
                    {
                        transactionSum -= td.Amount;
                    }
                    else if (td.ToAccount.Id == accountId)
                    {
                        transactionSum += td.Amount;
                    }
                    return transactionSum;

                });
                return result += transactionSum;
            });
        }

        public async Task<Response<Account>> DeleteAsync(int id)
        {
            var existingAccount = await _uow.AccountRepo.GetAsync(id);
            if (existingAccount == null)
            {
                return new Response<Account>("Account not found");
            }

            try
            {
                await _uow.AccountRepo.DeleteAsync(id);
                _uow.Commit();

                return new Response<Account>(existingAccount);
            }
            catch (Exception ex)
            {
                return new Response<Account>($"An error occurred when deleting the category: {ex.Message}");
            }
        }
    }
}
