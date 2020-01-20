using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Models.DTO
{
    public class AuthenticateDto
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public AuthenticateDto()
        {
        }

        public AuthenticateDto(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
