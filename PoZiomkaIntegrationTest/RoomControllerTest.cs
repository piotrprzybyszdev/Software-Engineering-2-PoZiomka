using PoZiomkaDomain.Room.Dtos;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace PoZiomkaIntegrationTest;

public class RoomControllerTest(MockWebApplicationFactory<Program> _factory) : IClassFixture<MockWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = _factory.CreateClient();

    private readonly string _adminEmail = "admin@example.com";
    private readonly string _adminPassword = "asdf";

    [Fact]
    public async Task CreateAndGetRoom_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);

        // Get current rooms
        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/room/get");
        var response = await _client.SendAsyncWithCookie(getRequest, cookie);
        response.EnsureSuccessStatusCode();
        var roomsBefore = await response.Content.ReadFromJsonAsync<IEnumerable<RoomDisplay>>();
        int countBefore = roomsBefore!.Count();

        // Create new room
        var room = new
        {
            Floor = 10,
            Number = 123,
            Capacity = 4
        };
        var content = new StringContent(JsonSerializer.Serialize(room), Encoding.UTF8, "application/json");
        var postRequest = new HttpRequestMessage(HttpMethod.Post, "/room/create")
        {
            Content = content
        };
        var postResponse = await _client.SendAsyncWithCookie(postRequest, cookie);
        postResponse.EnsureSuccessStatusCode();

        // Get rooms after creation
        var getRequestAfter = new HttpRequestMessage(HttpMethod.Get, "/room/get");
        var responseAfter = await _client.SendAsyncWithCookie(getRequestAfter, cookie);
        responseAfter.EnsureSuccessStatusCode();
        var roomsAfter = await responseAfter.Content.ReadFromJsonAsync<IEnumerable<RoomDisplay>>();
        Assert.Equal(countBefore + 1, roomsAfter!.Count());
    }

    [Fact]
    public async Task GetRoomById_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);

        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/room/get");
        var response = await _client.SendAsyncWithCookie(getRequest, cookie);
        response.EnsureSuccessStatusCode();
        var rooms = await response.Content.ReadFromJsonAsync<IEnumerable<RoomDisplay>>();
        int roomId = rooms!.First().Id;

        // Login as student
        string studentCookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);

        var getByIdRequest = new HttpRequestMessage(HttpMethod.Get, $"/room/get/{roomId}");
        var getByIdResponse = await _client.SendAsyncWithCookie(getByIdRequest, studentCookie);
        getByIdResponse.EnsureSuccessStatusCode();
        var roomDetails = await getByIdResponse.Content.ReadFromJsonAsync<RoomStudentDisplay>();
        Assert.NotNull(roomDetails);
        Assert.Equal(roomId, roomDetails.Id);
    }

    [Fact]
    public async Task AddAndRemoveStudent_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);

        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/room/get");
        var response = await _client.SendAsyncWithCookie(getRequest, cookie);
        response.EnsureSuccessStatusCode();
        var rooms = await response.Content.ReadFromJsonAsync<IEnumerable<RoomDisplay>>();
        int roomId = rooms!.First().Id;

        int numberOfStudentsBefore = rooms!.First().StudentCount;

        var addRequest = new
        {
            Id = roomId,
            StudentId = 1
        };
        var addContent = new StringContent(JsonSerializer.Serialize(addRequest), Encoding.UTF8, "application/json");
        var addHttpRequest = new HttpRequestMessage(HttpMethod.Put, "/room/add-student")
        {
            Content = addContent
        };
        var addResponse = await _client.SendAsyncWithCookie(addHttpRequest, cookie);
        addResponse.EnsureSuccessStatusCode();

        var getByIdRequest = new HttpRequestMessage(HttpMethod.Get, $"/room/get/{roomId}");
        var getByIdResponse = await _client.SendAsyncWithCookie(getByIdRequest, cookie);
        getByIdResponse.EnsureSuccessStatusCode();
        var roomDetails = await getByIdResponse.Content.ReadFromJsonAsync<RoomStudentDisplay>();
        Assert.NotNull(roomDetails);
        Assert.Equal(roomId, roomDetails.Id);
        Assert.Equal(numberOfStudentsBefore + 1, roomDetails.StudentCount);

        var removeRequest = new
        {
            Id = roomId,
            StudentId = 1
        };
        var removeContent = new StringContent(JsonSerializer.Serialize(removeRequest), Encoding.UTF8, "application/json");
        var removeHttpRequest = new HttpRequestMessage(HttpMethod.Put, "/room/remove-student")
        {
            Content = removeContent
        };
        var removeResponse = await _client.SendAsyncWithCookie(removeHttpRequest, cookie);
        removeResponse.EnsureSuccessStatusCode();

        var getByIdAfterRemoveRequest = new HttpRequestMessage(HttpMethod.Get, $"/room/get/{roomId}");
        var getByIdAfterRemoveResponse = await _client.SendAsyncWithCookie(getByIdAfterRemoveRequest, cookie);
        getByIdAfterRemoveResponse.EnsureSuccessStatusCode();
        var roomDetailsAfterRemove = await getByIdAfterRemoveResponse.Content.ReadFromJsonAsync<RoomStudentDisplay>();
        Assert.NotNull(roomDetailsAfterRemove);
        Assert.Equal(roomId, roomDetailsAfterRemove.Id);
        Assert.Equal(numberOfStudentsBefore, roomDetailsAfterRemove.StudentCount);
    }

    [Fact]
    public async Task DeleteRoom_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);

        // Create a room to delete
        var room = new
        {
            Floor = 20,
            Number = 223,
            Capacity = 24
        };
        var content = new StringContent(JsonSerializer.Serialize(room), Encoding.UTF8, "application/json");
        var createRequest = new HttpRequestMessage(HttpMethod.Post, "/room/create")
        {
            Content = content
        };
        var createResponse = await _client.SendAsyncWithCookie(createRequest, cookie);
        createResponse.EnsureSuccessStatusCode();

        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/room/get");
        var response = await _client.SendAsyncWithCookie(getRequest, cookie);
        response.EnsureSuccessStatusCode();
        var rooms = await response.Content.ReadFromJsonAsync<IEnumerable<RoomDisplay>>();
        int roomId = rooms!.Last().Id;

        int countBefore = rooms!.Count();

        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"/room/delete/{roomId}");
        var deleteResponse = await _client.SendAsyncWithCookie(deleteRequest, cookie);
        deleteResponse.EnsureSuccessStatusCode();

        var getAfterDeleteRequest = new HttpRequestMessage(HttpMethod.Get, "/room/get");
        var responseAfterDelete = await _client.SendAsyncWithCookie(getAfterDeleteRequest, cookie);
        responseAfterDelete.EnsureSuccessStatusCode();
        var roomsAfterDelete = await responseAfterDelete.Content.ReadFromJsonAsync<IEnumerable<RoomDisplay>>();
        Assert.NotNull(roomsAfterDelete);
        Assert.Equal(countBefore - 1, roomsAfterDelete.Count());
    }
}
