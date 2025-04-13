using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
            throw new InvalidOperationException($"File exceeds maximum allowed size of {_maxSize} bytes.");
        }

        var blobClient = _containerClient.GetBlobClient(guid.ToString());

        var blobHttpHeaders = new BlobHttpHeaders
        {
            ContentType = "application/pdf"
        };

        await blobClient.UploadAsync(file.Stream, new BlobUploadOptions
        {
            HttpHeaders = blobHttpHeaders
        });
    }

    public async Task<IFile> GetFileByGuid(Guid guid)
    {
        var blobClient = _containerClient.GetBlobClient(guid.ToString());

        if (!await blobClient.ExistsAsync())
        {
            throw new FileNotFoundException($"Blob {guid} not found.");
        }

        var download = await blobClient.DownloadAsync();

        return new NetworkFile(download.Value.Content);
    }
}
