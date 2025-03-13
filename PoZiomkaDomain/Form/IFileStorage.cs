using PoZiomkaDomain.Application.Dtos;

namespace PoZiomkaDomain.Form;

public interface IFileStorage
{
    public Task UploadFile(Guid guid, IFile file);
    public Task<IFile> GetFileByGuid(Guid guid);
}
