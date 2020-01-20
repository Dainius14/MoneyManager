using Dapper;
using MoneyManager.Models.Domain;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Core.Repositories.Dapper
{
    public class DapperTransactionDetailsRepository : DapperGenericRepository<TransactionDetails>, ITransactionDetailsRepository
    {
        public DapperTransactionDetailsRepository(IDbTransaction transaction)
            : base(transaction, "transactiondetails")
        {
        }

        new public async Task<TransactionDetails> GetAsync(int id)
        {
            string sql = @"
            SELECT * FROM ""TransactionDetails"" AS td
		        LEFT OUTER JOIN Account as accFrom
			        on td.FromAccountId = accFrom.Id
		        LEFT OUTER JOIN Account as accTo
			        on td.ToAccountId = accTo.Id
		        LEFT OUTER JOIN Currency as crnc
			        on td.CurrencyId = crnc.Id
		        LEFT OUTER JOIN Category as cat
			        on td.CategoryId = cat.Id
			        LEFT OUTER JOIN Category as catParent
				        on cat.ParentId = catParent.Id
                WHERE td.Id = @id
            ";


            var item = await Connection.QueryAsync<TransactionDetails, Account, Account, Currency,
                Category, Category, TransactionDetails> (
                sql,
                param: new { id },
                map: (td, fromAccount, toAccount, currency, category, categoryParent) =>
                {
                    td.FromAccount = fromAccount.Id != default ? fromAccount : null;
                    td.ToAccount = toAccount;
                    td.Currency = currency;
                    td.Category = category;

                    if (categoryParent != null)
                    {
                        td.Category.Parent = categoryParent;
                    }

                    return td;
                },
                splitOn: "Id,Id,Id,Id,Id",
                transaction: Transaction
            );
            return item.FirstOrDefault();
        }
    }
}
