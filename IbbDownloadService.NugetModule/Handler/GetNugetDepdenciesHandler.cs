using IbbDownloadService.NugetModule.Contracts.Queries;
using IbbDownloadService.NugetModule.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NugetDependency = IbbDownloadService.NugetModule.Contracts.DTOs.NugetDependency;

namespace IbbDownloadService.NugetModule.Handler;

internal class GetNugetDepdenciesHandler(NugetDbContext context)
    : IRequestHandler<GetDependenciesQuery, IList<NugetDependency>>
{

    public async Task<IList<NugetDependency>> Handle(GetDependenciesQuery request, CancellationToken cancellationToken)
    {
        var nuget = context.Nugets.FirstOrDefault(x => x.Id == request.Id);
        if (nuget == null) return new List<NugetDependency>();
        var dependencyIds = await context.NugetDependencies
            .Where(x => x.NugetId == nuget.Id)
            .Select(x=> x.DependencyId)
            .ToListAsync(cancellationToken);
        var dependencies = context.Nugets.Where(x => dependencyIds.Contains(x.Id));
        var result = dependencies.Select(x => new NugetDependency()
        {
            Id = x.Id,
            Name = x.Name,
            Version = x.Version
        }).ToList();
        return result;
    }
}