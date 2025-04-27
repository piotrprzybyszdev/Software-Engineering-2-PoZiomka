using MediatR;
using PoZiomkaDomain.Form.Dtos;

namespace PoZiomkaDomain.Form.Commands.GetAllForms;

public record GetAllFormsQuery : IRequest<IEnumerable<FormModel>>;
