using PoZiomkaDomain.Reservation;
using PoZiomkaDomain.Reservation.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaInfrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    public Task<ReservationModel> CreateRoomReservation(int roomId)
    {
        throw new NotImplementedException();
    }
}
