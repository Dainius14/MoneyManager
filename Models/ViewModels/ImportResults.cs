using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyManager.Models.ViewModels
{
    public class ImportResults
    {
        public int CreatedTransactions { get; set; }
        public int CreatedPersonalAccounts { get; set; }
        public int CreatedOtherAccounts { get; set; }
        public int CreatedCategories { get; set; }

        public ImportResults(int transactions, int personalAccounts, int otherAccounts, int categories)
        {
            CreatedTransactions = transactions;
            CreatedPersonalAccounts = personalAccounts;
            CreatedOtherAccounts = otherAccounts;
            CreatedCategories = categories;
        }
    }
}
