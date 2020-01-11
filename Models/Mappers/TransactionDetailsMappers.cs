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
                ID = (int)detail.ID!,
                Amount = detail.AmountCents,
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
                ID = detail.ID,
                Amount = detail.AmountCents,
                Currency = detail.Currency.ID,
                Category = detail.Category?.ID,
                FromAccount = detail.FromAccount?.ID,
                ToAccount = (int)detail.ToAccount.ID!,
            };
        }

        public static TransactionDetails ToDomainModel(this EditTransactionDetailsDTO dto)
        {
            return new TransactionDetails
            {
                ID = dto.ID,
                FromAccountID = dto.FromAccount,
                ToAccountID = dto.ToAccount,
                CurrencyID = dto.Currency,
                CategoryID = dto.Category,
                AmountCents = (int)(dto.Amount * 100),
            };
        }
        public static TransactionDetails ToDomainModel(this GetTransactionDetailsDTO dto)
        {
            return new TransactionDetails
            {
                ID = dto.ID,
                FromAccountID = dto.FromAccount?.ID,
                FromAccount = dto.FromAccount?.ToDomainModel(),
                ToAccountID = dto.ToAccount.ID,
                ToAccount = dto.ToAccount.ToDomainModel(),
                CurrencyID = dto.Currency.ID,
                Currency = dto.Currency.ToDomainModel(),
                AmountCents = (int)(dto.Amount * 100),
                CategoryID = dto.Category?.ID,
                Category = dto.Category?.ToDomainModel(),
            };
        }

    }
}
