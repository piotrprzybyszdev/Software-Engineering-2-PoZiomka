using PoZiomkaDomain.Match;
using PoZiomkaDomain.Match.Dtos;
using PoZiomkaDomain.Reservation;
using PoZiomkaDomain.Reservation.Dtos;
using PoZiomkaDomain.Room;
using PoZiomkaDomain.Room.Dtos;
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

    private class RoomHelper
    {
        public int RoomId { get; set; }
        public int PlacesLeft { get; set; }
        public int ReservationId { get; set; }
    }

    /*
     * adds only to empty rooms
     * 1) find empty rooms
     * 2) iterate over students and assign them to rooms
     */
    public async Task<IEnumerable<ReservationModel>> GenerateReservations(IEnumerable<MatchModel> matches, CancellationToken? cancellationToken)
    {
        List<ReservationModel> reservations = new List<ReservationModel>();



        List<RoomHelper> akademik = new List<RoomHelper>();
        List<RoomModel> emptyRooms = (await roomRepository.GetEmptyRooms()).ToList();
        foreach (var room in emptyRooms)
        {
            akademik.Add(new RoomHelper { RoomId = room.Id, PlacesLeft = room.Capacity, ReservationId = -1 });
        }

        // get uniq student ids from matches
        List<int> studentIds = matches.SelectMany(match => new[] { match.StudentId1, match.StudentId2 }).Distinct().ToList();

        // asign for each student room
        for (int i = 0; i < studentIds.Count; i++)
        {
            if (akademik.Count == 0)
                break;
            int akademikRoomIndex = 0;
            RoomHelper room = akademik[akademikRoomIndex];

            if (room.ReservationId == -1)
            {
                // create reservation

                ReservationModel reservationModel = await reservationRepository.CreateRoomReservation(room.RoomId, cancellationToken);

                reservations.Add(reservationModel);
                room.ReservationId = reservationModel.Id;
            }

            // update student's reservation
            await studentRepository.UpdateReservation(studentIds[i], room.ReservationId, null, null);

            room.PlacesLeft--;

            // remove room if no places left
            if (room.PlacesLeft == 0)
            {
                akademik.RemoveAt(akademikRoomIndex);
            }
        }

        return reservations;
    }

    public async Task<bool> IsMatch(int studentId, int otherStudentId)
    {
        var studentMathces = await matchRepository.GetStudentMatches(studentId);
        return studentMathces.Any(m => m.StudentId2 == otherStudentId || m.StudentId1 == otherStudentId);
    }
}
