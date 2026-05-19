using Microsoft.EntityFrameworkCore;
using Trycore.EVM.API.Configuration;
using Trycore.EVM.Application.Interfaces;
using Trycore.EVM.Application.Services;
using Trycore.EVM.Infrastructure.Persistence;
using Trycore.EVM.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var isTesting = builder.Environment.IsEnvironment("Testing");
var connectionString = ConnectionStringFactory.Resolve(builder.Configuration);

if (!isTesting)
{
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException(
            "Database connection not configured. Set ConnectionStrings__DefaultConnection or DATABASE_URL.");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("TrycoreEvmTests"));
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Trycore EVM API", Version = "v1" });
});

builder.Services.AddScoped<IEvmCalculationService, EvmCalculationService>();
builder.Services.AddScoped<IEvmPerformanceInterpreter, EvmPerformanceInterpreter>();
builder.Services.AddScoped<EvmMetricsBuilder>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IActivityService, ActivityService>();

var corsOrigins = builder.Configuration["CORS_ORIGINS"]?
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    ?? ["http://localhost:4200"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(corsOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}
else if (!isTesting)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trycore EVM API v1");
    c.RoutePrefix = string.Empty;
});

if (app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseCors("AllowFrontend");
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));
app.MapControllers();

app.Run();

public partial class Program;
