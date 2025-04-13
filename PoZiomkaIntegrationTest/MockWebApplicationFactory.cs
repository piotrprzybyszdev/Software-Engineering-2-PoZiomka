using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace PoZiomkaIntegrationTest;

public class MockWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly WebApplicationFactoryClientOptions _ClientOptions = new()
    {
        AllowAutoRedirect = true,
        HandleCookies = false,
    };

    public new WebApplicationFactoryClientOptions ClientOptions => _ClientOptions;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseEnvironment("IntegrationTest");
    }
}
