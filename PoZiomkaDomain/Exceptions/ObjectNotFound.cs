using System.Runtime.Serialization;

namespace PoZiomkaDomain.Exceptions;
public class ObjectNotFound : Exception
{
	public ObjectNotFound(string exceptionMessage) : base(exceptionMessage)
	{
	}
}

