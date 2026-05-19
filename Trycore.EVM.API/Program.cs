using Microsoft.EntityFrameworkCore;
using Trycore.EVM.Application.Interfaces;
using Trycore.EVM.Application.Services;
using Trycore.EVM.Infrastructure.Persistence;
using Trycore.EVM.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var isTesting = builder.Environment.IsEnvironment("Testing");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (isTesting)
        options.UseInMemoryDatabase("TrycoreEvmTests");
    else
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
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

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trycore EVM API v1");
    c.RoutePrefix = string.Empty;
});

if (!app.Environment.IsEnvironment("Testing"))
    app.UseHttpsRedirection();
app.UseCors("AllowLocalhost");
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program;
