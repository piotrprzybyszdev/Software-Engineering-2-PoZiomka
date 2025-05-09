using PoZiomkaApi;
using PoZiomkaDomain;
using PoZiomkaInfrastructure;


var builder = WebApplication.CreateBuilder(args);

Infrastructure.Initalize(builder.Configuration);
Infrastructure.Configure(builder.Configuration, builder.Services);
Domain.Configure(builder.Configuration, builder.Services);

Authentication.Configure(builder.Configuration, builder.Services);
Cors.Configure(builder.Configuration, builder.Services);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCustomSwagger();

var app = builder.Build();

Infrastructure.RunStartupTasks(builder.Configuration, app.Services);

app.UsePathBase("/api");

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors(Cors.Policy);
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
