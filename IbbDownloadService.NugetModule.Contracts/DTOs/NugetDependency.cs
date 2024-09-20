namespace IbbDownloadService.NugetModule.Contracts.DTOs;

public class NugetDependency
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string Version { get; init; } = "";
}