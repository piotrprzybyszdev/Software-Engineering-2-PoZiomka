namespace PoZiomkaApi;

public static class Authentication
{
    public static class Roles
    {
        public const string Student = "Student";
        public const string Administrator = "Administrator";
    }

    public static void Configure(IConfiguration configuration, IServiceCollection services)
    {
        services.AddAuthentication().AddCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            options.SlidingExpiration = true;

            options.Events.OnRedirectToLogin = (context) =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
        });
    }
}
