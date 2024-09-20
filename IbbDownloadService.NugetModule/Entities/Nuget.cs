namespace IbbDownloadService.NugetModule.Entities;

internal class Nuget
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = "";
    public string Version { get; init; } = "";
    public string Md5 { get; init; } = "";
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public bool NeedsVerification { get; set; } = true;
}