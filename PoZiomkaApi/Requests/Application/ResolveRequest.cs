using PoZiomkaDomain.Application.Dtos;

namespace PoZiomkaApi.Requests.Application;

public record ResolveRequest(int Id, ApplicationStatus Status);
