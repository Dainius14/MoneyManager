using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Models.DTO
{
    public class ChangePasswordDto
    {
        [Required]
        public string? CurrentPassword { get; set; }
        
        [Required]
        public string? NewPassword { get; set; }
    }
}