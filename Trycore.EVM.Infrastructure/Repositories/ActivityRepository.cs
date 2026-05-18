using System;
using System.Collections.Generic;
using System.Text;
using Trycore.EVM.Application.Interfaces;
using Trycore.EVM.Domain.Entitites;
using Trycore.EVM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Trycore.EVM.Infrastructure.Repositories
{
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

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
