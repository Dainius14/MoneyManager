using MoneyManager.Models.Domain;
using MoneyManager.Models.DTO;
using MoneyManager.Models.ViewModels;

namespace MoneyManager.Models.Mappers
{
    public static class UserMappers
    {
        public static EditUserDto ToEditUserDto(this User user)
        {
            return new EditUserDto(user.Id, user.Email);
        }

        public static User ToDomainModel(this EditUserDto dto)
        {
            return new User(dto.Id, dto.Email, dto.Password);
        }

        public static User ToDomainModel(this GetUserDto dto)
        {
            return new User(dto.Id, dto.Email);
        }

        public static GetUserDto ToGetUserDto(this User user)
        {
            return new GetUserDto((int)user.Id!, user.Email);
        }

        public static AuthenticatedUserVmDto ToDto(this AuthenticatedUserVm vm)
        {
            return new AuthenticatedUserVmDto(vm.User.ToGetUserDto(), vm.AccessToken, vm.RefreshToken);
        }

        public static AuthenticatedUserVm ToViewModel(this AuthenticatedUserVmDto dto)
        {
            return new AuthenticatedUserVm(dto.User.ToDomainModel(), dto.AccessToken, dto.RefreshToken);
        }
    }
}
