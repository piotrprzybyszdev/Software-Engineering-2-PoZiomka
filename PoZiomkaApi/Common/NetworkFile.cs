using PoZiomkaDomain.Application;

namespace PoZiomkaApi.Common;

public class NetworkFile(IFormFile file) : IFile
{
    IFormFile formFile = file;
    public Stream Stream => formFile.OpenReadStream();
}
