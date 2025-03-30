using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaIntegrationTest;
public static class HttpClientExtension
{
	public static async Task<HttpResponseMessage> SendAsyncWithCookie(this HttpClient client, HttpRequestMessage request, string cookie)
	{
		request.Headers.Add("Cookie", cookie);
		return await client.SendAsync(request);
	}

	public static async Task<string> LoginAsUser(this HttpClient client, string email, string password)
	{
		var loginRequest = new
		{
			Email = email,
			Password = password
		};

		var loginResponse = await client.PostAsJsonAsync("api/login", loginRequest);
		loginResponse.EnsureSuccessStatusCode();

		string? cookieHeader = loginResponse.Headers.Contains("Set-Cookie")
			? loginResponse.Headers.GetValues("Set-Cookie").FirstOrDefault()
			: null;

		if (cookieHeader == null)
		{
			throw new Exception("Cookie header not found");
		}

		return cookieHeader;
	}

	public static async Task<string> LoginAsAdmin(this HttpClient client, string email, string password)
	{
		throw new NotImplementedException();
	}
}

