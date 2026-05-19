using Trycore.EVM.Application.DTOs;
using Trycore.EVM.Application.Interfaces;
using Trycore.EVM.Domain.Entitites;

namespace Trycore.EVM.Application.Services;

public class ActivityService : IActivityService
{
    private readonly IActivityRepository _activityRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly EvmMetricsBuilder _metricsBuilder;

    public ActivityService(
        IActivityRepository activityRepository,
        IProjectRepository projectRepository,
        EvmMetricsBuilder metricsBuilder)
    {
        _activityRepository = activityRepository;
        _projectRepository = projectRepository;
        _metricsBuilder = metricsBuilder;
    }

    public async Task<ActivityResponseDto> CreateAsync(Guid projectId, CreateActivityDto dto)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);

        if (project is null)
            throw new KeyNotFoundException($"Project {projectId} was not found.");

        var activity = new Activity
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            Name = dto.Name,
            BudgetAtCompletion = dto.BudgetAtCompletion,
            PlannedProgressPercent = dto.PlannedProgressPercent,
            ActualProgressPercent = dto.ActualProgressPercent,
            ActualCost = dto.ActualCost
        };

        await _activityRepository.CreateAsync(activity);
        await _activityRepository.SaveChangesAsync();

        return BuildResponse(activity);
    }

    public async Task<List<ActivityResponseDto>> GetByProjectIdAsync(Guid projectId)
    {
        var activities = await _activityRepository.GetByProjectIdAsync(projectId);

        return activities.Select(BuildResponse).ToList();
    }

    public async Task<ActivityResponseDto?> UpdateAsync(
        Guid projectId,
        Guid activityId,
        UpdateActivityDto dto)
    {
        var activity = await _activityRepository.GetByIdAsync(activityId);

        if (activity is null || activity.ProjectId != projectId)
            return null;

        activity.Name = dto.Name;
        activity.BudgetAtCompletion = dto.BudgetAtCompletion;
        activity.PlannedProgressPercent = dto.PlannedProgressPercent;
        activity.ActualProgressPercent = dto.ActualProgressPercent;
        activity.ActualCost = dto.ActualCost;

        await _activityRepository.UpdateAsync(activity);
        await _activityRepository.SaveChangesAsync();

        return BuildResponse(activity);
    }

    public async Task<bool> DeleteAsync(Guid projectId, Guid activityId)
    {
        var activity = await _activityRepository.GetByIdAsync(activityId);

        if (activity is null || activity.ProjectId != projectId)
            return false;

        await _activityRepository.DeleteAsync(activityId);
        await _activityRepository.SaveChangesAsync();

        return true;
    }

    private ActivityResponseDto BuildResponse(Activity activity)
    {
        var metrics = _metricsBuilder.Build(
            activity.PlannedProgressPercent,
            activity.ActualProgressPercent,
            activity.BudgetAtCompletion,
            activity.ActualCost);

        return new ActivityResponseDto
        {
            Id = activity.Id,
            Name = activity.Name,
            BudgetAtCompletion = activity.BudgetAtCompletion,
            PlannedProgressPercent = activity.PlannedProgressPercent,
            ActualProgressPercent = activity.ActualProgressPercent,
            ActualCost = activity.ActualCost,
            PV = metrics.PV,
            EV = metrics.EV,
            CV = metrics.CV,
            SV = metrics.SV,
            CPI = metrics.CPI,
            SPI = metrics.SPI,
            EAC = metrics.EAC,
            VAC = metrics.VAC,
            CpiInterpretation = metrics.CpiInterpretation,
            SpiInterpretation = metrics.SpiInterpretation
        };
    }
}
