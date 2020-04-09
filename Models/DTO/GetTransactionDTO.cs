using System;
using System.Collections.Generic;

namespace MoneyManager.Models.DTO
{
    public class GetTransactionDTO
    {
        public int Id { get; set; }
        public string Date { get; set; } = null!;
        public string? Description { get; set; }
        public double Amount { get; set; }
        public string Type { get; set; } = null!;
        public string CreatedAt { get; set; } = null!;
        public string? UpdatedAt { get; set; }
        public IEnumerable<GetTransactionDetailsDTO> TransactionDetails { get; set; } = null!;
    }
}
