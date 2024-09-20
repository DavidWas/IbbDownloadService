using System.Text.Json;
using System.Text.Json.Serialization;
using IbbDownloadService.NugetModule.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IbbDownloadService.NugetModule.Services;

internal class NugetVersionSync(IServiceProvider serviceProvider, ILogger<NugetVersionSync> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<NugetDbContext>();
            logger.LogInformation("Starting Nuget version sync");
            var nugetsToCrawl = context.Nugets.Where(x => x.VerifiedAt != null);
            foreach (var nuget in nugetsToCrawl)
            {
                try
                {
                    await SyncNuget(stoppingToken, nuget, context);
                }
                catch (Exception ex)
                {
                    logger.LogError("Failed to sync nuget {NugetName} with error {Error}", nuget.Name, ex.Message);
                }
            }
            logger.LogInformation("Finished Nuget version sync");
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task SyncNuget(CancellationToken stoppingToken, Nuget nuget, NugetDbContext context)
    {
        var latestVersion = await GetLatestVersion(nuget.Name, stoppingToken);
        if (latestVersion == null || latestVersion == nuget.Version) return;
        var loadedNuget = context.Nugets.FirstOrDefault(x =>
            x.Name.ToLower() == nuget.Name.ToLower() &&
            x.Version.ToLower() == latestVersion.ToLower());
        if (loadedNuget != null) return;
        var createdNuget = new Nuget
        {
            Version = latestVersion,
            VerifiedAt = null,
            NeedsVerification = false,
            CreatedAt = DateTime.UtcNow,
            Name = nuget.Name,
            IsInsertedByUpdater = true
        };
        context.Nugets.Add(createdNuget);
        await context.SaveChangesAsync(stoppingToken);
    }

    private async Task<string?> GetLatestVersion(string packageName, CancellationToken cancellationToken)
    {
        var httpClient = serviceProvider.GetRequiredService<HttpClient>();
        try
        {
            var requestUrl = $"https://api.nuget.org/v3-flatcontainer/{packageName.ToLower()}/index.json";
            var response = await httpClient.GetAsync(requestUrl, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var data = JsonSerializer.Deserialize<NugetApiResponse>(content);
                var stableVersions = data?.Versions?.Where(v => !v.Contains('-')).ToList();
                return stableVersions?.LastOrDefault();
            }

            return null;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching latest version for {packageName}: {ex.Message}");
            return null;
        }
    }

    private class NugetApiResponse
    {
        [JsonPropertyName("versions")] public List<string> Versions { get; set; } = new List<string>();
    }
}