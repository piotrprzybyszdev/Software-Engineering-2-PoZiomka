using DbUp;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoZiomkaDomain.Admin;
using PoZiomkaDomain.Application;
using PoZiomkaDomain.Common.Interface;
using PoZiomkaDomain.Match;
using PoZiomkaDomain.Room;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.StudentAnswers;
using PoZiomkaInfrastructure.Exceptions;
using PoZiomkaInfrastructure.Migrations;
using PoZiomkaInfrastructure.Repositories;
using PoZiomkaInfrastructure.Services;
using System.Data;
using System.Reflection;

namespace PoZiomkaInfrastructure;

public static class Infrastructure
{
    public static void Initalize(IConfiguration configuration)
    {
        var connectionString = configuration["DB:ConnectionString"];

        if (bool.Parse(configuration["DB:Drop"]!))
            DropDatabase.For.SqlDatabase(connectionString);

        EnsureDatabase.For.SqlDatabase(connectionString);

        var upgrader = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), name =>
            {
                if (name.EndsWith("SampleApplication.pdf"))
                    return false;

                if (name.EndsWith("InsertSampleData.sql"))
                    return bool.Parse(configuration["DB:InsertSampleData"]!);
                return true;
            })
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
            throw new InfrastructureException(result.Error.Message);
    }

    public static void Configure(IConfiguration configuration, IServiceCollection services)
    {
        var connectionString = configuration["DB:ConnectionString"];

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
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IStudentAnswerRepository, StudentAnswerRepository>();

        services.AddScoped<IJudgeService, JudgeService>();
        services.AddScoped<IStudentService, StudentService>();

        services.AddScoped<IFileStorage>(_ => new AzureFileStorage(int.Parse(configuration["FileStorage:MaxSize"]!),
            configuration["FileStorage:ConnectionString"]!, configuration["FileStorage:ContainerName"]!)
        );
    }

    public static void RunStartupTasks(IConfiguration configuration, IServiceProvider services)
    {
        if (bool.Parse(configuration["FileStorage:InsertSampleData"]!) == false)
            return;

        using var scope = services.CreateScope();
        var fileStorage = scope.ServiceProvider.GetRequiredService<IFileStorage>();
        InsertSampleDataImpl.InsertSampleDataMethod(fileStorage);
    }
}
