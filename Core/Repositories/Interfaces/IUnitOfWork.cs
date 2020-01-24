using System.Data;

namespace MoneyManager.Core.Repositories
{
    public interface IUnitOfWork
    {
        public IAccountRepository AccountRepo { get; }
        public ICategoryRepository CategoryRepo { get; }
        public ICurrencyRepository CurrencyRepo { get; }
        public IRefreshTokenRepository RefreshTokenRepo { get; }
        public ITransactionRepository TransactionRepo { get; }
        public ITransactionDetailsRepository TransactionDetailsRepo { get; }
        public IUserRepository UserRepo { get; }

        public void Commit();
    }
}
