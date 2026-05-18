using System;
using System.Collections.Generic;
using System.Text;
using Trycore.EVM.Domain.Entitites;

namespace Trycore.EVM.Application.Interfaces
{
    public interface IActivityRepository
    {
        Task<Activity> CreateAsync(Activity activity);

        Task<List<Activity>> GetByProjectIdAsync(Guid projectId);

        Task SaveChangesAsync();
    }
}
