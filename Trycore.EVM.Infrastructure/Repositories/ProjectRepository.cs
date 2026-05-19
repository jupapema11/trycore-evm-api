using Trycore.EVM.Application.Interfaces;
using Trycore.EVM.Domain.Entitites;
using Trycore.EVM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Trycore.EVM.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Project> CreateAsync(Project project)
    {
        await _context.Projects.AddAsync(project);

        return project;
    }

    public async Task<List<Project>> GetAllAsync()
    {
        return await _context.Projects
            .Include(p => p.Activities)
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await _context.Projects
            .Include(p => p.Activities)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> UpdateAsync(Project project)
    {
        var existing = await _context.Projects.FindAsync(project.Id);

        if (existing is null)
            return false;

        existing.Name = project.Name;

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var project = await _context.Projects
            .Include(p => p.Activities)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project is null)
            return false;

        _context.Activities.RemoveRange(project.Activities);
        _context.Projects.Remove(project);

        return true;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
