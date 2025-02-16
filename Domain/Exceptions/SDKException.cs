using Domain.SDK_Comercial;

namespace Core.Domain.Exceptions
{
    public class SDKException : Exception
    {
        public int ErrorCode;
        public SDKException(int errCode) : base(SDK.rError(errCode))
        {
            ErrorCode = errCode;
        }

        public SDKException(string message, int errCode) : base(message + SDK.rError(errCode))
        {
            ErrorCode = errCode;
        }

        public SDKException(string message) : base(message) { }
    }
}
