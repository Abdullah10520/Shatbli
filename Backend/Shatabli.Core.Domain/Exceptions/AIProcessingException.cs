namespace Shatabli.Core.Domain.Exceptions
{
    public class AIProcessingException : Exception
    {
        public AIProcessingException(string message) : base(message)
        {
        }

        public AIProcessingException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}