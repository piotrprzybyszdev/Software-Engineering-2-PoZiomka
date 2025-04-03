using System.Text;
using System.Text.Json;
using PoZiomkaApi.Requests.Auth;
using System.Net;
using System.Net.Http.Json;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaIntegrationTest;

public class StudentControllerTest(MockWebApplicationFactory<Program> _factory) : IClassFixture<MockWebApplicationFactory<Program>>
{
	private readonly HttpClient _client = _factory.CreateClient();

	private readonly string _userEmail = "student@example.com";
	private readonly string _userPassword = "asdf";
	private readonly string _adminEmail = "admin@example.com";
	private readonly string _adminPassword = "asdf";

	[Fact]
	public async Task LoginAndGetLoggedInUser_ShouldReturnSuccess()
	{
		string cookie = await _client.LoginAsUser(_userEmail, _userPassword);

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
		string cookie = await _client.LoginAsUser(_userEmail, _userPassword);

		// getting Id
		var getRequest = new HttpRequestMessage(HttpMethod.Get, "api/student/get-logged-in");
		var getLoggedInResponse = await _client.SendAsyncWithCookie(getRequest, cookie);
		var userData = await getLoggedInResponse.Content.ReadFromJsonAsync<StudentDisplay>();
		int myId = userData.Id;

		// setting request for updating
		string newLastName = userData.LastName == "Big" ? "Small" : "Big";
		var editInfo = new StudentUpdate(
			Id: myId,
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
		string cookie = await _client.LoginAsUser(_userEmail, _userPassword);

		// getting Id
		var getRequest = new HttpRequestMessage(HttpMethod.Get, "api/student/get-logged-in");
		var getLoggedInResponse = await _client.SendAsyncWithCookie(getRequest, cookie);
		var userData = await getLoggedInResponse.Content.ReadFromJsonAsync<StudentDisplay>();
		int myId = userData.Id;
		string lastName = userData.LastName;

		// setting request for updating
		string newLastName = userData.LastName == "Big" ? "Small" : "Big";
		var editInfo = new StudentUpdate(
			Id: myId + 1,
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

	[Fact]
	public async Task GetAllStudentsByAdmin()
	{
		string cookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);

		var getRequest = new HttpRequestMessage(HttpMethod.Get, "api/student/get");
		var response = await _client.SendAsyncWithCookie(getRequest, cookie);

		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		var data = await response.Content.ReadFromJsonAsync<List<StudentDisplay>>();
		Assert.NotEmpty(data);
		Assert.True(data.Count > 0);
	}

	[Fact]
	public async Task CreateStudentByAdmin()
	{
		string cookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);

		var getRequest = new HttpRequestMessage(HttpMethod.Get, "api/student/get");
		var response = await _client.SendAsyncWithCookie(getRequest, cookie);
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		var data = await response.Content.ReadFromJsonAsync<List<StudentDisplay>>();
		Assert.NotEmpty(data);
		int countBefore = data.Count;

		Random random = new Random();
		int randomValue=random.Next(1,int.MaxValue);
		var signUp = new SignupRequest(
			Email: "newEmail@test"+randomValue.ToString()+".pl",
			Password: "password"
			);

		var json = JsonSerializer.Serialize(signUp);
		var content = new StringContent(json, Encoding.UTF8, "application/json");

		var postRequest = new HttpRequestMessage(HttpMethod.Post, "api/student/create")
		{
			Content = content
		};
		var response2 = await _client.SendAsyncWithCookie(postRequest, cookie);

		Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

		var getRequest3 = new HttpRequestMessage(HttpMethod.Get, "api/student/get");
		var response3 = await _client.SendAsyncWithCookie(getRequest3, cookie);
		Assert.Equal(HttpStatusCode.OK, response3.StatusCode);
		var data3 = await response3.Content.ReadFromJsonAsync<List<StudentDisplay>>();
		Assert.NotEmpty(data3);
		int countAfter = data3.Count;

		Assert.Equal(countBefore + 1, countAfter);
	}

	[Fact]
	public async Task DeleteLastAddedStudentByAdmin()
	{
		string cookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);

		// create student
		Random random = new Random();
		int randomValue = random.Next(1, int.MaxValue);
		string created_email = "newEmail@test" + randomValue.ToString() + ".pl";
		var signUp = new SignupRequest(
			Email: created_email,
			Password: "password"
			);

		var json = JsonSerializer.Serialize(signUp);
		var content = new StringContent(json, Encoding.UTF8, "application/json");

		var postRequest = new HttpRequestMessage(HttpMethod.Post, "api/student/create")
		{
			Content = content
		};
		var response0 = await _client.SendAsyncWithCookie(postRequest, cookie);

		Assert.Equal(HttpStatusCode.OK, response0.StatusCode);

		// get all students
		var getRequest = new HttpRequestMessage(HttpMethod.Get, "api/student/get");
		var response = await _client.SendAsyncWithCookie(getRequest, cookie);
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		var data = await response.Content.ReadFromJsonAsync<List<StudentDisplay>>();
		Assert.NotEmpty(data);
		int countBefore = data.Count;

		// choose created student id
		var student = data.FirstOrDefault(student => student.Email== created_email);
		Assert.NotNull(student);

		// delete student
		var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, "api/student/delete/" + student.Id);
		var response2 = await _client.SendAsyncWithCookie(deleteRequest, cookie);
		Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

		// get all students
		var getRequest3 = new HttpRequestMessage(HttpMethod.Get, "api/student/get");
		var response3 = await _client.SendAsyncWithCookie(getRequest3, cookie);
		Assert.Equal(HttpStatusCode.OK, response3.StatusCode);
		var data3 = await response3.Content.ReadFromJsonAsync<List<StudentDisplay>>();
		Assert.NotEmpty(data3);
		int countAfter = data3.Count;

		// check if count is lower by 1
		Assert.Equal(countBefore - 1, countAfter);
	}

	[Fact]
	public async Task DeleteRandomStudentByAdmin()
	{
		string cookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);

		// get all students
		var getRequest = new HttpRequestMessage(HttpMethod.Get, "api/student/get");
		var response = await _client.SendAsyncWithCookie(getRequest, cookie);
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		var data = await response.Content.ReadFromJsonAsync<List<StudentDisplay>>();
		Assert.NotEmpty(data);
		int countBefore = data.Count;

		// choose student id
		Random r = new Random();
		int n=r.Next(0, data.Count);
		var student = data[n];
		Assert.NotNull(student);

		// delete student
		var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, "api/student/delete/" + student.Id);
		var response2 = await _client.SendAsyncWithCookie(deleteRequest, cookie);
		Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

		// get all students
		var getRequest3 = new HttpRequestMessage(HttpMethod.Get, "api/student/get");
		var response3 = await _client.SendAsyncWithCookie(getRequest3, cookie);
		Assert.Equal(HttpStatusCode.OK, response3.StatusCode);
		var data3 = await response3.Content.ReadFromJsonAsync<List<StudentDisplay>>();
		Assert.NotEmpty(data3);
		int countAfter = data3.Count;

		// check if count is lower by 1
		Assert.Equal(countBefore - 1, countAfter);
	}
}
