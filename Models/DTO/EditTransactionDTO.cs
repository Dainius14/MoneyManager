using System;
using System.Collections.Generic;

namespace MoneyManager.Models.DTO
{
    public class EditTransactionDTO
    {
        public int? ID { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<EditTransactionDetailsDTO> TransactionDetails { get; set; } = null!;

    }
}
