using Trycore.EVM.Application.DTOs;
using Trycore.EVM.Application.Interfaces;
using Trycore.EVM.Domain.Entitites;

namespace Trycore.EVM.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;
    private readonly IEvmCalculationService _evmService;
    private readonly IEvmPerformanceInterpreter _interpreter;

    public ProjectService(
        IProjectRepository repository,
        IEvmCalculationService evmService,
        IEvmPerformanceInterpreter interpreter)
    {
        _repository = repository;
        _evmService = evmService;
        _interpreter = interpreter;
    }

    public async Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = dto.Name
        };

        await _repository.CreateAsync(project);
        await _repository.SaveChangesAsync();

        return MapProject(project);
    }

    public async Task<List<ProjectResponseDto>> GetAllAsync()
    {
        var projects = await _repository.GetAllAsync();

        return projects.Select(MapProject).ToList();
    }

    public async Task<ProjectResponseDto?> UpdateAsync(Guid id, UpdateProjectDto dto)
    {
        var project = await _repository.GetByIdAsync(id);

        if (project is null)
            return null;

        project.Name = dto.Name;

        await _repository.UpdateAsync(project);
        await _repository.SaveChangesAsync();

        return MapProject(project);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _repository.DeleteAsync(id);

        if (!deleted)
            return false;

        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<ProjectSummaryDto?> GetSummaryAsync(Guid projectId)
    {
        var project = await _repository.GetByIdAsync(projectId);

        if (project is null)
            return null;

        if (project.Activities.Count == 0)
        {
            return new ProjectSummaryDto
            {
                ProjectId = project.Id,
                ProjectName = project.Name,
                CpiInterpretation = "Sin actividades registradas",
                SpiInterpretation = "Sin actividades registradas"
            };
        }

        decimal totalPV = 0;
        decimal totalEV = 0;
        decimal totalAC = 0;
        decimal totalBAC = 0;

        foreach (var activity in project.Activities)
        {
            totalPV += _evmService.CalculatePV(
                activity.PlannedProgressPercent,
                activity.BudgetAtCompletion);

            totalEV += _evmService.CalculateEV(
                activity.ActualProgressPercent,
                activity.BudgetAtCompletion);

            totalAC += activity.ActualCost;
            totalBAC += activity.BudgetAtCompletion;
        }

        var totalCV = _evmService.CalculateCV(totalEV, totalAC);
        var totalSV = _evmService.CalculateSV(totalEV, totalPV);
        var totalCPI = _evmService.CalculateCPI(totalEV, totalAC);
        var totalSPI = _evmService.CalculateSPI(totalEV, totalPV);
        var totalEAC = _evmService.CalculateEAC(totalBAC, totalCPI);
        var totalVAC = _evmService.CalculateVAC(totalBAC, totalEAC);

        return new ProjectSummaryDto
        {
            ProjectId = project.Id,
            ProjectName = project.Name,
            TotalPV = totalPV,
            TotalEV = totalEV,
            TotalAC = totalAC,
            TotalBAC = totalBAC,
            TotalCV = totalCV,
            TotalSV = totalSV,
            TotalCPI = totalCPI,
            TotalSPI = totalSPI,
            TotalEAC = totalEAC,
            TotalVAC = totalVAC,
            CpiInterpretation = _interpreter.InterpretCpi(totalCPI, totalEV, totalAC),
            SpiInterpretation = _interpreter.InterpretSpi(totalSPI, totalEV, totalPV)
        };
    }

    private static ProjectResponseDto MapProject(Project project) =>
        new()
        {
            Id = project.Id,
            Name = project.Name
        };
}
