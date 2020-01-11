using MoneyManager.Models.ViewModels;
using System;

namespace MoneyManager.Models.Mappers
{
    public static class EditPersonalAccountVmMappers
    {
        public static EditPersonalAccountVmDto ToDto(this EditPersonalAccountVm vm)
        {
            return new EditPersonalAccountVmDto(vm.Id, vm.Name, vm.Currency!.ID, vm.InitialBalance, vm.InitialDate);
        }

        public static EditPersonalAccountVm ToViewModel(this EditPersonalAccountVmDto vm)
        {
            return new EditPersonalAccountVm(vm.Id, vm.Name, vm.CurrencyId, vm.InitialBalance, vm.InitialDate);
        }

        public static GetPersonalAccountVmDto ToDto(this GetPersonalAccountVm vm)
        {
            return new GetPersonalAccountVmDto(vm.Account.ToGetAccountDTO(), vm.CurrentBalance, vm.Currency.ToGetCurrencyDTO());
        }

        public static GetPersonalAccountVm ToViewModel(this GetPersonalAccountVmDto dto)
        {
            return new GetPersonalAccountVm(dto.Account.ToDomainModel(), dto.CurrentBalance, dto.Currency.ToDomainModel());
        }



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
            return new GetNonPersonalAccountVmDto(vm.Account.ToGetAccountDTO(), vm.CurrentBalance, vm.Currency.ToGetCurrencyDTO());
        }

        public static GetNonPersonalAccountVm ToViewModel(this GetNonPersonalAccountVmDto dto)
        {
            return new GetNonPersonalAccountVm(dto.Account.ToDomainModel(), dto.CurrentBalance, dto.Currency.ToDomainModel());
        }
    }
}
