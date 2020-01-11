using System.Data;

namespace MoneyManager.Core.Repositories
{
    public interface IUnitOfWork
    {
        public IAccountRepository AccountRepo { get; }
        public ICategoryRepository CategoryRepo { get; }
        public ICurrencyRepository CurrencyRepo { get; }
        public ITransactionRepository TransactionRepo { get; }
        public ITransactionDetailsRepository TransactionDetailsRepo{ get; }

        public void Commit();
    }
}
