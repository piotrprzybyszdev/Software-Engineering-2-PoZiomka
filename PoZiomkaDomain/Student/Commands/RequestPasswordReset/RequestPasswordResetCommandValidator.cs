using FluentValidation;

namespace PoZiomkaDomain.Student.Commands.RequestPasswordReset;

public class RequestPasswordResetCommandValidator : AbstractValidator<RequestPasswordResetCommand>
{
    public RequestPasswordResetCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100)
            .WithMessage("Student email cannot be empty or longer than 100 characters");
    }
}
