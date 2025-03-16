namespace PoZiomkaDomain.Application.Dtos;

public enum ApplicationStatus
{
    Pending,
    Accepted,
    Rejected
}

public record ApplicationTypeModel(int Id, string Name, string Description);
public record ApplicationModel(int Id, int StudentId, int ApplicationTypeId, Guid FileGuid, ApplicationStatus Status);
