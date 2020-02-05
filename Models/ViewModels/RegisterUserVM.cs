using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Models.ViewModels
{
    public class RegisterUserVm
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }

    public class RegisterUserVmDto
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
