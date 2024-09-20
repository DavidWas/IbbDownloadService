using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace IbbDownloadService.NugetModule.Entities;

internal class NugetDbContext : DbContext
{
    public NugetDbContext(DbContextOptions<NugetDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Nugets");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<Nuget>().HasIndex(x => new { x.Name, x.Version }).IsUnique();
    }

    public DbSet<Nuget> Nugets { get; set; } = null!;
    public DbSet<NugetDependency> NugetDependencies { get; set; } = null!;
}