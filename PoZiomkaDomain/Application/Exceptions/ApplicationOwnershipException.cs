namespace PoZiomkaDomain.Application.Exceptions
{
    [Serializable]
    internal class ApplicationOwnershipException : Exception
    {
        public ApplicationOwnershipException()
        {
        }

        public ApplicationOwnershipException(string? message) : base(message)
        {
        }

        public ApplicationOwnershipException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}