using Azure.Storage.Blobs;
using PoZiomkaDomain.Application;

namespace PoZiomkaInfrastructure.Services;

public class AzureFileStorage(int maxSize, string connectionString, string containerName) : IFileStorage
{
    private readonly BlobServiceClient client = new(connectionString);

    public Task<IFile> GetFileByGuid(Guid guid)
    {
        throw new NotImplementedException();
    }

    public Task UploadFile(Guid guid, IFile file)
    {
        throw new NotImplementedException();
    }
}
