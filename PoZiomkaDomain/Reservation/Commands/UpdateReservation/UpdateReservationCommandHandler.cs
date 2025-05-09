using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Reservation.Exceptions;
using PoZiomkaDomain.Student.Exceptions;
using PoZiomkaDomain.Student;

namespace PoZiomkaDomain.Reservation.Commands.UpdateReservation;

public class UpdateReservationCommandHandler(IReservationRepository reservationRepository, IStudentRepository studentRepository) : IRequestHandler<UpdateReservationCommand>
{
    public async Task Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
    {
        int loggedInUserId = request.User.GetUserId() ?? throw new DomainException("Id of the user isn't known");

        bool isUserAuthorized = false;

        if (request.User.IsInRole(Roles.Administrator))
        {
            try
            {
                await reservationRepository.UpdateReservation(request.Id, request.IsAcceptation, cancellationToken);
                return;
            }
            catch (IdNotFoundException e)
            {
                throw new ReservationNotFoundException("Reservation not found", e);
            }
        }

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
            await reservationRepository.UpdateStudentReservation(request.Id, request.IsAcceptation, cancellationToken);
        }
        catch (IdNotFoundException e)
        {
            throw new ReservationNotFoundException("Reservation not found", e);
        }
    }
}
