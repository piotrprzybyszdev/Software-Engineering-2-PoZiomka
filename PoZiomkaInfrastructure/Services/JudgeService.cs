using PoZiomkaDomain.Match;
using PoZiomkaDomain.Match.Dtos;
using PoZiomkaDomain.Reservation.Dtos;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaDomain.StudentAnswers.Dtos;

namespace PoZiomkaInfrastructure.Services;

public class JudgeService() : IJudgeService
{
    public Task<IEnumerable<MatchModel>> FindMatching(IEnumerable<StudentModel> students, IEnumerable<StudentAnswerDisplay> studentAnswers)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ReservationModel>> GenerateReservations(IEnumerable<MatchModel> matches)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsMatch(int studentId, int otherStudentId)
    {
        return Task.FromResult(false);
    }

}
