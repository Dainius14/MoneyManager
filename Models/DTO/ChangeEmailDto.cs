using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Models.DTO
{
    public class ChangeEmailDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}