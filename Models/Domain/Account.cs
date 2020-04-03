using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyManager.Models.Domain
{
    public class Account : IAudited
    {
        [Column]
        public int? Id { get; set; }
        
        [Column]
        public string Name { get; set; } = null!;

        [Column]
        public bool IsPersonal { get; set; }

        [Column]
        public double OpeningBalance { get; set; }

        [Column]
        public DateTime OpeningDate { get; set; }

        [Column]
        public DateTime CreatedAt { get; set; }
        
        [Column]
        public DateTime? UpdatedAt { get; set; }

        [Column]
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public Account()
        {
        }

        public Account(Account account)
        {
            Id = account.Id;
            Name = account.Name;
            IsPersonal = account.IsPersonal;
            OpeningBalance = account.OpeningBalance;
            OpeningDate = account.OpeningDate;
            CreatedAt = account.CreatedAt;
            UpdatedAt = account.UpdatedAt;
            UserId = account.UserId;
            User = account.User;
        }
    }
}