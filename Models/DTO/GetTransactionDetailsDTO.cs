namespace MoneyManager.Models.DTO
{
    public class GetTransactionDetailsDTO
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public GetCurrencyDTO Currency { get; set; } = null!;
        public GetCategoryDTO? Category { get; set; }
        public GetAccountDTO? FromAccount { get; set; }
        public GetAccountDTO ToAccount { get; set; } = null!;
    }
}
