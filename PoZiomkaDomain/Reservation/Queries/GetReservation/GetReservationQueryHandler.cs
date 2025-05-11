using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Reservation.Dtos;
using PoZiomkaDomain.Reservation.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Exceptions;

namespace PoZiomkaDomain.Reservation.Queries.GetReservation;

public class GetReservationQueryHandler(IReservationRepository reservationRepository, IStudentRepository studentRepository) : IRequestHandler<GetReservationQuery, ReservationDisplay>
{
    public async Task<ReservationDisplay> Handle(GetReservationQuery request, CancellationToken cancellationToken)
    {
        int loggedInUserId = request.User.GetUserId() ?? throw new DomainException("Id of the user isn't known");

        bool isUserAuthorized = false;

        if (request.User.IsInRole(Roles.Administrator)) isUserAuthorized = true;
        else if (request.User.IsInRole(Roles.Student))
        {
            var student = await studentRepository.GetStudentById(loggedInUserId, cancellationToken);
            if (student is null)
                throw new StudentNotFoundException($"Student with id `{loggedInUserId}` not found");
            if (student.ReservationId == request.Id)
                isUserAuthorized = true;
        }

        if (!isUserAuthorized)
            throw new UnauthorizedException("User must be logged in as an administrator or a student that is assigned to the reservation");

        try
        {
            return await reservationRepository.GetReservationDisplay(request.Id, cancellationToken);
        }
        catch (IdNotFoundException e)
        {
            throw new ReservationNotFoundException("Reservation not found", e);
        }
    }
}
