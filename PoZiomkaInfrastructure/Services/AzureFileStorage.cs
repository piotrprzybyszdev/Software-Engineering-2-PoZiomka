using PoZiomkaDomain.Application;
using PoZiomkaDomain.Application.Dtos;

namespace PoZiomkaInfrastructure.Services;

public class AzureFileStorage(int maxSize, string connectionString, string containerName) : IFileStorage
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
