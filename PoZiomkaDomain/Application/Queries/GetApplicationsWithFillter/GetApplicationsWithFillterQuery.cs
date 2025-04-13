
using MediatR;
using PoZiomkaDomain.Application.Dtos;

namespace PoZiomkaDomain.Application.Queries.GetApplicationsWithFillter;
public record GetApplicationsWithFillterQuery(
    string? StudentEmail, string? StudentIndex,
    int? ApplicationTypeId, ApplicationStatus? ApplicationStatus)
    : IRequest<IEnumerable<ApplicationModel>>;