using Dapper;
using MoneyManager.Models.Domain;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Core.Repositories.Dapper
{
    public interface IBalanceHistoryRepository : IGenericRepository<BalanceHistory>
    {
        new public Task<IEnumerable<BalanceHistory>> GetAllByUserAsync(int userId);
        public Task DeleteByTransaction(int transactionId);
    }

    public class DapperBalanceHistoryRepository : DapperGenericRepository<BalanceHistory>, IBalanceHistoryRepository
    {
        public DapperBalanceHistoryRepository(IDbTransaction transaction)
            : base(transaction, "BalanceHistory")
        {
        }
        new public async Task<IEnumerable<BalanceHistory>> GetAllByUserAsync(int userId)
        {
            string sql = $@"
                SELECT *
                FROM ""{TableName}"" AS bh
	                LEFT JOIN ""Transaction"" as t
		                ON bh.TransactionId = t.Id
		            LEFT JOIN  Account as account
			            on bh.AccountId = account.Id
                WHERE t.userId=@userId
                ORDER BY t.Date DESC
                ";

            //var balanceHistoryDict = new Dictionary<int, BalanceHistory>();
            var items = await Connection.QueryAsync<BalanceHistory, Transaction, Account, BalanceHistory>(
                sql,
                map: (balanceHistory, transaction, account) =>
                {
                    //BalanceHistory? balanceHistory;
                    //if (!balanceHistoryDict.TryGetValue((int)bh.Id!, out balanceHistory))
                    //{
                    //    balanceHistory = bh;
                    //    balanceHistoryDict.Add((int)balanceHistory.Id, balanceHistory);
                    //}
                    balanceHistory.Account = account;
                    balanceHistory.Transaction = transaction;
                    return balanceHistory;
                },
                param: new { userId },
                transaction: Transaction
            );
            return items.Distinct().ToList();
        }

        public async Task DeleteByTransaction(int transactionId)
        {
            string sql = $@"
                DELETE FROM ""{TableName}"" WHERE TransactionId=@transactionId
            ";

            await Connection.ExecuteAsync(
                sql,
                new { transactionId },
                Transaction
            );
        }
    }
}
