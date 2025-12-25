namespace Shatabli.Core.Domain.Exceptions
{
    public class InvalidFileTypeException : Exception
    {
        public InvalidFileTypeException(string allowedTypes) 
            : base($"Invalid file type. Allowed types: {allowedTypes}")
        {
        }

        public InvalidFileTypeException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}