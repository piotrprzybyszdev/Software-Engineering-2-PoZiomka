using MediatR;

namespace PoZiomkaDomain.Student.Commands.SignupStudent;

public record SignupStudentCommand(string Email, string Password) : IRequest;
