namespace MoneyManager.Models.DTO
{
    public class GetCurrencyDTO
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string Symbol { get; set; } = null!;
    }
}
