using Dapper;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Core.Repositories.Dapper
{
    public abstract class DapperGenericRepository<T> : IGenericRepository<T>
    {
        protected IDbTransaction Transaction { get; }
        protected IDbConnection Connection { get => Transaction.Connection; }

        private readonly string _tableName;

        protected DapperGenericRepository(IDbTransaction transaction, string tableName)
        {
            Transaction = transaction;
            _tableName = tableName;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Connection.QueryAsync<T>(
                $"SELECT * FROM \"{_tableName}\"",
                transaction: Transaction
            );
        }

        public async Task<T> GetAsync(int id)
        {
            return await Connection.QuerySingleOrDefaultAsync<T>(
                $"SELECT * FROM \"{_tableName}\" WHERE ID=@ID",
                new { ID = id },
                Transaction
            );
        }
        public async Task<int> InsertAsync(T t)
        {
            var insertQuery = GenerateInsertQuery();
            return await Connection.QuerySingleAsync<int>(
                insertQuery,
                t,
                Transaction
            );
        }

        private string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO \"{_tableName}\" ");

            insertQuery.Append("(");

            var properties = GenerateListOfProperties(GetProperties);
            properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") OUTPUT INSERTED.[ID] VALUES (");

            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");

            return insertQuery.ToString();
        }

        public async Task<int> SaveRangeAsync(IEnumerable<T> list)
        {
            var inserted = 0;
            var query = GenerateInsertQuery();

            inserted += await Connection.ExecuteAsync(
                query,
                list,
                Transaction
            );

            return inserted;
        }

        public async Task UpdateAsync(T t)
        {
            var updateQuery = GenerateUpdateQuery();
            await Connection.ExecuteAsync(
                updateQuery,
                t,
                Transaction
            );
        }


        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {_tableName} SET ");
            var properties = GenerateListOfProperties(GetProperties);

            properties.ForEach(property =>
            {
                if (!property.Equals("ID"))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });

            updateQuery.Remove(updateQuery.Length - 1, 1); //remove last comma
            updateQuery.Append(" WHERE ID=@ID");

            return updateQuery.ToString();
        }
        public async Task DeleteAsync(int id)
        {
            await Connection.ExecuteAsync(
                $"DELETE FROM \"{_tableName}\" WHERE ID=@ID",
                new { ID = id },
                Transaction
            );
        }

        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();

        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let isDbColumn = prop.GetCustomAttributes(typeof(ColumnAttribute)).FirstOrDefault() != null
                    where isDbColumn && prop.Name != "ID"
                    select prop.Name).ToList();
        }
    }
}
