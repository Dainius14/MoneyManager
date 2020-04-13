using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoneyManager.Models.Domain
{
    public class BalanceHistory
    {
        [Column]
        public int Id { get; set; }

        [Column]
        public int TransactionId { get; set; }

        [Column]
        public int AccountId { get; set; }

        [Column]
        public double Balance { get; set; }


        public Transaction Transaction { get; set; }

        public Account Account { get; set; }
    }
}
