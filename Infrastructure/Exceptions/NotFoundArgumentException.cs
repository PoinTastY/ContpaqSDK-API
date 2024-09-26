namespace Infrastructure.Exceptions
{
    public class NotFoundArgumentException : Exception
    {
        public NotFoundArgumentException(string message) : base(message)
        {
        }
    }
}
