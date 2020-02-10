using Dapper;
using MoneyManager.Models.Domain;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Core.Repositories.Dapper
{
    public class DapperTransactionRepository : DapperGenericRepository<Transaction>, ITransactionRepository
    {
        public DapperTransactionRepository(IDbTransaction transaction)
            : base(transaction, "transaction")
        {
        }

        new public async Task<IEnumerable<Transaction>> GetAllByUserAsync(int userId)
        {
            string sql = @"
            SELECT *
            FROM ""transaction"" AS t
	            LEFT JOIN TransactionDetails as td
		            ON td.TransactionId = t.Id
		        LEFT JOIN  Account as accFrom
			        on td.FromAccountId = accFrom.Id
		        LEFT JOIN Account as accTo
			        on td.ToAccountId = accTo.Id
		        LEFT  JOIN Currency as crnc
			        on td.CurrencyId = crnc.Id
		        LEFT JOIN Category as cat
			        on td.CategoryId = cat.Id
			        LEFT JOIN Category as catParent
				        on cat.ParentId = catParent.Id
            WHERE t.userId=@userId
            ORDER BY t.Date DESC
            ";

            var transactionsDict = new Dictionary<int, Transaction>();

            var items = await Connection.QueryAsync<Transaction, TransactionDetails, Account,
                Account, Currency, Category, Category, Transaction>(
                sql,
                map: (t, td, fromAccount, toAccount, currency, category, categoryParent) =>
                {
                    Transaction? transaction;

                    if (!transactionsDict.TryGetValue((int)t.Id!, out transaction))
                    {
                        transaction = t;
                        transaction.TransactionDetails = new List<TransactionDetails>();
                        transactionsDict.Add((int)t.Id, transaction);
                    }
                    td.FromAccount = fromAccount;
                    td.ToAccount = toAccount;
                    td.Currency = currency;
                    td.Category = category;

                    if (categoryParent != null)
                    {
                        td.Category.Parent = categoryParent;
                    }

                    transaction.TransactionDetails.Add(td);

                    return transaction;
                },
                param: new { userId },
                transaction: Transaction
            );
            return items.Distinct().ToList();
        }

        new public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            string sql = @"
            SELECT * FROM ""transaction"" AS t
	            LEFT JOIN TransactionDetails as td
		            ON td.TransactionId = t.Id
		        LEFT JOIN  Account as accFrom
			        on td.FromAccountId = accFrom.Id
		        LEFT JOIN Account as accTo
			        on td.ToAccountId = accTo.Id
		        LEFT  JOIN Currency as crnc
			        on td.CurrencyId = crnc.Id
		        LEFT JOIN Category as cat
			        on td.CategoryId = cat.Id
			        LEFT JOIN Category as catParent
				        on cat.ParentId = catParent.Id
            ORDER BY t.Date DESC
            ";

            var transactionsDict = new Dictionary<int, Transaction>();

            var items = await Connection.QueryAsync<Transaction, TransactionDetails, Account,
                Account, Currency, Category, Category, Transaction>(
                sql,
                map: (t, td, fromAccount, toAccount, currency, category, categoryParent) =>
                {
                    Transaction? transaction;

                    if (!transactionsDict.TryGetValue((int)t.Id!, out transaction))
                    {
                        transaction = t;
                        transaction.TransactionDetails = new List<TransactionDetails>();
                        transactionsDict.Add((int)t.Id, transaction);
                    }
                    td.FromAccount = fromAccount;
                    td.ToAccount = toAccount;
                    td.Currency = currency;
                    td.Category = category;

                    if (categoryParent != null)
                    {
                        td.Category.Parent = categoryParent;
                    }

                    transaction.TransactionDetails.Add(td);

                    return transaction;
                },
                splitOn: "Id,Id,Id,Id,Id,Id",
                transaction: Transaction
            );
            return items.Distinct().ToList();
        }
    }
}
