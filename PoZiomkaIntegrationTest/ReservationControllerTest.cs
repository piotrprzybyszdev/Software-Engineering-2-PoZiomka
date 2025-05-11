using PoZiomkaApi.Requests.Reservation;
using PoZiomkaDomain.Reservation.Dtos;
using System.Net.Http.Json;

namespace PoZiomkaIntegrationTest;

public class ReservationControllerTest(MockWebApplicationFactory<Program> _factory) : IClassFixture<MockWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = _factory.CreateClient();

    private readonly string _userEmail = "student@example.com";
    private readonly string _userPassword = "asdf";
    private readonly string _adminEmail = "admin@example.com";
    private readonly string _adminPassword = "asdf";

    [Fact]
    public async Task GetReservations_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);

        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/api/reservation/get");
        var response = await _client.SendAsyncWithCookie(getRequest, cookie);
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetReservationById_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);

        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/api/reservation/get");
        var response = await _client.SendAsyncWithCookie(getRequest, cookie);
        response.EnsureSuccessStatusCode();

        var getResponseContent = await response.Content.ReadFromJsonAsync<IEnumerable<ReservationModel>>();
        Assert.NotNull(getResponseContent);
        Assert.NotEmpty(getResponseContent);

        var reservationId = getResponseContent.First().Id;
        var getRequestById = new HttpRequestMessage(HttpMethod.Get, $"/api/reservation/get/{reservationId}");

        var responseById = await _client.SendAsyncWithCookie(getRequestById, cookie);
        responseById.EnsureSuccessStatusCode();
    }
}
