using System;

namespace MoneyManager.Core.Services.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string msg)
            : base(msg)
        {
        }
    }
}
