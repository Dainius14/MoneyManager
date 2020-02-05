using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyManager.Models.Domain
{
    public class Account : IAudited, ICloneable
    {
        [Column]
        public int? Id { get; set; }
        
        [Column]
        public string Name { get; set; } = null!;
        
        [Column]
        public bool IsPersonal { get; set; }
        
        [Column]
        public DateTime CreatedAt { get; set; }
        
        [Column]
        public DateTime? UpdatedAt { get; set; }

        [Column]
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}