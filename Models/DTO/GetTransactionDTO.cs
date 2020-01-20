using System;
using System.Collections.Generic;

namespace MoneyManager.Models.DTO
{
    public class GetTransactionDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public IEnumerable<GetTransactionDetailsDTO> TransactionDetails { get; set; } = null!;
    }
}
