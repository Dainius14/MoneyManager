using Dapper;
using MoneyManager.Core.Services;
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
    public interface IGenericRepository<T>
    {
        public Task<IEnumerable<T>> GetAllAsync();

        public Task<T> GetAsync(int id);
        public Task<T> GetAsync(string col, string value);
        public Task<T> GetWithoutUserAsync(int id);

        public Task<int> SaveRangeAsync(IEnumerable<T> list);

        public Task UpdateAsync(T t);

        public Task<int> InsertAsync(T t);

        public Task DeleteAsync(int id);
        public Task<bool> ExistsAsync(int id);
    }

    public abstract class DapperGenericRepository<T> : IGenericRepository<T>
    {
        private readonly CurrentUserService _currentUserService;

        protected IDbTransaction Transaction { get; }
        protected IDbConnection Connection { get => Transaction.Connection; }
        protected string TableName { get; }
        protected int CurrentUserId => _currentUserService.Id;

        protected DapperGenericRepository(IDbTransaction transaction, string tableName, CurrentUserService currentUserService)
        {
            Transaction = transaction;
            TableName = tableName;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Connection.QueryAsync<T>(
                @$"
                SELECT *
                FROM ""{TableName}""
                WHERE UserId=@CurrentUserId
                ",
                new { CurrentUserId },
                transaction: Transaction
            );
        }

        //public async Task<IEnumerable<T>> GetAllAsync(int userId)
        //{
        //    return await Connection.QueryAsync<T>(
        //        @$"
        //        SELECT *
        //        FROM ""{TableName}""
        //        WHERE UserId=@userId
        //        ",
        //        new { userId },
        //        transaction: Transaction
        //    );
        //}

        public async Task<T> GetAsync(int id)
        {
            return await Connection.QuerySingleOrDefaultAsync<T>(
                @$"
                SELECT *
                FROM ""{TableName}""
                WHERE UserId=@CurrentUserId AND Id=@id
                ",
                new { CurrentUserId, id },
                Transaction
            );
        }
        public async Task<T> GetWithoutUserAsync(int id)
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

        //public async Task<T> GetByUserAsync(int userId, int id)
        //{
        //    return await Connection.QuerySingleOrDefaultAsync<T>(
        //        @$"
        //        SELECT *
        //        FROM ""{TableName}""
        //        WHERE UserId=@userId AND Id=@id
        //        ",
        //        new { userId, id },
        //        Transaction
        //    );
        //}
        public async Task<T> GetAsync(string col, string value)
        {
            return await Connection.QuerySingleOrDefaultAsync<T>(
                @$"
                SELECT *
                FROM ""{TableName}""
                WHERE UserId=@CurrentUserId AND {col}=@value
                ",
                new { CurrentUserId, value },
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
                @$"
                DELETE
                FROM ""{TableName}""
                WHERE UserId=@CurrentUserId AND Id=@id
                ",
                new { CurrentUserId, id },
                Transaction
            );
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await Connection.ExecuteScalarAsync<bool>(
                @$"
                SELECT COUNT(*)
                FROM ""{TableName}""
                WHERE UserId=@CurrentUserId AND Id=@id
                ",
                new { CurrentUserId, id },
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
