using MoneyManager.Models.Domain;
using MoneyManager.Models.DTO;

namespace MoneyManager.Models.ViewModels
{
    public class AuthenticatedUserVm
    {
        public User User { get; private set; }

        public string AuthToken { get; private set; }

        public string RefreshToken { get; private set; }

        public AuthenticatedUserVm(User user, string authToken, string refreshToken)
        {
            User = user;
            AuthToken = authToken;
            RefreshToken = refreshToken;
        }
    }

    public class AuthenticatedUserVmDto
    {
        public GetUserDto User { get; set; } = null!;

        public string AuthToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;
        
        public AuthenticatedUserVmDto()
        {
        }

        public AuthenticatedUserVmDto(GetUserDto user, string authToken, string refreshToken)
        {
            User = user;
            AuthToken = authToken;
            RefreshToken = refreshToken;
        }
    }
}
