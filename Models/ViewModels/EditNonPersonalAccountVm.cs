using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Models.ViewModels
{
    public class EditNonPersonalAccountVm
    {
        public int? Id { get; set; } = null;

        [Required]
        [MinLength(5)]
        public string Name { get; set; } = string.Empty;

        public EditNonPersonalAccountVm()
        {
        }

        public EditNonPersonalAccountVm(string name)
        {
            Name = name;
        }
        public EditNonPersonalAccountVm(int? id, string name)
            : this(name)
        {
            Id = id;
        }
    }
    public class EditNonPersonalAccountVmDto
    {
        public int? Id { get; set; }

        public string Name { get; set; } = null!;

        public EditNonPersonalAccountVmDto()
        {
        }

        public EditNonPersonalAccountVmDto(string name)
        {
            Id = null;
            Name = name;
        }

        public EditNonPersonalAccountVmDto(int? id, string name)
            : this(name)
        {
            Id = id;
        }
    }
}
