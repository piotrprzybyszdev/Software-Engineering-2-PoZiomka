using FluentValidation;

namespace PoZiomkaDomain.Student.Commands.UpdateStudent;

public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentCommandValidator()
    {
        RuleFor(command => command.FirstName).MaximumLength(100);
        RuleFor(command => command.LastName).MaximumLength(100);
        RuleFor(command => command.PhoneNumber).Length(9).Must(p => uint.TryParse(p, out _)).Unless(command => command.PhoneNumber is null).WithMessage("Student phone number must be 9 digits");
        RuleFor(command => command.IndexNumber).Length(6).Must(p => uint.TryParse(p, out _)).Unless(command => command.IndexNumber is null).WithMessage("Student index number must be 6 digits");
    }
}
