using MoneyManager.Models.Domain;
using System;
using System.Collections.Generic;

namespace MoneyManager.Client.State
{
    public class AccountsState : ICloneable
    {
        public IList<Account> Accounts { get; set; } = new List<Account>();
        public bool IsLoading { get; set; } = false;
        public bool IsFirstLoadComplete { get; set; } = false;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public class CurrencyState : ICloneable
    {
        public IList<Currency> Currencies { get; set; } = new List<Currency>();
        public bool IsLoading { get; set; } = false;
        public bool IsFirstLoadComplete { get; set; } = false;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public class CategoryState : ICloneable
    {
        public IList<Category> Categories { get; set; } = new List<Category>();
        public bool IsLoading { get; set; } = false;
        public bool IsFirstLoadComplete { get; set; } = false;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
    public class TransactionState : ICloneable
    {
        public IList<Transaction> Transactions { get; set; } = new List<Transaction>();
        public bool IsLoading { get; set; } = false;
        public bool IsFirstLoadComplete { get; set; } = false;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public class UserState : ICloneable
    {
        public User? User { get; set; } = null;

        public bool IsAuthenticated => User != null;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public class AppState
    {
        public AccountsState AccountsState { get; set; } = new AccountsState();
        public CurrencyState CurrencyState { get; set; } = new CurrencyState();
        public CategoryState CategoryState { get; set; } = new CategoryState();
        public TransactionState TransactionState { get; set; } = new TransactionState();
    }
}
