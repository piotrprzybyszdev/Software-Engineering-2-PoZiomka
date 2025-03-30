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
		//Assert.NotNull(userData);
		Assert.Equal("student@example.com", userData.Email);
	}

	[Fact]
	public async Task UpdatingUserData()
	{
		string cookie = await _client.LoginAsUser("student@example.com", "password");

		// getting Id
		var getRequest = new HttpRequestMessage(HttpMethod.Get, "api/student/get-logged-in");
		var getLoggedInResponse = await _client.SendAsyncWithCookie(getRequest, cookie);
		var userData = await getLoggedInResponse.Content.ReadFromJsonAsync<StudentDisplay>();
		int myId = userData.Id;

		// setting request for updating
		string newLastName = userData.LastName == "Big" ? "Small" : "Big";
		var editInfo = new StudentEdit(
			Id: myId,
			Email: "student@example.com",
			FirstName: "John",
			LastName: newLastName,
			PhoneNumber: "123-456-7890",
			IndexNumber: "A12345",
			IsPhoneNumberHidden: true,
			IsIndexNumberHidden: false
		);

		var json = JsonSerializer.Serialize(editInfo);
		var content = new StringContent(json, Encoding.UTF8, "application/json");

		var postRequest = new HttpRequestMessage(HttpMethod.Put, "api/student/update")
		{
			Content = content
		};
		await _client.SendAsyncWithCookie(postRequest, cookie);

		// checking if data was updated
		var getRequest2 = new HttpRequestMessage(HttpMethod.Get, "api/student/get-logged-in");
		var getLoggedInResponse2 = await _client.SendAsyncWithCookie(getRequest2, cookie);
		var userData2 = await getLoggedInResponse2.Content.ReadFromJsonAsync<StudentDisplay>();

		Assert.Equal(HttpStatusCode.OK, getLoggedInResponse2.StatusCode);
		Assert.Equal(newLastName, userData2.LastName);
	}

	[Fact]
	public async Task UpdatingUserDataByAnotherUser()
	{
		string cookie = await _client.LoginAsUser("student@example.com", "password");

		// getting Id
		var getRequest = new HttpRequestMessage(HttpMethod.Get, "api/student/get-logged-in");
		var getLoggedInResponse = await _client.SendAsyncWithCookie(getRequest, cookie);
		var userData = await getLoggedInResponse.Content.ReadFromJsonAsync<StudentDisplay>();
		int myId = userData.Id;
		string lastName = userData.LastName;

		// setting request for updating
		string newLastName = userData.LastName == "Big" ? "Small" : "Big";
		var editInfo = new StudentEdit(
			Id: myId+1,
			Email: "student@example.com",
			FirstName: "John",
			LastName: newLastName,
			PhoneNumber: "123-456-7890",
			IndexNumber: "A12345",
			IsPhoneNumberHidden: true,
			IsIndexNumberHidden: false
		);

		var json = JsonSerializer.Serialize(editInfo);
		var content = new StringContent(json, Encoding.UTF8, "application/json");

		var postRequest = new HttpRequestMessage(HttpMethod.Put, "api/student/update")
		{
			Content = content
		};
		var response = await _client.SendAsyncWithCookie(postRequest, cookie);

		// checking if user was authorized
		Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
	}
}
