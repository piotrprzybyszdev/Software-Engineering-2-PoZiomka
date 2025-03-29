namespace PoZiomkaInfrastructure.Exceptions;

public class QueryExecutionException(string message, int code) : InfrastructureException(message)
{
    public int Code { get; } = code;
}
