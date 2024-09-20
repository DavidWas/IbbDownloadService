using IbbDownloadService.NugetModule.Contracts.DTOs;
using MediatR;

namespace IbbDownloadService.NugetModule.Contracts.Queries;

public class GetDependenciesQuery : IRequest<IList<NugetDependency>>
{
    public Guid Id { get; init; }
}