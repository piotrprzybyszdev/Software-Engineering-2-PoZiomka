using PoZiomkaDomain.Application;

namespace PoZiomkaApi.Common;

public class NetworkFile(IFormFile file) : IFile
{
    public Stream Stream => Stream.Null;
}
