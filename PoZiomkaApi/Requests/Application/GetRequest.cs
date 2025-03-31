using PoZiomkaDomain.Application.Dtos;

namespace PoZiomkaApi.Requests.Application;

public record GetRequest(int? StudentId, string? StudentEmail, string? StudentIndex, ApplicationStatus? ApplicationStatus);
