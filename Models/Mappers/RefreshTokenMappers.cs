using MoneyManager.Models.ViewModels;

namespace MoneyManager.Models.Mappers
{
    public static class RefreshTokenMappers
    {
        public static RefreshTokenVmDto ToDto(this RefreshTokenVm vm)
        {
            return new RefreshTokenVmDto(vm.AccessToken, vm.RefreshToken);
        }

        public static RefreshTokenVm ToViewModel(this RefreshTokenVmDto dto)
        {
            return new RefreshTokenVm(dto.AccessToken, dto.RefreshToken);
        }
    }
}
