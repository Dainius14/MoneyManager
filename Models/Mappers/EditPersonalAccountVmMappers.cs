using MoneyManager.Models.ViewModels;
using System;

namespace MoneyManager.Models.Mappers
{
    public static class EditPersonalAccountVmMappers
    {


        public static EditNonPersonalAccountVm ToViewModel(this EditNonPersonalAccountVmDto dto)
        {
            return new EditNonPersonalAccountVm(dto.Id, dto.Name);
        }

        public static EditNonPersonalAccountVmDto ToDto(this EditNonPersonalAccountVm vm)
        {
            return new EditNonPersonalAccountVmDto(vm.Id, vm.Name);
        }

        public static GetNonPersonalAccountVmDto ToDto(this GetNonPersonalAccountVm vm)
        {
            return new GetNonPersonalAccountVmDto(vm.Account.ToGetAccountDTO(), vm.CurrentBalance);
        }

        public static GetNonPersonalAccountVm ToViewModel(this GetNonPersonalAccountVmDto dto)
        {
            return new GetNonPersonalAccountVm(dto.Account.ToDomainModel(), dto.CurrentBalance);
        }
    }
}
