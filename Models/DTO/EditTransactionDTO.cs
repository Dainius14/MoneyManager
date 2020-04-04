using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Models.DTO
{
    public class EditTransactionDTO
    {
        public int? Id { get; set; }
        
        [Required]
        public string? Description { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        [Required]
        public IEnumerable<EditTransactionDetailsDTO> TransactionDetails { get; set; } = null!;

    }
}
