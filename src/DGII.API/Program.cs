using DGII.API.Middleware;
using DGII.Application;
using DGII.Infrastructure;
using DGII.Infrastructure.Context;
using DGII.Infrastructure.Seeding;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Native exception handling and ProblemDetails in .NET 9
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Clean Architecture layer dependencies
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Swagger / OpenAPI documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DGII API - Contribuyentes y Comprobantes Fiscales",
        Version = "v1",
        Description = "API técnica para la Dirección General de Impuestos Internos (DGII)"
    });
});

// CORS configuration for Client Frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClientApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Idiomatic Global Exception Handler
app.UseExceptionHandler();

// Enable Swagger UI across environments
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "DGII API v1");
    options.RoutePrefix = "swagger";
});

app.UseCors("AllowClientApp");

app.UseAuthorization();

app.MapControllers();

// Ensure SQLite database is created and seeded with test cases
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<DgiiDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        await DgiiDataSeeder.SeedAsync(dbContext, logger);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al inicializar o sembrar la base de datos.");
    }
}

app.Run();

// Partial class for WebApplicationFactory in integration tests if needed
public partial class Program { }
