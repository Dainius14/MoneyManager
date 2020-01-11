using System;

namespace MoneyManager.Core.Services.Communication
{
    public class Response<T> : BaseResponse where T : class
    {
        public T? Item { get; private set; }

        private Response(bool success, string message, T? item)
            : base(success, message)
        {
            Item = item;
        }

        public Response(T item)
            : this(true, string.Empty, item)
        { }

        public Response(string message)
            : this(false, message, null)
        { }
    }
}
