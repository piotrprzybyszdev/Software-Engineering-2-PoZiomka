namespace PoZiomkaDomain.Application.Dtos;

public interface IFile
{

}

public record ApplicationDisplay(int Id, int StudentId, ApplicationTypeModel ApplicationType, IFile File, ApplicationStatus ApplicationStatus);
