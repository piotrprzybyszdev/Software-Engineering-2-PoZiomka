using MediatR;

namespace PoZiomkaDomain.Student.Commands.ConfirmStudent;

public record ConfirmStudentCommand(string Email) : IRequest;
