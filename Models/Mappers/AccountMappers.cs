using MoneyManager.Models.Domain;
using MoneyManager.Models.DTO;
using MoneyManager.Models.ViewModels;
using System;

namespace MoneyManager.Models.Mappers
{
    public static class AccountMappers
    {
        public static Account ToDomainModel(this EditPersonalAccountVmDto dto)
        {
            return new Account
            {
                Id = dto.Id,
                Name = dto.Name,
                IsPersonal = true,
            };
        }

        public static Account ToDomainModel(this GetAccountDTO dto)
        {
            return new Account
            {
                Id = dto.Id,
                Name = dto.Name,
                IsPersonal = dto.IsPersonal,
            };
        }

        public static GetAccountDTO ToGetAccountDTO(this Account domain)
        {
            return new GetAccountDTO
            {
                Id = (int)domain.Id!,
                Name = domain.Name,
                IsPersonal = domain.IsPersonal,
                CreatedAt = domain.CreatedAt.ToISOString(),
                UpdatedAt = domain.UpdatedAt?.ToISOString(),
            };
        }
        public static GetAccountDTO ToGetAccountDTO(this AccountVm vm)
        {
            return new GetAccountDTO
            {
                Id = (int)vm.Id!,
                Name = vm.Name,
                IsPersonal = vm.IsPersonal,
                CreatedAt = vm.CreatedAt.ToISOString(),
                UpdatedAt = vm.UpdatedAt?.ToISOString(),
                CurrentBalance = vm.CurrentBalance,
                OpeningBalance = vm.OpeningBalance,
                OpeningDate = vm.OpeningDate.ToISODateString(),
            };
        }

        public static EditPersonalAccountVmDto ToEditAccountDTO(this Account domain)
        {
            throw new NotImplementedException();
        }

    }
}
