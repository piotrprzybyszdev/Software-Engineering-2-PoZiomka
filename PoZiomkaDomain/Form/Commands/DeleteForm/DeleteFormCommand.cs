using MediatR;

namespace PoZiomkaDomain.Form.Commands.DeleteForm;

public record DeleteFormCommand(int Id) : IRequest;
