using System;
using System.Collections.Generic;
using System.Text;
using Trycore.EVM.Domain.Entitites;

namespace Trycore.EVM.Application.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project> CreateAsync(Project project);

        Task<List<Project>> GetAllAsync();

        Task<Project?> GetByIdAsync(Guid id);

        Task SaveChangesAsync();
    }
}
