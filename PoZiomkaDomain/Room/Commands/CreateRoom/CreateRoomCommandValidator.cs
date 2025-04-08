using FluentValidation;
using PoZiomkaDomain.Student.Commands.CreateStudent;

namespace PoZiomkaDomain.Room.Commands.CreateRoom;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(command => command.Floor).GreaterThanOrEqualTo(0);
        RuleFor(command => command.Capacity).GreaterThan(0);
    }
}
