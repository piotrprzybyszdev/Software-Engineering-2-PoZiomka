using PoZiomkaDomain.Reservation.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaDomain.Reservation;

public interface IReservationRepository
{
    Task<ReservationModel> CreateRoomReservation(int roomId);
}
