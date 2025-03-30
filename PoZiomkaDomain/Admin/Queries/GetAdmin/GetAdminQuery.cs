using MediatR;
using PoZiomkaDomain.Admin.Dtos;
using System.Security.Claims;

namespace PoZiomkaDomain.Admin.Queries.GetAdmin;

public record GetAdminQuery(int Id, ClaimsPrincipal User) : IRequest<AdminDisplay>;
