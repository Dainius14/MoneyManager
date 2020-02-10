using MoneyManager.Models.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Models.ViewModels
{
    public class EditPersonalAccountVm
    {
        public int? Id { get; set; } = null;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public double InitialBalance { get; set; } = 0;

        [Required]
        public Currency? Currency { get; set; } = new Currency { Id = 1 };
        public int? CurrencyId { get; set; }

        [Required]
        public DateTime InitialDate { get; set; } = DateTime.Now;

        public EditPersonalAccountVm()
        {
        }

        public EditPersonalAccountVm(int? id, string name, int currencyId, double initialBalance, DateTime initialDate)
        {
            Id = id;
            Name = name;
            CurrencyId = currencyId;
            InitialBalance = initialBalance;
            InitialDate = initialDate;
        }
    }
    public class EditPersonalAccountVmDto
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public int CurrencyId { get; set; }

        public double InitialBalance { get; set; }

        public DateTime InitialDate { get; set; }

        public EditPersonalAccountVmDto()
        {
        }

        public EditPersonalAccountVmDto(string name, int currencyId, double initialBalance, DateTime initialDate)
        {
            Id = null;
            Name = name;
            CurrencyId = currencyId;
            InitialBalance = initialBalance;
            InitialDate = initialDate;
        }

        public EditPersonalAccountVmDto(int? id, string name, int currencyId, double initialBalance, DateTime initialDate)
            : this(name, currencyId, initialBalance, initialDate)
        {
            Id = id;
        }
    }
}
