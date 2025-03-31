using FluentValidation;

namespace PoZiomkaDomain.Student.Commands.CreateStudent;

public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100)
            .WithMessage("Student email cannot be empty or longer than 100 characters");

        RuleFor(command => command.FirstName).MaximumLength(100);
        RuleFor(command => command.LastName).MaximumLength(100);
    }
}
