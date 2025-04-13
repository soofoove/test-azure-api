using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Test.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    {
        options.Authority = "https://login.microsoftonline.com/4e726671-6def-427b-a724-60f7c409aaa2";
        options.Audience = "api://6ad5eab3-37b8-47bf-afd7-ff468bb6ac65";
    });

builder.Services.AddAuthorization();
builder.Services.AddAuthorizationBuilder()
  .AddPolicy("weather", policy =>
        policy
            .RequireClaim(ClaimsConstants.Scope, "Weather.Read"));

builder.Services.AddScoped<IPenisRepository, PenisRepository>();
builder.Services.AddDbContext<PenisDbContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSql"));
});

// setup pipeline
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options => 
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (HttpContext context) =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.RequireAuthorization();

app.MapGet("penis", async (IPenisRepository penisRepository, CancellationToken cancellationToken) => 
{
    var penis = await penisRepository.GetAsync(cancellationToken);

    return TypedResults.Ok(penis);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
