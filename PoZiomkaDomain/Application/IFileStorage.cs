using PoZiomkaDomain.Application.Dtos;

namespace PoZiomkaDomain.Application;

public interface IFileStorage
{
    public Task UploadFile(Guid guid, IFile file);
    public Task<IFile> GetFileByGuid(Guid guid);
}
