using System.IO.Compression;
using System.Text.Json;

namespace IbbDownloadService.NugetModule.Services;

internal interface INugetPackageReader
{
    Task<NugetDependencyData[]> GetDependencies(string id, string version);
}

internal class NugetPackageReader(HttpClient httpClient) : INugetPackageReader
{
    
    public async Task<NugetDependencyData[]> GetDependencies(string id, string version)
    {
        var catalogEntryUrl = await GetCatalogEntryUrl(id, version);
        if (string.IsNullOrEmpty(catalogEntryUrl))
        {
            return Array.Empty<NugetDependencyData>();
        }

        var dependencies = await FetchDependenciesFromCatalog(catalogEntryUrl);
        return dependencies.ToArray();
    }
    
    private async Task<string?> GetCatalogEntryUrl(string id, string version)
    {
        var url = $"https://api.nuget.org/v3/registration5-gz-semver2/{id.ToLower()}/{version}.json";
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        await using var responseStream = await response.Content.ReadAsStreamAsync();
        await using var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress);
        using var streamReader = new StreamReader(decompressedStream);

        var content = await streamReader.ReadToEndAsync();
        using var jsonDocument = JsonDocument.Parse(content);

        if (jsonDocument.RootElement.TryGetProperty("catalogEntry", out var catalogEntryElement))
        {
            return catalogEntryElement.GetString();
        }

        return null;
    }
    
    private async Task<List<NugetDependencyData>> FetchDependenciesFromCatalog(string catalogEntryUrl)
    {
        var catalogEntryResponse = await httpClient.GetAsync(catalogEntryUrl);
        catalogEntryResponse.EnsureSuccessStatusCode();
        var catalogEntryContent = await catalogEntryResponse.Content.ReadAsStringAsync();

        using var catalogEntryDocument = JsonDocument.Parse(catalogEntryContent);
        var dependencies = new List<NugetDependencyData>();

        if (catalogEntryDocument.RootElement.TryGetProperty("dependencyGroups", out var dependencyGroups))
        {
            foreach (var group in dependencyGroups.EnumerateArray())
            {
                if (group.TryGetProperty("dependencies", out var dependenciesElement))
                {
                    foreach (var dependency in dependenciesElement.EnumerateArray())
                    {
                        var id = dependency.GetProperty("id").GetString() ?? string.Empty;
                        var range = dependency.GetProperty("range").GetString() ?? string.Empty;

                        dependencies.Add(new NugetDependencyData(id, range));
                    }
                }
            }
        }

        return dependencies;
    }
}