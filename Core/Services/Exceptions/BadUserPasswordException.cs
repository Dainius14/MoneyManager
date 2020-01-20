using System;

namespace MoneyManager.Core.Services.Exceptions
{
    public class BadUserPasswordException : Exception
    {
        public BadUserPasswordException(string msg)
            : base(msg)
        {
        }
    }
}
