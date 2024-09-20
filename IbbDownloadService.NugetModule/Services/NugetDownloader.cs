using IbbDownloadService.NugetModule.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
namespace IbbDownloadService.NugetModule.Services;

internal class NugetDownloader(IServiceProvider serviceProvider, HttpClient httpClient, INugetPackageReader packageReader, ILogger logger) : BackgroundService
{
    private HashSet<string> _existingNugets = new();
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.Information("Nuget downloader started");
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<NugetDbContext>();
            var nugetsToDownload = context.Nugets.Where(x => (x.NeedsVerification == false || x.VerifiedAt != null) && x.DownloadedAt == null);
            logger.Information("Starting Nuget download");
            try
            {
                foreach (var nuget in nugetsToDownload)
                {
                    await LoadNugetPackage(stoppingToken, nuget, context);
                    
                }

                var downloadedNugets = context.Nugets.Where(x => x.DownloadedAt != null);
                foreach (var nuget in downloadedNugets)
                {
                    await LoadDependencies(stoppingToken, nuget, context);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to download nuget packages");
            }
            logger.Information("Finished Nuget download");
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }

    }

    private async Task LoadDependencies(CancellationToken stoppingToken, Nuget nuget, NugetDbContext context)
    {
   
        if (nuget.DownloadedAt == null) return;
        try
        {
            var dependencies = await packageReader.GetDependencies(nuget.Name, nuget.Version);
            foreach (var dependency in dependencies)
            {

                var existingDependency =
                    await context.Nugets.FirstOrDefaultAsync(
                        x => x.Name.ToLower() == dependency.Id.ToLower() && x.Version.ToLower() == dependency.MaxVersion.Trim().ToLower(), stoppingToken);
                if (_existingNugets.Contains($"{dependency.Id.ToLower()}.{dependency.MaxVersion.ToLower()}") ||
                    existingDependency != null)
                {
                    logger.Information("Dependency {Id} {Version} already exists", dependency.Id, dependency.MaxVersion);
                    continue;
                };
                var dependencyNuget = new Nuget
                {
                    Version = dependency.MaxVersion,
                    VerifiedAt = null,
                    NeedsVerification = false,
                    CreatedAt = DateTime.UtcNow,
                    Name = dependency.Id,
                    IsInsertedByUpdater = true
                };
                context.Nugets.Add(dependencyNuget);
                _existingNugets.Add($"{dependencyNuget.Name.ToLower()}.{dependencyNuget.Version.ToLower()}");
                context.NugetDependencies.Add(new NugetDependency()
                {
                    NugetId = nuget.Id,
                    DependencyId = dependencyNuget.Id
                });
                await context.SaveChangesAsync(stoppingToken);
                
            }
        }
        catch (Exception ex)
        {
            logger.Error("Failed to load dependencies for {Id} {Version}", nuget.Name, nuget.Version);
        }

    }

    private async Task LoadNugetPackage(CancellationToken stoppingToken, Nuget nuget, NugetDbContext context)
    {
        try
        {
            var downloadResult = await DownloadPackage(nuget.Name, nuget.Version, stoppingToken);
            if (downloadResult.Length == 0) return;
            var path = Path.Combine("packages", $"{nuget.Name}.{nuget.Version}.nupkg");
            if (!Directory.Exists("packages"))
                Directory.CreateDirectory("packages");
            logger.Information("Downloading package {Id} {Version} to {Path}", nuget.Name, nuget.Version,
                path);
            await File.WriteAllBytesAsync(path, downloadResult, stoppingToken);
            nuget.DownloadedAt = DateTime.UtcNow;
            await context.SaveChangesAsync(stoppingToken);
            _existingNugets.Add($"{nuget.Name.ToLower()}.{nuget.Version.ToLower()}");
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Failed to download package {Id} {Version}", nuget.Name, nuget.Version);
        }
    }

    private async Task<byte[]> DownloadPackage(string id, string version, CancellationToken cancellationToken)
    {
        var url = $"https://api.nuget.org/v3-flatcontainer/{id.ToLower()}/{version.ToLower()}/{id.ToLower()}.{version.ToLower()}.nupkg";
        var response = await httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }
}