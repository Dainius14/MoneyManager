using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyManager.Models.Domain
{
    public class Account : IAudited, ICloneable
    {
        [Column]
        public int? ID { get; set; }
        [Column]
        public string Name { get; set; } = null!;
        [Column]
        public bool IsPersonal { get; set; }
        [Column]
        public DateTime CreatedAt { get; set; }
        [Column]
        public DateTime? UpdatedAt { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}