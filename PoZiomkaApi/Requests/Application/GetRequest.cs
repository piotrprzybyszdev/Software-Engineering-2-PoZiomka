using PoZiomkaDomain.Application.Dtos;

namespace PoZiomkaApi.Requests.Application;

public record GetRequest(string? StudentEmail, string? StudentIndex, int? ApplicationTypeId, ApplicationStatus? ApplicationStatus);
