using System.Text.Json;
using System.Text.Json.Serialization;
using IbbDownloadService.NugetModule.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IbbDownloadService.NugetModule.Services;

internal class NugetVersionSync(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<NugetDbContext>();
            var nugetsToCrawl = context.Nugets.Where(x => x.VerifiedAt != null || x.NeedsVerification == false);
            foreach (var nuget in nugetsToCrawl)
            {
                var latestVersion = await GetLatestVersion(nuget.Name, stoppingToken);
                if (latestVersion == null || latestVersion == nuget.Version) continue;
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
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task<string?> GetLatestVersion(string packageName, CancellationToken cancellationToken)
    {
        var httpClient = serviceProvider.GetRequiredService<HttpClient>();
        try
        {
            var requestUrl = $"https://api.nuget.org/v3-flatcontainer/{packageName}/index.json";
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
        [JsonPropertyName("versions")]
        public List<string> Versions { get; set; } = new List<string>();
    }
}