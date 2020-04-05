using MoneyManager.Models.Domain;
using MoneyManager.Models.DTO;
using System;
using System.Linq;

namespace MoneyManager.Models.Mappers
{
    public static class TransactionMappers
    {
        public static GetTransactionDTO ToGetTransactionDTO(this Transaction transaction)
        {
            return new GetTransactionDTO
            {
                Id = (int)transaction.Id!,
                Date = transaction.Date.ToISODateString(),
                Description = transaction.Description,
                CreatedAt = transaction.CreatedAt.ToISOString(),
                UpdatedAt= transaction.UpdatedAt?.ToISOString(),
                TransactionDetails = transaction.TransactionDetails.Select(td => td.ToGetTransactionDetailsDTO()),
            };
        }

        public static EditTransactionDTO ToEditTransactionDTO(this Transaction transaction)
        {
            return new EditTransactionDTO
            {
                Id = transaction.Id,
                Date = transaction.Date,
                Description = transaction.Description ?? string.Empty,
                TransactionDetails = transaction.TransactionDetails.Select(td => td.ToEditTransactionDetailsDTO()),
            };
        }

        public static Transaction ToDomainModel(this EditTransactionDTO dto)
        {
            return new Transaction
            {
                Id = dto.Id,
                Description = dto.Description,
                Date = (DateTime)dto.Date!,
                TransactionDetails = dto.TransactionDetails.Select(td => td.ToDomainModel()).ToList(),
            };
        }

        public static Transaction ToDomainModel(this GetTransactionDTO dto)
        {
            return new Transaction
            {
                Id = dto.Id,
                Description = dto.Description,
                Date = DateTime.Parse(dto.Date),
                TransactionDetails = dto.TransactionDetails.Select(td => td.ToDomainModel()).ToList(),
            };
        }
    }
}
