namespace PoZiomkaApi;

public static class Cors
{
    public const string Policy = "Poziomka-Policy";

    public static void Configure(IConfiguration configuration, IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(Policy, policy =>
            {
                policy
                    .WithOrigins(configuration["App:Domain"]!)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }
}
