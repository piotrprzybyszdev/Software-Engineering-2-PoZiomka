using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using PoZiomkaInfrastructure;
using System.Runtime.CompilerServices;

namespace PoZiomkaIntegrationTest;

public class StudentControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
	private readonly HttpClient _client;

	public StudentControllerTest(WebApplicationFactory<Program> factory)
	{
		_client = factory
		   .WithWebHostBuilder(builder =>
		   {
			   builder.UseEnvironment("IntegrationTest"); // Set environment to "Testing" for appsettings.IntegrationTest.json
		   })
		   .CreateClient();
	}
	[Fact]
	public async Task Test1()
	{
		var response = await _client.GetAsync("api/student/get");

		response.EnsureSuccessStatusCode(); // Ensures 2xx status code
		var content = await response.Content.ReadAsStringAsync();

		Assert.NotEmpty(content);


		Assert.True(true);
	}


}

