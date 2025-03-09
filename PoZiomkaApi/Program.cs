using PoZiomkaApi;
using PoZiomkaDomain;
using PoZiomkaInfrastructure;

var builder = WebApplication.CreateBuilder(args);

Infrastructure.Initalize(builder.Configuration);
Infrastructure.Configure(builder.Configuration, builder.Services);
Domain.Configure(builder.Configuration, builder.Services);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UsePathBase("/api");

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
