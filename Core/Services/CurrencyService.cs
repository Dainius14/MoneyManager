using MoneyManager.Core.Repositories;
using MoneyManager.Core.Services.Communication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Models.Domain;

namespace MoneyManager.Core.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IUnitOfWork _uow;

        public CurrencyService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<IEnumerable<Currency>> ListAsync()
        {
            return await _uow.CurrencyRepo.GetAllAsync();
        }
    }
}
