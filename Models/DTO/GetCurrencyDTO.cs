namespace MoneyManager.Models.DTO
{
    public class GetCurrencyDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string IsoCode { get; set; } = null!;
        public string Symbol { get; set; } = null!;
    }
}
