using Domain.SDK_Comercial;

namespace Domain.Exceptions
{
    public class SDKException : Exception
    {
        public int ErrorCode;
        public string? MsgString;
        public SDKException(int errCode) : base(SDK.rError(errCode))
        {
            ErrorCode = errCode;
        }

        public SDKException(string msgString, int errCode) : base(msgString + SDK.rError(errCode) + $" (code: {errCode})")
        {
            ErrorCode = errCode;
            MsgString = msgString;
        }

        public SDKException(string message) : base(message) { }
    }
}
