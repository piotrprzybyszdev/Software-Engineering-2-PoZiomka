using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using PoZiomkaInfrastructure;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using PoZiomkaApi.Requests.Auth;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http.Json;

namespace PoZiomkaIntegrationTest;

public class StudentControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
	private readonly HttpClient _client;

	public StudentControllerTest(WebApplicationFactory<Program> factory)
	{

		var options = new WebApplicationFactoryClientOptions
		{
			AllowAutoRedirect = true, // Follow redirects if necessary
			HandleCookies = true, // Enable cookies for authentication persistence
		};

		_client = factory
		   .WithWebHostBuilder(builder =>
		   {
			   builder.UseEnvironment("IntegrationTest"); // Set environment to "Testing" for appsettings.IntegrationTest.json
		   })
		   .CreateClient(options);
	}
	[Fact]
	public async Task LoginAndGetLoggedInUser_ShouldReturnSuccess()
	{
		// 1. Define login request payload
		var loginRequest = new
		{
			Email = "student@example.com",
			Password = "pass"
		};

		// 2. Send login request
		var loginResponse = await _client.PostAsJsonAsync("api/login", loginRequest);
		Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

		// 3. Extract Set-Cookie header
		string? cookieHeader = loginResponse.Headers.Contains("Set-Cookie")
			? loginResponse.Headers.GetValues("Set-Cookie").FirstOrDefault()
			: null;

		Assert.NotNull(cookieHeader); // Ensure the cookie is set

		Console.WriteLine($"Extracted Cookie: {cookieHeader}");

		// 4. Attach cookie to next request
		var getRequest = new HttpRequestMessage(HttpMethod.Get, "api/student/get-logged-in");
		getRequest.Headers.Add("Cookie", cookieHeader); // Manually adding cookie header

		var getLoggedInResponse = await _client.SendAsync(getRequest);
		Assert.Equal(HttpStatusCode.OK, getLoggedInResponse.StatusCode);

		// 5. Verify user details are returned
		var userData = await getLoggedInResponse.Content.ReadFromJsonAsync<StudentDto>();
		Assert.NotNull(userData);
		Assert.Equal("teststudent", userData.Username);
	}

}

// DTO class for expected user response
public class StudentDto
{
	public int Id { get; set; }
	public string Username { get; set; }
}