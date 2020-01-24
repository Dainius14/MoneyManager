using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Models.ViewModels
{
    public class RefreshTokenVm
    {

        public string AuthToken { get; set; }

        public string RefreshToken { get; set; }

        public RefreshTokenVm(string authToken, string refreshToken)
        {
            AuthToken = authToken;
            RefreshToken = refreshToken;
        }
    }
    public class RefreshTokenVmDto
    {
        [Required]
        public string AuthToken { get; set; } = null!;
        
        [Required]
        public string RefreshToken { get; set; } = null!;


        public RefreshTokenVmDto()
        {
        }

        public RefreshTokenVmDto(string authToken, string refreshToken)
        {
            AuthToken = authToken;
            RefreshToken = refreshToken;
        }
    }
}
