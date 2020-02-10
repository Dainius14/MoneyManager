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
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _uow;
        private readonly int _currentUserId;

        public AccountService(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _uow = unitOfWork;
            _currentUserId = currentUser.Id;
        }

        public async Task<IEnumerable<Account>> ListAsync()
        {
            return (await _uow.AccountRepo.GetAllByUserAsync(_currentUserId));
        }

        public async Task<IEnumerable<GetPersonalAccountVm>> ListPersonalAsync()
        {
            var accounts = await ListAsync();
            var personalAccounts = accounts.Where(a => a.IsPersonal);

            var currency = await _uow.CurrencyRepo.GetAsync(1);

            var viewModels = personalAccounts.Select(a => new GetPersonalAccountVm(a, 69, currency));
            return viewModels;
        }
        public async Task<IEnumerable<GetNonPersonalAccountVm>> ListNonPersonalAsync()
        {
            var accounts = await ListAsync();
            var personalAccounts = accounts.Where(a => !a.IsPersonal);

            var currency = await _uow.CurrencyRepo.GetAsync(1);

            var viewModels = personalAccounts.Select(a => new GetNonPersonalAccountVm(a, 696, currency));
            return viewModels;
        }

        public async Task<Response<Account>> GetById(int accountId)
        {
            try
            {
                var account = await _uow.AccountRepo.GetByUserAsync(_currentUserId, accountId);
                return new Response<Account>(account);
            }
            catch (Exception ex)
            {
                return new Response<Account>($"An error occurred when getting the account: {ex.Message}");
            }
        }

        public async Task<Response<GetPersonalAccountVm>> CreateAsync(EditPersonalAccountVm vm)
        {
            try
            {
                var createdAt = DateTime.UtcNow;

                var account = new Account
                {
                    Name = vm.Name,
                    IsPersonal = true,
                    CreatedAt = createdAt,
                    UserId = _currentUserId,
                };
                int accountId = await _uow.AccountRepo.InsertAsync(account);


                var transaction = new Transaction
                {
                    Date = vm.InitialDate.Date,
                    CreatedAt = createdAt,
                    UserId = _currentUserId,
                };
                int transactionId = await _uow.TransactionRepo.InsertAsync(transaction);

                var transactionDetail = new TransactionDetails
                {
                    FromAccountId = null,
                    ToAccountId = accountId,
                    Amount = vm.InitialBalance,
                    CurrencyId = 1,  // TODO remove with currency support I guess
                    TransactionId = transactionId,
                    CreatedAt = createdAt
                };
                int transactionDetailsId = await _uow.TransactionDetailsRepo.InsertAsync(transactionDetail);

                _uow.Commit();

                var accountInserted = await _uow.AccountRepo.GetAsync(accountId);
                var currency = await _uow.CurrencyRepo.GetAsync((int)vm.CurrencyId);

                var createdVm = new GetPersonalAccountVm(accountInserted, vm.InitialBalance, currency);

                return new Response<GetPersonalAccountVm>(createdVm);
            }
            catch (Exception ex)
            {
                return new Response<GetPersonalAccountVm>($"An error occurred when saving the category: {ex.Message}");
            }
        }
        public async Task<Response<GetNonPersonalAccountVm>> CreateAsync(EditNonPersonalAccountVm vm)
        {
            try
            {
                var createdAt = DateTime.UtcNow;

                var account = new Account
                {
                    Name = vm.Name,
                    IsPersonal = false,
                    CreatedAt = createdAt,
                    UserId = _currentUserId,
                };
                int accountId = await _uow.AccountRepo.InsertAsync(account);

                _uow.Commit();

                var accountInserted = await _uow.AccountRepo.GetAsync(accountId);
                var currency = await _uow.CurrencyRepo.GetAsync(1);

                var createdVm = new GetNonPersonalAccountVm(accountInserted, 0, currency);

                return new Response<GetNonPersonalAccountVm>(createdVm);
            }
            catch (Exception ex)
            {
                return new Response<GetNonPersonalAccountVm>($"An error occurred when saving the category: {ex.Message}");
            }
        }

        public async Task<Response<Account>> UpdateAsync(int id, Account account)
        {
            var existingAccount = await _uow.AccountRepo.GetAsync(id);
            if (existingAccount == null)
            {
                return new Response<Account>("Category not found");
            }

            existingAccount.Name = account.Name;
            existingAccount.IsPersonal = account.IsPersonal;
            existingAccount.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _uow.AccountRepo.UpdateAsync(existingAccount);
                _uow.Commit();

                return new Response<Account>(existingAccount);
            }
            catch (Exception ex)
            {
                return new Response<Account>($"An error occurred when updating the category: {ex.Message}");
            }
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
