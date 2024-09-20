using NuGet.Versioning;

namespace IbbDownloadService.NugetModule.Services;

public class NugetDependencyData
{
    public string Id { get; }
    public string Range { get; }
    public string MaxVersion { get; }

    public NugetDependencyData(string id, string range)
    {
        Id = id;
        Range = range;
        if (VersionRange.TryParse(range, out var versionRange))
        {
            MaxVersion = versionRange.HasUpperBound
                ? versionRange.MaxVersion.ToString()
                : versionRange.MinVersion?.ToString() ?? "Unknown";
        }
        else
        {
            MaxVersion = "Unknown";
        }
    }
}