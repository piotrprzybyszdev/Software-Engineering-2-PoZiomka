﻿using DbUp;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoZiomkaDomain.Common;
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

        services.AddScoped<IStudentRepository, StudentRepository>();
    }
}
