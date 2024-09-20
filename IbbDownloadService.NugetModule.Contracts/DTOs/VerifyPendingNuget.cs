namespace IbbDownloadService.NugetModule.Contracts.DTOs;

public class VerifyPendingNuget
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string Version { get; init; } = "";
}