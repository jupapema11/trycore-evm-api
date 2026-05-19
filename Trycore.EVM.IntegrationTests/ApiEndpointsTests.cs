using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Trycore.EVM.Application.DTOs;

namespace Trycore.EVM.IntegrationTests;

public class ApiEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ApiEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task PostProject_ReturnsCreatedProjectContract()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/Projects",
            new CreateProjectDto { Name = "Proyecto Demo" });

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var project = await response.Content.ReadFromJsonAsync<ProjectResponseDto>();

        project.Should().NotBeNull();
        project!.Name.Should().Be("Proyecto Demo");
        project.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetProjects_ReturnsProjectListContract()
    {
        await _client.PostAsJsonAsync(
            "/api/Projects",
            new CreateProjectDto { Name = "Proyecto Listado" });

        var response = await _client.GetAsync("/api/Projects");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var projects = await response.Content.ReadFromJsonAsync<List<ProjectResponseDto>>();

        projects.Should().NotBeNull();
        projects!.Should().NotBeEmpty();
    }

    [Fact]
    public async Task PutProject_ReturnsUpdatedProjectContract()
    {
        var createResponse = await _client.PostAsJsonAsync(
            "/api/Projects",
            new CreateProjectDto { Name = "Antes" });

        var created = await createResponse.Content.ReadFromJsonAsync<ProjectResponseDto>();

        var response = await _client.PutAsJsonAsync(
            $"/api/Projects/{created!.Id}",
            new UpdateProjectDto { Name = "Después" });

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await response.Content.ReadFromJsonAsync<ProjectResponseDto>();

        updated!.Name.Should().Be("Después");
    }

    [Fact]
    public async Task PostActivity_ReturnsActivityWithEvmIndicators()
    {
        var project = await CreateProjectAsync("Proyecto Actividades");

        var response = await _client.PostAsJsonAsync(
            $"/api/projects/{project.Id}/activities",
            new CreateActivityDto
            {
                Name = "Actividad 1",
                BudgetAtCompletion = 10000,
                PlannedProgressPercent = 50,
                ActualProgressPercent = 40,
                ActualCost = 5000
            });

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var activity = await response.Content.ReadFromJsonAsync<ActivityResponseDto>();

        activity.Should().NotBeNull();
        activity!.PV.Should().Be(5000);
        activity.EV.Should().Be(4000);
        activity.CPI.Should().Be(0.8m);
        activity.SPI.Should().Be(0.8m);
        activity.CpiInterpretation.Should().NotBeNullOrWhiteSpace();
        activity.SpiInterpretation.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetActivities_ReturnsActivitiesForProject()
    {
        var project = await CreateProjectAsync("Proyecto Consulta");

        await _client.PostAsJsonAsync(
            $"/api/projects/{project.Id}/activities",
            new CreateActivityDto
            {
                Name = "Actividad A",
                BudgetAtCompletion = 5000,
                PlannedProgressPercent = 20,
                ActualProgressPercent = 10,
                ActualCost = 400
            });

        var response = await _client.GetAsync($"/api/projects/{project.Id}/activities");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var activities = await response.Content.ReadFromJsonAsync<List<ActivityResponseDto>>();

        activities.Should().NotBeNull();
        activities!.Should().HaveCount(1);
    }

    [Fact]
    public async Task PutActivity_ReturnsUpdatedIndicators()
    {
        var project = await CreateProjectAsync("Proyecto Update");

        var createResponse = await _client.PostAsJsonAsync(
            $"/api/projects/{project.Id}/activities",
            new CreateActivityDto
            {
                Name = "Actividad Editable",
                BudgetAtCompletion = 10000,
                PlannedProgressPercent = 50,
                ActualProgressPercent = 40,
                ActualCost = 5000
            });

        var created = await createResponse.Content.ReadFromJsonAsync<ActivityResponseDto>();

        var response = await _client.PutAsJsonAsync(
            $"/api/projects/{project.Id}/activities/{created!.Id}",
            new UpdateActivityDto
            {
                Name = "Actividad Actualizada",
                BudgetAtCompletion = 10000,
                PlannedProgressPercent = 60,
                ActualProgressPercent = 50,
                ActualCost = 4500
            });

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await response.Content.ReadFromJsonAsync<ActivityResponseDto>();

        updated!.Name.Should().Be("Actividad Actualizada");
        updated.PV.Should().Be(6000);
        updated.EV.Should().Be(5000);
    }

    [Fact]
    public async Task DeleteActivity_ReturnsNoContent()
    {
        var project = await CreateProjectAsync("Proyecto Delete");

        var createResponse = await _client.PostAsJsonAsync(
            $"/api/projects/{project.Id}/activities",
            new CreateActivityDto
            {
                Name = "Actividad Temporal",
                BudgetAtCompletion = 1000,
                PlannedProgressPercent = 10,
                ActualProgressPercent = 5,
                ActualCost = 100
            });

        var created = await createResponse.Content.ReadFromJsonAsync<ActivityResponseDto>();

        var response = await _client.DeleteAsync(
            $"/api/projects/{project.Id}/activities/{created!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetProjectSummary_ReturnsConsolidatedEvmContract()
    {
        var project = await CreateProjectAsync("Proyecto Resumen");

        await _client.PostAsJsonAsync(
            $"/api/projects/{project.Id}/activities",
            new CreateActivityDto
            {
                Name = "A1",
                BudgetAtCompletion = 10000,
                PlannedProgressPercent = 50,
                ActualProgressPercent = 40,
                ActualCost = 5000
            });

        var response = await _client.GetAsync($"/api/Projects/{project.Id}/summary");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var summary = await response.Content.ReadFromJsonAsync<ProjectSummaryDto>();

        summary.Should().NotBeNull();
        summary!.TotalPV.Should().Be(5000);
        summary.TotalEV.Should().Be(4000);
        summary.TotalAC.Should().Be(5000);
        summary.CpiInterpretation.Should().NotBeNullOrWhiteSpace();
        summary.SpiInterpretation.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task DeleteProject_ReturnsNoContent()
    {
        var project = await CreateProjectAsync("Proyecto Eliminar");

        var response = await _client.DeleteAsync($"/api/Projects/{project.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private async Task<ProjectResponseDto> CreateProjectAsync(string name)
    {
        var response = await _client.PostAsJsonAsync(
            "/api/Projects",
            new CreateProjectDto { Name = name });

        return (await response.Content.ReadFromJsonAsync<ProjectResponseDto>())!;
    }
}
