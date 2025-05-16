using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace PoZiomkaIntegrationTest;

public class MockWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly WebApplicationFactoryClientOptions _ClientOptions = new()
    {
        AllowAutoRedirect = true,
        HandleCookies = false,
    };

    public new WebApplicationFactoryClientOptions ClientOptions => _ClientOptions;

    static readonly Dictionary<string, int> DbCount = [];
    static readonly Mutex DbCountMutex = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        string dbName = "PoziomkaTestDB";

        var frames = new StackTrace().GetFrames();
        foreach (var frame in frames)
        {
            var type = frame.GetMethod()?.DeclaringType;

            if (type != null && Array.Exists(type.GetInterfaces(), i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IClassFixture<>)))
            {
                dbName = $"Poziomka{type.Name}DB";
                break;
            }
        }

        DbCountMutex.WaitOne();
        DbCount.TryGetValue(dbName, out var count);
        DbCount[dbName] = count + 1;
        DbCountMutex.ReleaseMutex();

        base.ConfigureWebHost(builder);
        builder.UseEnvironment("IntegrationTest");

        var config = new ConfigurationBuilder().AddJsonFile("appsettings.IntegrationTest.json").Build();
        var connectionString = config["DB:ConnectionString"] ?? "Server=localhost,1433;Database=PoZiomkaTestDB;User ID=sa;Password=Pass@word;TrustServerCertificate=true;";

        builder.UseConfiguration(new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>()
            {
                ["DB:ConnectionString"] = connectionString.Replace("PoziomkaTestDB", dbName + count)
            }).Build()
        );
    }
}
