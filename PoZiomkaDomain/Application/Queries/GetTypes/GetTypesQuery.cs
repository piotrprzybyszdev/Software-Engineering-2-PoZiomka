using MediatR;
using PoZiomkaDomain.Application.Dtos;

namespace PoZiomkaDomain.Application.Queries.GetTypes;
public record GetTypesQuery() : IRequest<IEnumerable<ApplicationTypeModel>>;
