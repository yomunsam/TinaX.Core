using System;

namespace TinaX.Exceptions
{
    public class NotFoundException : XException
    {
        public NotFoundException() { }

        public NotFoundException(string msg) : base(msg) { }

        public NotFoundException(int errorCode) : base(errorCode) { }

        public NotFoundException(string msg, int errorCode) : base(msg, errorCode) { }

        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }

        public NotFoundException(string msg, string notFoundItemName) : base(msg) { NotFoundItemName = notFoundItemName; }

        public NotFoundException(string msg, int notFoundItemCode, string notFoundItemName) : base(msg)
        {
            NotFoundItemCode = notFoundItemCode;
            NotFoundItemName = notFoundItemName;
        }

        public int NotFoundItemCode { get; set; } = 0;
        public string NotFoundItemName { get; set; } = string.Empty;
    }
}
