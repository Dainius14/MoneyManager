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

        protected string TableName { get; }

        protected DapperGenericRepository(IDbTransaction transaction, string tableName)
        {
            Transaction = transaction;
            TableName = tableName;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Connection.QueryAsync<T>(
                @$"
                SELECT *
                FROM ""{TableName}""
                ",
                transaction: Transaction
            );
        }

        public async Task<IEnumerable<T>> GetAllByUserAsync(int userId)
        {
            return await Connection.QueryAsync<T>(
                @$"
                SELECT *
                FROM ""{TableName}""
                WHERE UserId=@userId
                ",
                new { userId },
                transaction: Transaction
            );
        }

        public async Task<T> GetAsync(int id)
        {
            return await Connection.QuerySingleOrDefaultAsync<T>(
                @$"
                SELECT *
                FROM ""{TableName}""
                WHERE Id=@id
                ",
                new { id },
                Transaction
            );
        }

        public async Task<T> GetByUserAsync(int userId, int id)
        {
            return await Connection.QuerySingleOrDefaultAsync<T>(
                @$"
                SELECT *
                FROM ""{TableName}""
                WHERE UserId=@userId AND Id=@id
                ",
                new { userId, id },
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
            var insertQuery = new StringBuilder(@$"INSERT INTO ""{TableName}"" ");

            insertQuery.Append("(");

            var properties = GenerateListOfProperties(GetProperties);
            properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append("); SELECT last_insert_rowid();");

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
            var updateQuery = new StringBuilder(@$"UPDATE ""{TableName}"" SET ");
            var properties = GenerateListOfProperties(GetProperties);

            properties.ForEach(property =>
            {
                if (!property.Equals("Id"))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });

            updateQuery.Remove(updateQuery.Length - 1, 1); //remove last comma
            updateQuery.Append(" WHERE Id=@Id");

            return updateQuery.ToString();
        }

        public async Task DeleteAsync(int id)
        {
            await Connection.ExecuteAsync(
                @$"DELETE FROM ""{TableName}"" WHERE Id=@id",
                new { id },
                Transaction
            );
        }

        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();

        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let isDbColumn = prop.GetCustomAttributes(typeof(ColumnAttribute)).FirstOrDefault() != null
                    where isDbColumn && prop.Name != "Id"
                    select prop.Name).ToList();
        }
    }
}
