using MoneyManager.Core.Repositories;
using MoneyManager.Models.Domain;
using MoneyManager.Models.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Core.Services
{
    public class DashboardData
    {
        public object ExpensesPerCategory { get; set; } = null!;
        public IEnumerable<Aggr> AmountsPerMonth { get; set; } = null!;
    }

    public class Aggr
    {
        public string Month { get; set; } = null!;
        public double Expenses { get; set; }
        public double Income { get; set; }
    }

    public class DashboardService
    {
        private readonly IUnitOfWork _uow;
        private readonly int _currentUserId;

        public DashboardService(IUnitOfWork unitOfWork, CurrentUserService currentUser)
        {
            _uow = unitOfWork;
            _currentUserId = currentUser.Id;
        }

        public async Task<DashboardData> GatherDataAsync(DateTime fromDate, DateTime toDate)
        {
            return new DashboardData
            {
                ExpensesPerCategory = await GetExpensesPerCategory(fromDate, toDate),
                AmountsPerMonth = await GetAmountsLastThreeMonths(3)
            };
        }

        private async Task<IEnumerable<Aggr>> GetAmountsLastThreeMonths(int monthsBack)
        {
            var months = new List<string>();
            var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - (monthsBack - 1), 1);
            for (int i = 0; i < monthsBack; i++)
            {
                months.Add(fromDate.AddMonths(i).ToISODateString());
            }

            var table = months.Select(month => new Aggr
            {
                Month = month,
                Expenses = 0,
                Income = 0
            });


            var transactions = (await _uow.TransactionRepo.GetAllAsync())
                .Where(t => t.Date >= fromDate)
                .GroupBy(t => new DateTime(DateTime.Now.Year, t.Date.Month, 1))
                .Select(group => new
                {
                    Month =  group.Key,
                    Amounts = group.Aggregate((0.0, 0.0), (amounts, t) =>
                    {
                        if (t.Type == TransactionType.Expense)
                        {
                            amounts.Item1 += t.Amount;
                        }
                        else if (t.Type == TransactionType.Income)
                        {
                            amounts.Item2 += t.Amount;
                        }
                        return amounts;
                    })
                })
                .Select(group => new Aggr
                { 
                    Month = group.Month.ToISODateString(),
                    Expenses = group.Amounts.Item1,
                    Income = group.Amounts.Item2
                });

            var result =
                from tableRow in table
                join data in transactions on
                    tableRow.Month equals data.Month into gr
                from dataJoin in gr.DefaultIfEmpty()
                select new Aggr
                {
                    Month = tableRow.Month,
                    Expenses = dataJoin?.Expenses ?? 0,
                    Income = dataJoin?.Income ?? 0
                };
                

            return result;
        }

        private async Task<object> GetExpensesPerCategory(DateTime fromDate, DateTime toDate)
        {
            var transactions = await _uow.TransactionRepo.GetAllAsync();
            var categoryAmounts = new Dictionary<int, double>()
            {
                { -1, 0 }
            };
            var categories = new Dictionary<int, Category?>()
            {
                { -1, null }
            };

            foreach (var transaction in transactions)
            {
                if (transaction.Date >= fromDate && transaction.Date <= toDate)
                {
                    foreach (var detail in transaction.TransactionDetails)
                    {
                        int categoryId = detail.Category?.Id ?? -1;
                        if (!categoryAmounts.ContainsKey(categoryId))
                        {
                            categoryAmounts.Add(categoryId!, 0);
                            categories.Add(categoryId, detail.Category);
                        }
                        categoryAmounts[categoryId] -= detail.AdjustedAmount;
                    }
                }
            }

            return categoryAmounts
                .Select(item => new { Category = categories[item.Key]?.ToGetCategoryDTO(), Amount = item.Value })
                .OrderByDescending(item => item.Amount);
        }
    }
}
