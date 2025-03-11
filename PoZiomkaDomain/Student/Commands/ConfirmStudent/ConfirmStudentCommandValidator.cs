using FluentValidation;

namespace PoZiomkaDomain.Student.Commands.ConfirmStudent;

public class ConfirmStudentCommandValidator : AbstractValidator<ConfirmStudentCommand>
{
    public ConfirmStudentCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100)
            .WithMessage("Student email cannot be empty or longer than 100 characters");
    }
}
