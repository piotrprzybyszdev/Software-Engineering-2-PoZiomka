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
        RuleFor(command => command.PhoneNumber).Length(9).Must(p => uint.TryParse(p, out _)).Unless(command => command.PhoneNumber is null).WithMessage("Student phone number must be 9 digits");
        RuleFor(command => command.IndexNumber).Length(6).Must(p => uint.TryParse(p, out _)).Unless(command => command.IndexNumber is null).WithMessage("Student index number must be 6 digits");
    }
}
