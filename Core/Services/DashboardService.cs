using MoneyManager.Core.Repositories;
using MoneyManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Core.Services
{
    public class DashboardService
    {
        private readonly IUnitOfWork _uow;
        private readonly int _currentUserId;

        public DashboardService(IUnitOfWork unitOfWork, CurrentUserService currentUser)
        {
            _uow = unitOfWork;
            _currentUserId = currentUser.Id;
        }

        public async Task<object> GatherDataAsync(DateTime fromDate, DateTime toDate)
        {
            return new
            {
                AmountsPerCategory = await GetAmountsPerCategory(fromDate, toDate)
            };
        }

        private async Task<object> GetAmountsPerCategory(DateTime fromDate, DateTime toDate)
        {
            var transactions = await _uow.TransactionRepo.GetAllByUserAsync(_currentUserId);
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
                .Select(item => new { Category = categories[item.Key], Amount = item.Value })
                .OrderByDescending(item => item.Amount);
        }
    }
}
