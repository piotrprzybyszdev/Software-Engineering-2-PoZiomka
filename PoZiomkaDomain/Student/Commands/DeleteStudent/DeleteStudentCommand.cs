
using MediatR;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student.Commands.EditStudent;

public record DeleteStudentCommand(int Id) : IRequest;