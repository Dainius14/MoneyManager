using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MoneyManager.Models.Domain
{
    public class TransactionDetails
    {
        [Column]
        public int? Id { get; set; }
        [Column]
        public int TransactionId { get; set; }
        [Column]
        public double Amount { get; set; }
        [Column]
        public int FromAccountId { get; set; }
        [Column]
        public int ToAccountId { get; set; }
        [Column]
        public int? CategoryId { get; set; }


        public Transaction Transaction { get; set; } = null!;
        public Account FromAccount { get; set; } = new Account();
        public Account ToAccount { get; set; } = new Account();
        public Category? Category { get; set; } = new Category();

    }
}