using System.Runtime.Serialization;

namespace PoZiomkaDomain.Exceptions
{
    [Serializable]
    public class NoRowEditedException : Exception
    {
        public NoRowEditedException()
        {
        }

        public NoRowEditedException(string? message) : base(message)
        {
        }

        public NoRowEditedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoRowEditedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}