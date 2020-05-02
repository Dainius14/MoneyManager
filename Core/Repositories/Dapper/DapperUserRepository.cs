using Dapper;
using MoneyManager.Core.Services;
using MoneyManager.Models.Domain;
using System.Data;
using System.Threading.Tasks;

namespace MoneyManager.Core.Repositories.Dapper
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task UpdateEmailAsync(string email);
        Task<User> GetCurrentUserAsync();
        Task UpdatePasswordAsync(byte[] passwordHash, byte[] passwordSalt);

    }

    public class DapperUserRepository : DapperGenericRepository<User>, IUserRepository
    {
        public DapperUserRepository(IDbTransaction transaction, CurrentUserService currentUserService)
            : base(transaction, "User", currentUserService)
        {
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await Connection.QuerySingleOrDefaultAsync<User>(
                @$"SELECT * FROM ""{TableName}"" WHERE Email=@email",
                new { email },
                Transaction
            );
        }
        
        public async Task UpdateEmailAsync(string email)
        {
            await Connection.ExecuteAsync(
                @$"
                UPDATE ""{TableName}""
                SET Email=@email
                WHERE Id=@CurrentUserId
                ",
                new { CurrentUserId, email },
                Transaction
            );
        }
        
        public async Task UpdatePasswordAsync(byte[] passwordHash, byte[] passwordSalt)
        {
            await Connection.ExecuteAsync(
                @$"
                UPDATE ""{TableName}""
                SET PasswordHash=@passwordHash, PasswordSalt=@passwordSalt
                WHERE Id=@CurrentUserId
                ",
                new { CurrentUserId, passwordHash, passwordSalt },
                Transaction
            );
        }
        
        public async Task<User> GetCurrentUserAsync()
        {
            return await Connection.QuerySingleOrDefaultAsync<User>(
                @$"SELECT * FROM ""{TableName}"" WHERE Id=@CurrentUserId",
                new { CurrentUserId },
                Transaction
            );
        }
    }
}
