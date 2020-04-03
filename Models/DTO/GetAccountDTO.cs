using System;

namespace MoneyManager.Models.DTO
{
    public class GetAccountDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsPersonal { get; set; }
        public string CreatedAt { get; set; } = null!;
        public string? UpdatedAt { get; set; }
        public double OpeningBalance { get; set; }
        public string OpeningDate { get; set; } = null!;
        public double CurrentBalance { get; set; }
    }
}
