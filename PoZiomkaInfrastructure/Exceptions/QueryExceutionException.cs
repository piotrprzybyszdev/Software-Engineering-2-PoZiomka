namespace PoZiomkaInfrastructure.Exceptions;

public class QueryExceutionException(string message, int code) : InfrastructureException(message)
{
    public int Code { get; } = code;
}
