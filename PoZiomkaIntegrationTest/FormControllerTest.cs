using PoZiomkaDomain.Form.Dtos;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace PoZiomkaIntegrationTest;

public class FormControllerTest(MockWebApplicationFactory<Program> _factory) : IClassFixture<MockWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = _factory.CreateClient();

    private readonly string _userEmail = "student@example.com";
    private readonly string _userPassword = "asdf";
    private readonly string _adminEmail = "admin@example.com";
    private readonly string _adminPassword = "asdf";

    [Fact]
    public async Task CreateAndGetForm_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);

        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/api/form/get");
        var response = await _client.SendAsyncWithCookie(getRequest, cookie);
        response.EnsureSuccessStatusCode();
        var getResponseContent = await response.Content.ReadFromJsonAsync<IEnumerable<FormModel>>();
        Assert.NotNull(getResponseContent);
        Assert.NotEmpty(getResponseContent);
        int countBefore = getResponseContent.Count();

        var form = new
        {
            Title = "Test Form",
            ObligatoryPreferences = new[]
            {
                new
                {
                    Name = "Preference 1",
                    Options = new[] { "Option 1", "Option 2" }
                },
                new
                {
                    Name = "Preference 2",
                    Options = new[] { "Option 3", "Option 4" }
                }
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(form), Encoding.UTF8, "application/json");

        var postRequest = new HttpRequestMessage(HttpMethod.Post, "/api/form/create")
        {
            Content = content
        };
        var postResponse = await _client.SendAsyncWithCookie(postRequest, cookie);
        postResponse.EnsureSuccessStatusCode();

        var getRequestAfter = new HttpRequestMessage(HttpMethod.Get, "/api/form/get");
        var responseAfter = await _client.SendAsyncWithCookie(getRequestAfter, cookie);
        response.EnsureSuccessStatusCode();
        var getResponseAfterContent = await responseAfter.Content.ReadFromJsonAsync<IEnumerable<FormModel>>();
        Assert.NotNull(getResponseAfterContent);
        Assert.NotEmpty(getResponseAfterContent);
        Assert.Equal(getResponseAfterContent.Count(), countBefore + 1);
    }

    [Fact]
    public async Task GetFormContent_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);
        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/api/form/get");
        var response = await _client.SendAsyncWithCookie(getRequest, cookie);
        response.EnsureSuccessStatusCode();
        var getResponseContent = await response.Content.ReadFromJsonAsync<IEnumerable<FormModel>>();
        Assert.NotNull(getResponseContent);
        Assert.NotEmpty(getResponseContent);
        int id = getResponseContent.First().Id;
        var getRequestContent = new HttpRequestMessage(HttpMethod.Get, $"/api/form/get-content/{id}");
        var responseContent = await _client.SendAsyncWithCookie(getRequestContent, cookie);
        response.EnsureSuccessStatusCode();
        var getResponseContentAfter = await responseContent.Content.ReadFromJsonAsync<FormDisplay>();
        Assert.NotNull(getResponseContentAfter);
    }

    [Fact]
    public async Task UpdateForm_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);
        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/api/form/get");
        var response = await _client.SendAsyncWithCookie(getRequest, cookie);
        response.EnsureSuccessStatusCode();
        var getResponseContent = await response.Content.ReadFromJsonAsync<IEnumerable<FormModel>>();
        Assert.NotNull(getResponseContent);
        Assert.NotEmpty(getResponseContent);
        int id = getResponseContent.First().Id;
        var formUpdate = new
        {
            Id = id,
            Title = "Updated Form",
            ObligatoryPreferences = new[]
            {
                new
                {
                    Id = 1,
                    Name = "Updated Preference 1",
                    Options = new[] { "Updated Option 1", "Updated Option 2" }
                },
                new
                {
                    Id = 2,
                    Name = "Updated Preference 2",
                    Options = new[] { "Updated Option 3", "Updated Option 4" }
                }
            }
        };
        var contentUpdate = new StringContent(JsonSerializer.Serialize(formUpdate), Encoding.UTF8, "application/json");
        var putRequest = new HttpRequestMessage(HttpMethod.Put, "/api/form/update")
        {
            Content = contentUpdate
        };
        var putResponse = await _client.SendAsyncWithCookie(putRequest, cookie);
        putResponse.EnsureSuccessStatusCode();
        var getRequestAfterUpdate = new HttpRequestMessage(HttpMethod.Get, $"/api/form/get-content/{id}");
        var responseAfterUpdate = await _client.SendAsyncWithCookie(getRequestAfterUpdate, cookie);
        response.EnsureSuccessStatusCode();
        var getResponseAfterContentUpdate = await responseAfterUpdate.Content.ReadFromJsonAsync<FormDisplay>();
        Assert.NotNull(getResponseAfterContentUpdate);
    }

    [Fact]
    public async Task DeleteForm_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);
        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/api/form/get");
        var response = await _client.SendAsyncWithCookie(getRequest, cookie);
        response.EnsureSuccessStatusCode();
        var getResponseContent = await response.Content.ReadFromJsonAsync<IEnumerable<FormModel>>();
        Assert.NotNull(getResponseContent);
        Assert.NotEmpty(getResponseContent);
        int id = getResponseContent.First().Id;

        var formDelete = new { id };
        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"/api/form/delete/{id}")
        {
            Content = new StringContent(JsonSerializer.Serialize(formDelete), Encoding.UTF8, "application/json")
        };
        var deleteResponse = await _client.SendAsyncWithCookie(deleteRequest, cookie);
        deleteResponse.EnsureSuccessStatusCode();
        var getRequestAfterDelete = new HttpRequestMessage(HttpMethod.Get, "/api/form/get");
        var responseAfterDelete = await _client.SendAsyncWithCookie(getRequestAfterDelete, cookie);
        response.EnsureSuccessStatusCode();
        var getResponseAfterContentDelete = await responseAfterDelete.Content.ReadFromJsonAsync<IEnumerable<FormModel>>();
        Assert.NotNull(getResponseAfterContentDelete);
        Assert.Equal(getResponseAfterContentDelete.Count(), getResponseContent.Count() - 1);
        Assert.All(getResponseAfterContentDelete, form => Assert.NotEqual(form.Id, id));
    }
}
