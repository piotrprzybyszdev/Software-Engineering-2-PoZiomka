using PoZiomkaDomain.Match;
using PoZiomkaDomain.Match.Dtos;
using PoZiomkaDomain.Reservation;
using PoZiomkaDomain.Reservation.Dtos;
using PoZiomkaDomain.Room;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaDomain.StudentAnswers.Dtos;

namespace PoZiomkaInfrastructure.Services;

public class JudgeService(IMatchRepository matchRepository, IReservationRepository reservationRepository, IRoomRepository roomRepository, IStudentRepository studentRepository) : IJudgeService
{
    public async Task<IEnumerable<MatchModel>> FindMatching(IEnumerable<StudentModel> students, IEnumerable<StudentAnswerDisplay> studentAnswers)
    {
        List<MatchModel> matches = new List<MatchModel>();
        for (int i = 0; i < students.Count(); i++)
        {
            matches.Add(await matchRepository.CreateMatch(students.ElementAt(i).Id, students.ElementAt(i).Id));
        }
        return matches;
    }

    public async Task<IEnumerable<ReservationModel>> GenerateReservations(IEnumerable<MatchModel> matches)
    {
        //update students and reservations

        int roomId = -1;
        List<int> studentIds = new List<int>();

        // create reservations
        ReservationModel reservationModel = await reservationRepository.CreateRoomReservation(roomId);

        // update students
        foreach (var studetnId in studentIds)
        {
            await studentRepository.UpdateReservation(studetnId, reservationModel.Id, null, null);
        }

        throw new NotImplementedException();
    }

    public async Task<bool> IsMatch(int studentId, int otherStudentId)
    {
        var studentMathces = await matchRepository.GetStudentMatches(studentId);
        return studentMathces.Any(m => m.StudentId2 == otherStudentId || m.StudentId1 == otherStudentId);
    }

}
