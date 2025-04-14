namespace PoZiomkaDomain.Application;

public interface IFile
{
    Stream Stream { get; }
}

public class FileTooLargeException(int size, int maxSize) : Exception
{
    public int Size { get; } = size;
    public int MaxSize { get; } = maxSize;
}

public class FileNotFoundException(Guid guid) : Exception
{
    public Guid Guid { get; } = guid;
}

public interface IFileStorage
{
    public Task UploadFile(Guid guid, IFile file);
    public Task<IFile> GetFileByGuid(Guid guid);
}
