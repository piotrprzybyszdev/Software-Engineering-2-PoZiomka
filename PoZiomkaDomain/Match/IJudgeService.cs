using PoZiomkaDomain.Match.Dtos;
using PoZiomkaDomain.Reservation.Dtos;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaDomain.StudentAnswers.Dtos;

namespace PoZiomkaDomain.Match;

public interface IJudgeService
{
    public Task<IEnumerable<MatchModel>> FindMatching(IEnumerable<StudentModel> students, IEnumerable<StudentAnswerDisplay> studentAnswers);
    public Task<IEnumerable<ReservationModel>> GenerateReservations(List<MatchModel> matches);
}
