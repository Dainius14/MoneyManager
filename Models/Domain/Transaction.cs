using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MoneyManager.Models.Domain
{
    public class Transaction : IAudited
    {
        [Column]
        public int? Id { get; set; }
        [Column]
        public string? Description { get; set; }
        [Column]
        public DateTime Date { get; set; } = DateTime.UtcNow.Date;
        [Column]
        public DateTime CreatedAt { get; set; }
        [Column]
        public DateTime? UpdatedAt { get; set; }
        [Column]
        public int UserId { get; set; }
        [Column]
        public string Type { get; set; } = null!;

        public User User { get; set; } = null!;

        public double Amount
        {
            get => TransactionDetails.Sum(td => td.Amount);
        }

        public IList<TransactionDetails> TransactionDetails { get; set; } = new List<TransactionDetails>()
        {
            new TransactionDetails()
        };





        public double FromAccountBalance { get; set; }
        public double ToAccountBalance { get; set; }
    }
    public class TransactionType
    {
        public const string Expense = "expense";
        public const string Income = "income";
        public const string Transfer = "transfer";
    }

}