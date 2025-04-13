namespace PoZiomkaInfrastructure.Exceptions;

public class FileTooLargeException(string message, int size, int maxSize) : InfrastructureException(message)
{
    public int Size { get; } = size;
    public int MaxSize { get; } = maxSize;
}
