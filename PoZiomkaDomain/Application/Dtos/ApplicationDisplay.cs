namespace PoZiomkaDomain.Application.Dtos;

public record ApplicationDisplay(int Id, int StudentId, ApplicationTypeModel ApplicationType, ApplicationStatus ApplicationStatus)
{
    public ApplicationDisplay(int id, int studentId, int applicationTypeId, string applicationTypeName, string applicationTypeDescription, ApplicationStatus applicationStatus)
        : this(id, studentId, new ApplicationTypeModel(applicationTypeId, applicationTypeName, applicationTypeDescription), applicationStatus)
    {
    }
}
