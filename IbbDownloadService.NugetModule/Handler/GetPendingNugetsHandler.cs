using IbbDownloadService.NugetModule.Contracts.DTOs;
using IbbDownloadService.NugetModule.Contracts.Queries;
using IbbDownloadService.NugetModule.Entities;
using MediatR;

namespace IbbDownloadService.NugetModule.Handler;

internal class GetPendingNugetsHandler(NugetDbContext context)
    : IRequestHandler<GetPendingNugetsQuery, IList<PendingNuget>>
{
    public Task<IList<PendingNuget>> Handle(GetPendingNugetsQuery request, CancellationToken cancellationToken)
    {
        var result = context.Nugets.Where(x => x.VerifiedAt == null && x.NeedsVerification).ToList();
        IList<PendingNuget> pendingNugets = result.Select(x => new PendingNuget()
        {
            Id = x.Id,
            Name = x.Name,
            Version = x.Version,
            Md5Hash = x.Md5,
            CreatedAt = x.CreatedAt
        }).ToList();
        return Task.FromResult(pendingNugets);
    }
}