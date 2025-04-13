using Moq;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Room.Queries.GetAllRooms;
using PoZiomkaDomain.Room;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaDomain.Student;

namespace PoZiomkaUnitTest.Domain.Room;
public class GetRoomQueryHandlerTest
{
    [Fact]
    public async Task AdminSeeHidenInfoStudents()
    {
        Assert.True(false); 
    }
    [Fact]
    public async Task UserNotSeeHidenInfoStudents()
    {
        Assert.True(false);
    }
    [Fact]
    public async Task StudentInRoomHaveAccess()
    {
        Assert.True(false);
    }
    [Fact]
    public async Task StudentNotInRoomDontHaveAccess()
    {
        Assert.True(false);
    }
}

