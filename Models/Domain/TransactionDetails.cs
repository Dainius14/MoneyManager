using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MoneyManager.Models.Domain
{
    public class TransactionDetails : IAudited
    {
        [Column]
        public int? Id { get; set; }
        [Column]
        public int TransactionId { get; set; }
        [Column]
        public double Amount { get; set; }
        [Column]
        public int CurrencyId { get; set; }
        [Column]
        public int? FromAccountId { get; set; }
        [Column]
        public int ToAccountId { get; set; }
        [Column]
        public int? CategoryId { get; set; }
        [Column]
        public DateTime CreatedAt { get; set; }

        [Column]
        public DateTime? UpdatedAt { get; set; }

        public Transaction Transaction { get; set; } = null!;
        public Account? FromAccount { get; set; } = new Account();
        public Account ToAccount { get; set; } = new Account();
        public Currency Currency { get; set; } = new Currency()!;
        public Category? Category { get; set; } = new Category();

    }

    public class TransactionDetailsIDComparator : IEqualityComparer<TransactionDetails>
    {
        public bool Equals([AllowNull] TransactionDetails x, [AllowNull] TransactionDetails y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] TransactionDetails obj)
        {
            return obj.GetHashCode();
        }
    }
}