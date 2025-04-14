using Azure.Storage.Blobs;
using PoZiomkaDomain.Application;
using PoZiomkaInfrastructure.Common;

namespace PoZiomkaInfrastructure.Services;

public class AzureFileStorage : IFileStorage
{
    private readonly BlobContainerClient _containerClient;
    private readonly int _maxSize;

    public AzureFileStorage(int maxSize, string connectionString, string containerName)
    {
        _maxSize = maxSize;
        _containerClient = new BlobContainerClient(connectionString, containerName);
        _containerClient.CreateIfNotExists();
    }

    public async Task UploadFile(Guid guid, IFile file)
    {
        if (file.Stream.Length > _maxSize)
        {
            throw new FileTooLargeException((int)file.Stream.Length, _maxSize);
        }

        var blobClient = _containerClient.GetBlobClient(guid.ToString());

        await blobClient.UploadAsync(file.Stream);
    }

    public async Task<IFile> GetFileByGuid(Guid guid)
    {
        var blobClient = _containerClient.GetBlobClient(guid.ToString());

        if (!await blobClient.ExistsAsync())
        {
            throw new PoZiomkaDomain.Application.FileNotFoundException(guid);
        }

        return new BlobFile(await blobClient.OpenReadAsync());
    }
}
