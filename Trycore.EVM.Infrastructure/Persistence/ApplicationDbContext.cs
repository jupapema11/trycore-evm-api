using Microsoft.EntityFrameworkCore;
using Trycore.EVM.Domain.Entitites;

namespace Trycore.EVM.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects => Set<Project>();

    public DbSet<Activity> Activities => Set<Activity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Activities)
            .WithOne(a => a.Project)
            .HasForeignKey(a => a.ProjectId);
    }
}