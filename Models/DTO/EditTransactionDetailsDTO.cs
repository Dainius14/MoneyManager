namespace MoneyManager.Models.DTO
{
    public class EditTransactionDetailsDTO
    {
        public int? ID { get; set; }
        public double Amount { get; set; }
        public int Currency { get; set; }
        public int? Category { get; set; }
        public int? FromAccount { get; set; }
        public int ToAccount { get; set; }
    }
}
