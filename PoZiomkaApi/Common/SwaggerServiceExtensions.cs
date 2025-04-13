using Microsoft.OpenApi.Models;
using System.Reflection;

public static class SwaggerServiceExtensions
{
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "PoZiomkaApi",
                Version = "v1"
            });

            // Enable XML comments (for <summary>, <example>, etc.)
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            options.IncludeXmlComments(xmlPath);
        });

        return services;
    }
}
