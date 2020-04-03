using MoneyManager.Models.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Models.ViewModels
{
    public class EditAccountVm
    {
        public string? Name { get; set; }


        [Range(0, double.MaxValue)]
        public double? OpeningBalance { get; set; }

        public DateTime? OpeningDate { get; set; }
    }
    public class CreateAccountVm : EditAccountVm
    {
        [Required]
        new public string? Name { get; set; }

        [Required]
        public bool? IsPersonal { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        new public double? OpeningBalance { get; set; }

        [Required]
        new public DateTime? OpeningDate { get; set; }
    }

    public class EditPersonalAccountVmDto
    {
        public int? Id { get; set; }

        public string Name { get; set; } = null!;

        public int CurrencyId { get; set; }

        public double OpeningBalance { get; set; }

        public DateTime OpeningDate { get; set; }

        public EditPersonalAccountVmDto()
        {
        }

        public EditPersonalAccountVmDto(string name, double initialBalance, DateTime initialDate)
        {
            Id = null;
            Name = name;
            OpeningBalance = initialBalance;
            OpeningDate = initialDate;
        }

        public EditPersonalAccountVmDto(int? id, string name, double initialBalance, DateTime initialDate)
            : this(name, initialBalance, initialDate)
        {
            Id = id;
        }
    }
}
