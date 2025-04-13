using PoZiomkaDomain.Application;

namespace PoZiomkaInfrastructure.Common;
public class NetworkFile(Stream stream) : IFile
{
    Stream stream = stream;
    public Stream Stream => stream;
}

