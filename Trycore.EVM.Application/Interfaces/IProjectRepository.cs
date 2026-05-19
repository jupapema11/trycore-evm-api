using Trycore.EVM.Domain.Entitites;

namespace Trycore.EVM.Application.Interfaces;

public interface IProjectRepository
{
    Task<Project> CreateAsync(Project project);

    Task<List<Project>> GetAllAsync();

    Task<Project?> GetByIdAsync(Guid id);

    Task<bool> UpdateAsync(Project project);

    Task<bool> DeleteAsync(Guid id);

    Task SaveChangesAsync();
}
