using Dapper;
using MoneyManager.Core.Services;
using MoneyManager.Models.Domain;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Core.Repositories.Dapper
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        public Task<IEnumerable<Transaction>> GetRangeAsync(int from, int count);
    }

    public class DapperTransactionRepository : DapperGenericRepository<Transaction>, ITransactionRepository
    {
        private const string _joinedFromClause = @"
            FROM ""transaction"" AS t
	            LEFT JOIN TransactionDetails as td
		            ON td.TransactionId = t.Id
		        LEFT JOIN  Account as accFrom
			        on td.FromAccountId = accFrom.Id
		        LEFT JOIN Account as accTo
			        on td.ToAccountId = accTo.Id
		        LEFT JOIN Category as cat
			        on td.CategoryId = cat.Id
			        LEFT JOIN Category as catParent
				        on cat.ParentId = catParent.Id";
        public DapperTransactionRepository(IDbTransaction transaction, CurrentUserService currentUserService)
            : base(transaction, "transaction", currentUserService)
        {
        }

        public async Task<IEnumerable<Transaction>> GetRangeAsync(int from, int count)
        {
            string sql = @$"
            SELECT *
            {_joinedFromClause}
            WHERE t.userId=@CurrentUserId
            ORDER BY t.Date DESC, t.CreatedAt DESC
            LIMIT @from, @count
            ";

            var transactionsDict = new Dictionary<int, Transaction>();

            var items = await Connection.QueryAsync<Transaction, TransactionDetails, Account,
                Account, Category, Category, Transaction>(
                sql,
                map: (t, td, fromAccount, toAccount, category, categoryParent) =>
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
                    td.Category = category;

                    if (categoryParent != null)
                    {
                        td.Category.Parent = categoryParent;
                    }

                    transaction.TransactionDetails.Add(td);

                    return transaction;
                },
                param: new { CurrentUserId, from, count },
                transaction: Transaction
            );
            return items.Distinct().ToList();
        }

        new public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await GetRangeAsync(0, -1);
        }

        new public async Task<Transaction> GetAsync(int id)
        {
            string sql = @$"
            SELECT *
            {_joinedFromClause}
            WHERE t.userId=@CurrentUserId AND t.Id=@id
            ";

            var transactionsDict = new Dictionary<int, Transaction>();

            var items = await Connection.QueryAsync<Transaction, TransactionDetails, Account,
                Account, Category, Category, Transaction>(
                sql,
                map: (t, td, fromAccount, toAccount, category, categoryParent) =>
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
                    td.Category = category;

                    if (categoryParent != null)
                    {
                        td.Category.Parent = categoryParent;
                    }

                    transaction.TransactionDetails.Add(td);

                    return transaction;
                },
                param: new { CurrentUserId, id },
                transaction: Transaction
            );
            return items.Distinct().Single();
        }

        //new public async Task<IEnumerable<Transaction>> GetAllAsync()
        //{
        //    string sql = @"
        //    SELECT * FROM ""transaction"" AS t
        //     LEFT JOIN TransactionDetails as td
        //      ON td.TransactionId = t.Id
        //  LEFT JOIN  Account as accFrom
        //   on td.FromAccountId = accFrom.Id
        //  LEFT JOIN Account as accTo
        //   on td.ToAccountId = accTo.Id
        //  LEFT JOIN Category as cat
        //   on td.CategoryId = cat.Id
        //   LEFT JOIN Category as catParent
        //    on cat.ParentId = catParent.Id
        //    ORDER BY t.Date DESC
        //    ";

        //    var transactionsDict = new Dictionary<int, Transaction>();

        //    var items = await Connection.QueryAsync<Transaction, TransactionDetails, Account,
        //        Account, Category, Category, Transaction>(
        //        sql,
        //        map: (t, td, fromAccount, toAccount, category, categoryParent) =>
        //        {
        //            Transaction? transaction;

        //            if (!transactionsDict.TryGetValue((int)t.Id!, out transaction))
        //            {
        //                transaction = t;
        //                transaction.TransactionDetails = new List<TransactionDetails>();
        //                transactionsDict.Add((int)t.Id, transaction);
        //            }
        //            td.FromAccount = fromAccount;
        //            td.ToAccount = toAccount;
        //            td.Category = category;

        //            if (categoryParent != null)
        //            {
        //                td.Category.Parent = categoryParent;
        //            }

        //            transaction.TransactionDetails.Add(td);

        //            return transaction;
        //        },
        //        transaction: Transaction
        //    );
        //    return items.Distinct().ToList();
        //}

        //public Task<int> GetCount()
        //{

        //    string sql = @"
        //    SELECT * FROM ""transaction"" AS t
        //     LEFT JOIN TransactionDetails as td
        //      ON td.TransactionId = t.Id
        //  LEFT JOIN  Account as accFrom
        //   on td.FromAccountId = accFrom.Id
        //  LEFT JOIN Account as accTo
        //   on td.ToAccountId = accTo.Id
        //  LEFT JOIN Category as cat
        //   on td.CategoryId = cat.Id
        //   LEFT JOIN Category as catParent
        //    on cat.ParentId = catParent.Id
        //    ORDER BY t.Date DESC
        //    ";

        //}
    }
}
