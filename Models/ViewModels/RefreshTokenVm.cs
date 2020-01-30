using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Models.ViewModels
{
    public class RefreshTokenVm
    {

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }


        public RefreshTokenVm(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
    public class RefreshTokenVmDto
    {
        [Required]
        public string AccessToken { get; set; } = null!;
        
        [Required]
        public string RefreshToken { get; set; } = null!;


        public RefreshTokenVmDto()
        {
        }

        public RefreshTokenVmDto(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
