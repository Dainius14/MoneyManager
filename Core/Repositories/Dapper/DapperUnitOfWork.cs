using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using MoneyManager.Core.Data;
using MoneyManager.Core.Repositories.Dapper;
using MoneyManager.Core.Services;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MoneyManager.Core.Repositories
{
    public interface IUnitOfWork
    {
        public IAccountRepository AccountRepo { get; }
        public ICategoryRepository CategoryRepo { get; }
        public IRefreshTokenRepository RefreshTokenRepo { get; }
        public ITransactionRepository TransactionRepo { get; }
        public ITransactionDetailsRepository TransactionDetailsRepo { get; }
        public IUserRepository UserRepo { get; }

        public void Commit();
    }

    public class DapperUnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IOptions<DapperDbContext> _dbContext;
        private readonly CurrentUserService _currentUserService;

        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; private set; }


        private IAccountRepository? _accountRepo;
        public IAccountRepository AccountRepo =>
            _accountRepo ?? (_accountRepo = new DapperAccountRepository(Transaction, _currentUserService));

        private ICategoryRepository? _categoryRepo;
        public ICategoryRepository CategoryRepo =>
            _categoryRepo ?? (_categoryRepo = new DapperCategoryRepository(Transaction, _currentUserService));
       
        private IRefreshTokenRepository? _refreshTokenRepo;
        public IRefreshTokenRepository RefreshTokenRepo =>
            _refreshTokenRepo ?? (_refreshTokenRepo = new DapperRefreshTokenRepository(Transaction, _currentUserService));

        private ITransactionRepository? _transactionRepo;
        public ITransactionRepository TransactionRepo =>
            _transactionRepo ?? (_transactionRepo = new DapperTransactionRepository(Transaction, _currentUserService));

        private ITransactionDetailsRepository? _transactionDetailsRepo;
        public ITransactionDetailsRepository TransactionDetailsRepo =>
            _transactionDetailsRepo ?? (_transactionDetailsRepo = new DapperTransactionDetailsRepository(Transaction, _currentUserService));

        private IUserRepository? _userRepo;
        public IUserRepository UserRepo =>
            _userRepo ?? (_userRepo = new DapperUserRepository(Transaction, _currentUserService));

        public DapperUnitOfWork(IOptions<DapperDbContext> dbContext, CurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;

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
