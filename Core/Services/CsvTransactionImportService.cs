using MoneyManager.Core.Repositories;
using MoneyManager.Models.Domain;
using MoneyManager.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MoneyManager.Core.Services
{
    public class CsvTransactionImportService
    {

        private readonly Regex _amountRegex = new Regex(@"\d*(?:,|.)?\d+");
        private readonly TransactionService _transactionService;
        private readonly CategoryService _categoryService;
        private readonly AccountService _accountService;
        private readonly IUnitOfWork _uow;
        private readonly int _currentUserId;

        public CsvTransactionImportService(TransactionService transactionService, CategoryService categoryService,
            AccountService accountService, IUnitOfWork unitOfWork, CurrentUserService currentUserService)
        {
            _transactionService = transactionService;
            _categoryService = categoryService;
            _accountService = accountService;
            _uow = unitOfWork;
            _currentUserId = currentUserService.Id;
        }


        public async Task<ImportResults> Import(string content)
        {
            using var reader = new StringReader(content);
            reader.ReadLine();
            int createdTransactions = 0;
            int createdPersonalAccounts = 0;
            int createdOtherAccounts = 0;
            int createdCategories = 0;

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] columns = line.Split(';');
                Row transactionRow = MapColumns(columns);
                (int, int, int) res = await CreateTransactionAsync(transactionRow);
                createdTransactions++;
                createdPersonalAccounts += res.Item1;
                createdOtherAccounts += res.Item2;
                createdCategories += res.Item3;

                if (createdTransactions == 10) break;
            }
            return new ImportResults(createdTransactions, createdPersonalAccounts, createdOtherAccounts, createdCategories);
        }

        private async Task<(int, int, int)> CreateTransactionAsync(Row row)
        {
            int createdPersonalAccount = 0;
            int createdOtherAccount = 0;
            int createdCategory = 0;

            bool isFromAccountPersonal = row.ExpenseType == "Expense" || row.ExpenseType == "Transfer";
            var existingFromAccount = await _uow.AccountRepo.GetByNameAsync(_currentUserId, row.FromAccount, isFromAccountPersonal);
            if (existingFromAccount == null)
            {
                var newAccount = new CreateAccountVm
                {
                    Name = row.FromAccount,
                    IsPersonal = isFromAccountPersonal,
                    OpeningBalance = 0,
                    OpeningDate = DateTime.UtcNow
                };
                existingFromAccount = await _accountService.CreateAccountAsync(newAccount);
                if (isFromAccountPersonal)
                {
                    createdPersonalAccount++;
                }
                else
                {
                    createdOtherAccount++;
                }
            }

            bool isToAccountPersonal = row.ExpenseType == "Income" || row.ExpenseType == "Transfer";
            var existingToAccount = await _uow.AccountRepo.GetByNameAsync(_currentUserId, row.ToAccount, isToAccountPersonal);
            if (existingToAccount == null)
            {
                var newAccount = new CreateAccountVm
                {
                    Name = row.ToAccount,
                    IsPersonal = isToAccountPersonal,
                    OpeningBalance = 0,
                    OpeningDate = DateTime.UtcNow
                };
                existingToAccount = await _accountService.CreateAccountAsync(newAccount);

                if (isToAccountPersonal)
                {
                    createdPersonalAccount++;
                }
                else
                {
                    createdOtherAccount++;
                }
            }

            var existingCategory = !string.IsNullOrEmpty(row.Category) ? await _uow.CategoryRepo.GetByUserAsync(_currentUserId, "Name", row.Category) : null;
            if (!string.IsNullOrEmpty(row.Category) && existingCategory == null)
            {
                var newCategory = new Category
                {
                    Name = row.Category,
                };
                existingCategory = await _categoryService.CreateAsync(newCategory);
                createdCategory++;
            }

            var newTransaction = new Transaction
            {
                Date = DateTime.Parse(row.Date),
                Description = !string.IsNullOrEmpty(row.Description) ? row.Description : null,
                TransactionDetails = new List<TransactionDetails>()
                {
                    new TransactionDetails
                    {
                        Amount = row.Amount,
                        FromAccountId = (int)existingFromAccount.Id!,
                        ToAccountId = (int)existingToAccount.Id!,
                        CategoryId = existingCategory?.Id,
                    }
                }
            };
            await _transactionService.CreateAsync(newTransaction);
            return (createdPersonalAccount, createdOtherAccount, createdCategory);
        }

        private Row MapColumns(string[] columns)
        {
            return new Row
            {
                ExpenseType = columns[0].Trim(),
                Date = columns[1].Trim(),
                Amount = ParseAmount(columns[2]),
                FromAccount = columns[3].Trim(),
                ToAccount = columns[4].Trim(),
                Category = columns[5].Trim(),
                Description = columns[6].Trim()

            };
        }

        private double ParseAmount(string amount)
        {
            var amountMatch = _amountRegex.Match(amount);
            if (!amountMatch.Success)
            {
                throw new Exception($"Could not parse amount to number: {amount}");
            }
            
            return double.Parse(amountMatch.Value.Replace('.', ','));
        }


        class Row
        {
            public string ExpenseType { get; set; } = null!;
            public string Date { get; set; } = null!;
            public double Amount { get; set; }
            public string FromAccount { get; set; } = null!;
            public string ToAccount { get; set; } = null!;
            public string Category { get; set; } = null!;
            public string Description { get; set; } = null!;

        }
    }
}
