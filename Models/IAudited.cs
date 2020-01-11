using System;

namespace MoneyManager.Models
{
    public interface IAudited
    {
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
