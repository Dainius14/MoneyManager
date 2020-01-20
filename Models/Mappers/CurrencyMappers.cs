using MoneyManager.Models.Domain;
using MoneyManager.Models.DTO;

namespace MoneyManager.Models.Mappers
{
    public static class CurrencyMappers
    {
        public static GetCurrencyDTO ToGetCurrencyDTO(this Currency currency)
        {
            return new GetCurrencyDTO
            {
                Id = currency.Id,
                Name = currency.Name,
                IsoCode = currency.IsoCode,
                Symbol = currency.Symbol,
            };
        }
        public static Currency ToDomainModel(this GetCurrencyDTO dto)
        {
            return new Currency
            {
                Id = dto.Id,
                Name = dto.Name,
                IsoCode = dto.IsoCode,
                Symbol = dto.Symbol,
            };
        }
    }
}
