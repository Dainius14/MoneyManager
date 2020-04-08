using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Models.DTO
{
    public class EditTransactionDetailsDTO
    {
        public int? Id { get; set; }

        [Required]
        public double? Amount { get; set; }

        public int? Category { get; set; }

        [Required]
        public int? FromAccount { get; set; }

        [Required]
        public int? ToAccount { get; set; }
    }
}
