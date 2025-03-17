using FluentValidation;

namespace PoZiomkaDomain.Student.Commands.LoginStudent;

public class LoginStudentCommandValidator : AbstractValidator<LoginStudentCommand>
{
    public LoginStudentCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100)
            .WithMessage("Student email cannot be empty or longer than 100 characters");

        RuleFor(command => command.Password)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Password cannot be empty or longer than 100 characters");
    }
}
