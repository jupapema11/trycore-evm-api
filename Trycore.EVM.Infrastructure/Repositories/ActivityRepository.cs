using Trycore.EVM.Application.Interfaces;
using Trycore.EVM.Domain.Entitites;
using Trycore.EVM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Trycore.EVM.Infrastructure.Repositories;

public class ActivityRepository : IActivityRepository
{
    private readonly ApplicationDbContext _context;

    public ActivityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Activity> CreateAsync(Activity activity)
    {
        await _context.Activities.AddAsync(activity);

        return activity;
    }

    public async Task<List<Activity>> GetByProjectIdAsync(Guid projectId)
    {
        return await _context.Activities
            .Where(a => a.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<Activity?> GetByIdAsync(Guid id)
    {
        return await _context.Activities.FindAsync(id);
    }

    public async Task<bool> UpdateAsync(Activity activity)
    {
        var existing = await _context.Activities.FindAsync(activity.Id);

        if (existing is null)
            return false;

        existing.Name = activity.Name;
        existing.BudgetAtCompletion = activity.BudgetAtCompletion;
        existing.PlannedProgressPercent = activity.PlannedProgressPercent;
        existing.ActualProgressPercent = activity.ActualProgressPercent;
        existing.ActualCost = activity.ActualCost;

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var activity = await _context.Activities.FindAsync(id);

        if (activity is null)
            return false;

        _context.Activities.Remove(activity);

        return true;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
