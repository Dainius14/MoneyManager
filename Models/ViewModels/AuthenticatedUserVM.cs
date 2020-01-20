using MoneyManager.Models.Domain;
using MoneyManager.Models.DTO;

namespace MoneyManager.Models.ViewModels
{
    public class AuthenticatedUserVm
    {
        public User User { get; private set; }

        public string Token { get; private set; }

        public AuthenticatedUserVm(User user, string token)
        {
            User = user;
            Token = token;
        }
    }

    public class AuthenticatedUserVmDto
    {
        public GetUserDto User { get; set; } = null!;

        public string Token { get; set; } = null!;
        
        public AuthenticatedUserVmDto()
        {
        }

        public AuthenticatedUserVmDto(GetUserDto user, string token)
        {
            User = user;
            Token = token;
        }
    }
}
