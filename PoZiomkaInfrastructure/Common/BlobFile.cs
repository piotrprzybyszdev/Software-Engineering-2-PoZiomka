using PoZiomkaDomain.Application;

namespace PoZiomkaInfrastructure.Common;

public class BlobFile(Stream stream) : IFile, IDisposable
{
    public Stream Stream => stream;

    public void Dispose()
    {
        stream.Dispose();
        GC.SuppressFinalize(this);
    }
}
