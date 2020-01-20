using System;

namespace MoneyManager.Models.DTO
{
    public class GetAccountDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsPersonal { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
