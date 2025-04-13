using PoZiomkaDomain.Application;

namespace PoZiomkaInfrastructure.Services;

public class LocalFileStorage(int maxSize, string rootDirectory, string applicationsDirectory) : IFileStorage
{
    public Task<IFile> GetFileByGuid(Guid guid)
    {
        throw new NotImplementedException();
    }

    public Task UploadFile(Guid guid, IFile file)
    {
        throw new NotImplementedException();
    }
}
