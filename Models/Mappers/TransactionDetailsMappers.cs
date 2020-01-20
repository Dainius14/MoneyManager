using MoneyManager.Models.Domain;
using MoneyManager.Models.DTO;

namespace MoneyManager.Models.Mappers
{
    public static class TransactionDetailsMappers
    {
        public static GetTransactionDetailsDTO ToGetTransactionDetailsDTO(this TransactionDetails detail)
        {
            return new GetTransactionDetailsDTO
            {
                Id = (int)detail.Id!,
                Amount = detail.Amount,
                Currency = detail.Currency.ToGetCurrencyDTO(),
                Category = detail.Category?.ToGetCategoryDTO(),
                FromAccount = detail.FromAccount?.ToGetAccountDTO(),
                ToAccount = detail.ToAccount.ToGetAccountDTO(),
            };
        }

        public static EditTransactionDetailsDTO ToEditTransactionDetailsDTO(this TransactionDetails detail)
        {
            return new EditTransactionDetailsDTO
            {
                Id = detail.Id,
                Amount = detail.Amount,
                Currency = detail.Currency.Id,
                Category = detail.Category?.Id,
                FromAccount = detail.FromAccount?.Id,
                ToAccount = (int)detail.ToAccount.Id!,
            };
        }

        public static TransactionDetails ToDomainModel(this EditTransactionDetailsDTO dto)
        {
            return new TransactionDetails
            {
                Id = dto.Id,
                FromAccountId = dto.FromAccount,
                ToAccountId = dto.ToAccount,
                CurrencyId = dto.Currency,
                CategoryId = dto.Category,
                Amount = dto.Amount,
            };
        }
        public static TransactionDetails ToDomainModel(this GetTransactionDetailsDTO dto)
        {
            return new TransactionDetails
            {
                Id = dto.Id,
                FromAccountId = dto.FromAccount?.Id,
                FromAccount = dto.FromAccount?.ToDomainModel(),
                ToAccountId = dto.ToAccount.Id,
                ToAccount = dto.ToAccount.ToDomainModel(),
                CurrencyId = dto.Currency.Id,
                Currency = dto.Currency.ToDomainModel(),
                Amount = dto.Amount,
                CategoryId = dto.Category?.Id,
                Category = dto.Category?.ToDomainModel(),
            };
        }

    }
}
