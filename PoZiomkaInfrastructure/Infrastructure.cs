using DbUp;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoZiomkaDomain.Admin;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Match;
using PoZiomkaDomain.Student;
using PoZiomkaInfrastructure.Exceptions;
using PoZiomkaInfrastructure.Repositories;
using PoZiomkaInfrastructure.Services;
using System.Data;
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
        var connectionString = configuration["DB:Connection-String"];

        services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IJwtService>(_ => new JwtService(configuration["Jwt:Key"]!, configuration["Jwt:Issuer"]!, configuration["Jwt:Audience"]!));

        services.AddScoped<IEmailContentGenerator>(provider => new EmailContentGenerator(
            configuration["App:Url"]!, configuration["App:ConfirmEmailPath"]!,
            configuration["App:PasswordResetPath"]!, provider.GetService<IJwtService>()!
        ));
        services.AddScoped<IEmailService>(provider => new EmailService(
            configuration["Email:Host"]!, int.Parse(configuration["Email:Port"]!),
            configuration["Email:Sender"]!, configuration["Email:Password"]!,
            bool.Parse(configuration["Email:Secure"]!), provider.GetService<IEmailContentGenerator>()!)
        );

        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IJudgeService, JudgeService>();
    }
}
