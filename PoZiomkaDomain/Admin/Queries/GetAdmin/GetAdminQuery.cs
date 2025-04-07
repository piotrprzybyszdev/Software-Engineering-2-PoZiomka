using MediatR;
using PoZiomkaDomain.Admin.Dtos;
using System.Security.Claims;

namespace PoZiomkaDomain.Admin.Queries.GetAdmin;

public record GetAdminQuery(ClaimsPrincipal User) : IRequest<AdminDisplay>;
