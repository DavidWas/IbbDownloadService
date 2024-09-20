using System.Runtime.InteropServices.JavaScript;

namespace IbbDownloadService.NugetModule.Contracts.DTOs;

public class VerifiedNuget
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string Version { get; init; } = "";
    public string Md5Hash { get; init; } = "";
    public DateTime CreatedAt { get; init; }
    public DateTime? VerifiedAt { get; init; }
    public DateTime? DownloadedAt { get; init; }
}