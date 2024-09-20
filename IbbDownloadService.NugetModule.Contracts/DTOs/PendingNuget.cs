namespace IbbDownloadService.NugetModule.Contracts.DTOs;

public class PendingNuget
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string Version { get; init; } = "";
    public string Md5Hash { get; init; } = "";
    public DateTime CreatedAt { get; init; }
}