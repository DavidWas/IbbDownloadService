namespace IbbDownloadService.NugetModule.Entities;

public class NugetDependency
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid NugetId { get; init; }
    public Guid DependencyId { get; init; }
}