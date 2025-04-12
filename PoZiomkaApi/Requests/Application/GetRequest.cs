using PoZiomkaDomain.Application.Dtos;
using PoZiomkaDomain.Application.Queries.GetApplicationsWithFillter;

namespace PoZiomkaApi.Requests.Application;

public record GetRequest(string? StudentEmail, string? StudentIndex, int? ApplicationTypeId, ApplicationStatus? ApplicationStatus)
{
	public GetApplicationsWithFillterQuery ToQuery()
	{
		return new GetApplicationsWithFillterQuery(StudentEmail, StudentIndex, ApplicationTypeId, ApplicationStatus);
	}
}
