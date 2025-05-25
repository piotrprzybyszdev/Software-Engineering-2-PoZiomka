using PoZiomkaDomain.Reservation.Dtos;

namespace PoZiomkaDomain.Reservation;

public interface IReservationRepository
{
    public Task<IEnumerable<ReservationModel>> GetAllReservations(CancellationToken? cancellationToken);
    public Task<ReservationDisplay> GetReservationDisplay(int id, CancellationToken? cancellationToken);
    public Task UpdateReservation(int id, bool isAcceptation, CancellationToken? cancellationToken);
    public Task UpdateStudentReservation(int id, int studentId, bool isAcceptation, CancellationToken? cancellationToken);
}
