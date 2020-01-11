using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MoneyManager.Models.Domain
{
    public class Transaction : IAudited
    {
        [Column]
        public int? ID { get; set; }
        [Column]
        public string? Description { get; set; }
        [Column]
        public DateTime Date { get; set; } = DateTime.Now.Date;
        [Column]
        public DateTime CreatedAt { get; set; }
        [Column]
        public DateTime? UpdatedAt { get; set; }

        public double Amount
        {
            get => TransactionDetails.Sum(td => td.AmountCents) / (double)100;
        }

        public string AmountStr => string.Format("{0:0,0.00}", Amount);

        public string FromAccountName =>
            TransactionDetails.FirstOrDefault()?.FromAccount?.Name ?? string.Empty;

        public string ToAccountName =>
            string.Join(", ", TransactionDetails.Select(td => td.ToAccount.Name));

        public string CategoryName =>
            string.Join(", ", TransactionDetails.Select(td => td.Category?.Name).Where(c => !string.IsNullOrEmpty(c)));

        public IList<TransactionDetails> TransactionDetails { get; set; } = new List<TransactionDetails>()
        {
            new TransactionDetails()
        };

        public TransactionTypeEnum? TransactionType
        {
            get
            {
                bool? isFromPersonal = TransactionDetails.FirstOrDefault()?.FromAccount?.IsPersonal;
                bool? isToPersonal = TransactionDetails.FirstOrDefault()?.ToAccount?.IsPersonal;

                if (isFromPersonal == null || isToPersonal == null)
                {
                    return null;
                }
                else if ((bool)isFromPersonal && (bool)!isToPersonal)
                {
                    return TransactionTypeEnum.Expense;
                }
                else if ((bool)!isFromPersonal && (bool)isToPersonal)
                {
                    return TransactionTypeEnum.Income;
                }
                else if ((bool)isFromPersonal && (bool)isToPersonal)
                {
                    return TransactionTypeEnum.Transfer;
                }
                else
                {
                    return TransactionTypeEnum.Invalid;
                }
            }
        }


    }

    public enum TransactionTypeEnum
    {
        Income,
        Expense,
        Transfer,
        Invalid
    }

}