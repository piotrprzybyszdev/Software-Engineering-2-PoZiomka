using Moq;
using PoZiomkaDomain.Match.Dtos;
using PoZiomkaDomain.Reservation;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Room;
using PoZiomkaDomain.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoZiomkaInfrastructure.Services;
using PoZiomkaDomain.Match;
using PoZiomkaDomain.Reservation.Dtos;

namespace PoZiomkaUnitTest.Domain.Service;

public class GenerateReservationsTests
{
    [Fact]
    public async Task Test1()
    {
        // Arrange
        var mockRoomRepo = new Mock<IRoomRepository>();
        var mockReservationRepo = new Mock<IReservationRepository>();
        var mockStudentRepo = new Mock<IStudentRepository>();
        var matchRepo = new Mock<IMatchRepository>();

        var service = new JudgeService(matchRepo.Object, mockReservationRepo.Object, mockRoomRepo.Object, mockStudentRepo.Object);

        var emptyRooms = new List<RoomModel>
        {
            new RoomModel(1,1,1,2),
            new RoomModel (2, 2, 2, 3)
        };

        mockRoomRepo.Setup(r => r.GetEmptyRooms())
            .ReturnsAsync(emptyRooms);

        mockReservationRepo.Setup(r => r.CreateRoomReservation(It.IsAny<int>(), It.IsAny<CancellationToken?>()))
            .ReturnsAsync((int roomId, CancellationToken? _) => new ReservationModel(roomId+100, roomId, false));

        var matches = new List<MatchModel>
        {
            new MatchModel(1,1,2,0,0),
            new MatchModel(2,2,3,0,0)
        };

        // Act
        var result = await service.GenerateReservations(matches, null);

        // Assert
        Assert.Equal(2, result.Count());
    }
}
