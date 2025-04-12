namespace PoZiomkaDomain.Application;

public interface IFile
{
    Stream Stream { get; }
}



public interface IFileStorage
{
    public Task UploadFile(Guid guid, IFile file);
    public Task<IFile> GetFileByGuid(Guid guid);
}
