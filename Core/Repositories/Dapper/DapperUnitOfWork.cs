using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using MoneyManager.Core.Data;
using MoneyManager.Core.Repositories.Dapper;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MoneyManager.Core.Repositories
{
    public interface IUnitOfWork
    {
        public IAccountRepository AccountRepo { get; }
        public IBalanceHistoryRepository BalanceHistoryRepo { get; }
        public ICategoryRepository CategoryRepo { get; }
        public IRefreshTokenRepository RefreshTokenRepo { get; }
        public ITransactionRepository TransactionRepo { get; }
        public ITransactionDetailsRepository TransactionDetailsRepo { get; }
        public IUserRepository UserRepo { get; }

        public void Commit();
    }

    public class DapperUnitOfWork : IUnitOfWork, IDisposable
    {
        private IOptions<DapperDbContext> _dbContext;
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; private set; }


        private IAccountRepository? _accountRepo;
        public IAccountRepository AccountRepo =>
            _accountRepo ?? (_accountRepo = new DapperAccountRepository(Transaction));

        private IBalanceHistoryRepository? _balanceHistoryRepo;
        public IBalanceHistoryRepository BalanceHistoryRepo =>
            _balanceHistoryRepo ?? (_balanceHistoryRepo = new DapperBalanceHistoryRepository(Transaction));

        private ICategoryRepository? _categoryRepo;
        public ICategoryRepository CategoryRepo =>
            _categoryRepo ?? (_categoryRepo = new DapperCategoryRepository(Transaction));

        private IRefreshTokenRepository? _refreshTokenRepo;
        public IRefreshTokenRepository RefreshTokenRepo =>
            _refreshTokenRepo ?? (_refreshTokenRepo = new DapperRefreshTokenRepository(Transaction));

        private ITransactionRepository? _transactionRepo;
        public ITransactionRepository TransactionRepo =>
            _transactionRepo ?? (_transactionRepo = new DapperTransactionRepository(Transaction));

        private ITransactionDetailsRepository? _transactionDetailsRepo;
        public ITransactionDetailsRepository TransactionDetailsRepo =>
            _transactionDetailsRepo ?? (_transactionDetailsRepo = new DapperTransactionDetailsRepository(Transaction));

        private IUserRepository? _userRepo;
        public IUserRepository UserRepo =>
            _userRepo ?? (_userRepo = new DapperUserRepository(Transaction));

        public DapperUnitOfWork(IOptions<DapperDbContext> dbContext)
        {
            _dbContext = dbContext;

            Connection = new SqliteConnection(_dbContext.Value.ConnectionString);
            Connection.Open();

            Transaction = Connection.BeginTransaction();
        }

        ~DapperUnitOfWork()
        {
            Dispose();
        }

        public void Commit()
        {
            try
            {
                Transaction.Commit();
            }
            catch
            {
                Transaction.Rollback();
                throw;
            }
            finally
            {
                Transaction.Dispose();
                Transaction = Connection.BeginTransaction();
                ResetRepositories();
            }
        }

        private void ResetRepositories()
        {
            _accountRepo = null;
            _categoryRepo = null;
            _refreshTokenRepo = null;
            _transactionRepo = null;
            _transactionDetailsRepo = null;
            _userRepo = null;
        }

        public void Dispose()
        {
            try
            {
                Transaction.Commit();
            }
            catch
            {
                Transaction.Rollback();
                throw;
            }
            finally
            {
                Transaction.Dispose();
                GC.SuppressFinalize(this);
            }
        }
    }
}
