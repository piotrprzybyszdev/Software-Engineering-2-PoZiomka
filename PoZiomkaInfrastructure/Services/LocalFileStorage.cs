using PoZiomkaDomain.Application;
using PoZiomkaInfrastructure.Common;

namespace PoZiomkaInfrastructure.Services;

public class LocalFileStorage(int maxSize, string rootDirectory, string applicationsDirectory) : IFileStorage
{
    public async Task<IFile> GetFileByGuid(Guid guid)
    {
		string filePath = Path.Combine("storage", guid.ToString());

		if (!File.Exists(filePath))
		{
			throw new FileNotFoundException("File not found", filePath);
		}

		var memoryStream = new MemoryStream();
		using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
		{
			await fileStream.CopyToAsync(memoryStream);
		}
		memoryStream.Position = 0;

		return new NetworkFile(memoryStream);
	}

    public Task UploadFile(Guid guid, IFile file)
    {
        if(!Directory.Exists("storage"))
		{
			Directory.CreateDirectory("storage");
		}

		string filePath = Path.Combine("storage", guid.ToString());

		using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
		{
			file.Stream.CopyTo(fileStream);
		}

		return Task.CompletedTask;
	}
}
