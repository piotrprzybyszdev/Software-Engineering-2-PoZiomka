using FluentValidation;

namespace PoZiomkaDomain.Student.Commands.ConfirmStudent;

public class ConfirmStudentCommandValidator : AbstractValidator<ConfirmStudentCommand>
{
    public ConfirmStudentCommandValidator()
    {
    }
}
