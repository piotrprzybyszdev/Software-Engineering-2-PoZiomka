using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoZiomkaInfrastructure.Exceptions;
using System.Reflection;

namespace PoZiomkaInfrastructure;

public static class Infrastructure
{
    public static void Initalize(IConfiguration configuration)
    {
        var connectionString = configuration["DB:Connection-String"];

        EnsureDatabase.For.SqlDatabase(connectionString);

        var upgrader = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
            throw new InfrastructureException(result.Error.Message);
    }

    public static void Configure(IConfiguration configuration, IServiceCollection services)
    {

    }
}
