using Microsoft.Extensions.Options;
using MoneyManager.Core.Data;
using MoneyManager.Core.Repositories.Dapper;
using System.Data;
using System.Data.SqlClient;

namespace MoneyManager.Core.Repositories
{
    public class DapperUnitOfWork : IUnitOfWork
    {
        private IOptions<DapperDbContext> _dbContext;
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; private set; }

        private IAccountRepository? _accountRepo;
        public IAccountRepository AccountRepo =>
            _accountRepo ?? (_accountRepo = new DapperAccountRepository(Transaction));

        private ICategoryRepository? _categoryRepo;
        public ICategoryRepository CategoryRepo =>
            _categoryRepo ?? (_categoryRepo = new DapperCategoryRepository(Transaction));


        private ICurrencyRepository? _currencyRepo;
        public ICurrencyRepository CurrencyRepo =>
            _currencyRepo ?? (_currencyRepo = new DapperCurrencyRepository(Transaction));

        private ITransactionRepository? _transactionRepo;
        public ITransactionRepository TransactionRepo =>
            _transactionRepo ?? (_transactionRepo = new DapperTransactionRepository(Transaction));

        private ITransactionDetailsRepository? _transactionDetailsRepo;
        public ITransactionDetailsRepository TransactionDetailsRepo =>
            _transactionDetailsRepo ?? (_transactionDetailsRepo = new DapperTransactionDetailsRepository(Transaction));

        public DapperUnitOfWork(IOptions<DapperDbContext> dbContext)
        {
            _dbContext = dbContext;

            Connection = new SqlConnection(_dbContext.Value.ConnectionString);
            Connection.Open();

            Transaction = Connection.BeginTransaction();
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
            _transactionRepo = null;
            _transactionDetailsRepo = null;
        }
    }
}
