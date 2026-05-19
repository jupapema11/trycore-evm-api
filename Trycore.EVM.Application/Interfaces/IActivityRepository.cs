using Trycore.EVM.Domain.Entitites;

namespace Trycore.EVM.Application.Interfaces;

public interface IActivityRepository
{
    Task<Activity> CreateAsync(Activity activity);

    Task<List<Activity>> GetByProjectIdAsync(Guid projectId);

    Task<Activity?> GetByIdAsync(Guid id);

    Task<bool> UpdateAsync(Activity activity);

    Task<bool> DeleteAsync(Guid id);

    Task SaveChangesAsync();
}
