using MediatR;
using PoZiomkaApi.Utils;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Match;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student.Queries.GetStudent;

public class GetStudentQueryHandler(IStudentRepository studentRepository, IJudgeService judgeService) : IRequestHandler<GetStudentQuery, StudentDisplay>
{
	public async Task<StudentDisplay> Handle(GetStudentQuery request, CancellationToken cancellationToken)
	{
		int loggedInUser = request.User.GetUserId();
		bool isUserAuthorized = request.User.IsInRole(Roles.Administrator) ||
		  request.User.IsInRole(Roles.Student) &&
		  (loggedInUser == request.Id || await judgeService.IsMatch(loggedInUser, request.Id));

		if (!isUserAuthorized)
			throw new UnauthorizedException("User must be logged in as an administrator or a student that has a match with the student");

		var hidePersonalInfo = !(request.User.IsInRole(Roles.Administrator) || loggedInUser == request.Id);

		try
		{
			var student = await studentRepository.GetStudentById(request.Id, cancellationToken);
			return student.ToStudentDisplay(hidePersonalInfo);
		}
		catch
		{
			throw new StudentNotFoundException($"Student with id `{request.Id}` not found");
		}
	}
}

