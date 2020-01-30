using MoneyManager.Models.Domain;
using MoneyManager.Models.DTO;

namespace MoneyManager.Models.ViewModels
{
    public class AuthenticatedUserVm
    {
        public User User { get; private set; }

        public string AccessToken { get; private set; }

        public string RefreshToken { get; private set; }

        public AuthenticatedUserVm(User user, string accessToken, string refreshToken)
        {
            User = user;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }

    public class AuthenticatedUserVmDto
    {
        public GetUserDto User { get; set; } = null!;

        public string AccessToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;
        
        public AuthenticatedUserVmDto()
        {
        }

        public AuthenticatedUserVmDto(GetUserDto user, string accessToken, string refreshToken)
        {
            User = user;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
