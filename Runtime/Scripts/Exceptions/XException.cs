using System;

namespace TinaX
{
    public class XException : Exception
    {
        public bool ServiceException { get; protected set; } = false;
        public string ServiceName { get; protected set; } = "TinaX.Core";
        public int ErrorCode { get; protected set; }
        public XException(string msg) : base(msg) { }
        public XException(string msg, int errorCode) : base(msg) { ErrorCode = errorCode; }
        public XException (int errorCode) { ErrorCode = errorCode; }
        public XException() { }

    }
}

