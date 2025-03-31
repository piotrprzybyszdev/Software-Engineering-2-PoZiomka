using MediatR;

namespace PoZiomkaDomain.Student.Commands.CreateStudent;

public record CreateStudentCommand(string Email, string? FirstName, string? LastName, string? IndexNumber, string? PhoneNumber) : IRequest;
