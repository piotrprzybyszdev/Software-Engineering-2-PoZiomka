using MediatR;
using PoZiomkaApi.Utils;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Match;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student.Commands.GetStudent;

public class GetStudentCommandHandler(IStudentRepository studentRepository, IJudgeService judgeService) : IRequestHandler<GetStudentCommand, StudentDisplay>
{
    public async Task<StudentDisplay> Handle(GetStudentCommand request, CancellationToken cancellationToken)
    {
        int loggedInUser = request.User.GetUserId();
        bool ok = request.User.IsInRole(Roles.Administrator) ||
          (request.User.IsInRole(Roles.Student) &&
          (loggedInUser == request.Id || await judgeService.IsMatch(loggedInUser, request.Id)));

        if (!ok)
            throw new UnauthorizedException("User must be logged in as an administrator or a student that has a match with the student");
       
        bool hide = true;
        if (request.User.IsInRole(Roles.Administrator))
            hide = false;

        try
        {
            var student = await studentRepository.GetStudentById(request.Id, cancellationToken);
            return student.ToStudentDisplay(hide);
        }
        catch
        {
            throw new ObjectNotFound($"Student with id `{request.Id}` not found");
        }
    }
}

