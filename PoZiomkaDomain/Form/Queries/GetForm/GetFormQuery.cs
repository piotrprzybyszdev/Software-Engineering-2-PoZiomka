using MediatR;
using PoZiomkaDomain.Form.Dtos;

namespace PoZiomkaDomain.Form.Commands.GetForm;

public record GetFormQuery(int Id) : IRequest<FormDisplay>;
