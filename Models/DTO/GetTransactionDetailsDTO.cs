namespace MoneyManager.Models.DTO
{
    public class GetTransactionDetailsDTO
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public GetCategoryDTO? Category { get; set; }
        public GetAccountDTO FromAccount { get; set; } = null!;
        public GetAccountDTO ToAccount { get; set; } = null!;
    }
}
