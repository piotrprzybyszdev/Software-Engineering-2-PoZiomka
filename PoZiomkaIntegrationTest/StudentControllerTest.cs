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
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaIntegrationTest;

public class StudentControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
	private readonly HttpClient _client;

	public StudentControllerTest(WebApplicationFactory<Program> factory)
	{

		var options = new WebApplicationFactoryClientOptions
		{
			AllowAutoRedirect = true, // Follow redirects if necessary
			HandleCookies = false, // Enable cookies for authentication persistence
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
		string cookie = await _client.LoginAsUser("student@example.com", "password");

		var getRequest = new HttpRequestMessage(HttpMethod.Get, "api/student/get-logged-in");
		var getLoggedInResponse = await _client.SendAsyncWithCookie(getRequest, cookie);


		Assert.Equal(HttpStatusCode.OK, getLoggedInResponse.StatusCode);

		// 5. Verify user details are returned
		var userData = await getLoggedInResponse.Content.ReadFromJsonAsync<StudentDisplay>();
		Assert.NotNull(userData);
		//Assert.Equal("teststudent", userData.Username);
	}

}
