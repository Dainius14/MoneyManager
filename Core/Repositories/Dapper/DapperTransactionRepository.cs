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

        new public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            string sql = @"
            SELECT * FROM ""transaction"" AS t
	            LEFT JOIN TransactionDetails as td
		            ON td.TransactionID = t.ID
		        LEFT JOIN  Account as accFrom
			        on td.FromAccountID = accFrom.ID
		        LEFT JOIN Account as accTo
			        on td.ToAccountID = accTo.ID
		        LEFT  JOIN Currency as crnc
			        on td.CurrencyID = crnc.ID
		        LEFT JOIN Category as cat
			        on td.CategoryID = cat.ID
			        LEFT JOIN Category as catParent
				        on cat.ParentID = catParent.ID
            ";

            var transactionsDict = new Dictionary<int, Transaction>();

            var items = await Connection.QueryAsync<Transaction, TransactionDetails, Account,
                Account, Currency, Category, Category, Transaction>(
                sql,
                map: (t, td, fromAccount, toAccount, currency, category, categoryParent) =>
                {
                    Transaction? transaction;

                    if (!transactionsDict.TryGetValue((int)t.ID!, out transaction))
                    {
                        transaction = t;
                        transaction.TransactionDetails = new List<TransactionDetails>();
                        transactionsDict.Add((int)t.ID, transaction);
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
                splitOn: "ID,ID,ID,ID,ID,ID",
                transaction: Transaction
            );
            return items.Distinct().ToList();
        }
    }
}
