using FluentValidation;

namespace PoZiomkaDomain.Student.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(command => command.Password)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Password cannot be empty or longer than 100 characters");
    }
}
