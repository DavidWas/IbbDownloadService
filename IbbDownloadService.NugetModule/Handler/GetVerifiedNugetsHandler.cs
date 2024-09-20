using IbbDownloadService.NugetModule.Contracts.DTOs;
using IbbDownloadService.NugetModule.Contracts.Queries;
using IbbDownloadService.NugetModule.Entities;
using MediatR;

namespace IbbDownloadService.NugetModule.Handler;

internal class GetVerifiedNugetsHandler : IRequestHandler<GetVerifiedNugetsQuery, IList<VerifiedNuget>>
{
    private readonly NugetDbContext _context;

    public GetVerifiedNugetsHandler(NugetDbContext context)
    {
        _context = context;
    }
    public Task<IList<VerifiedNuget>> Handle(GetVerifiedNugetsQuery request, CancellationToken cancellationToken)
    {
        var verifiedNugets = _context.Nugets.Where(x => x.VerifiedAt != null || x.IsUpdate == true)
            .OrderBy(x => x.Name);
        IList<VerifiedNuget> result = verifiedNugets.Select(x => new VerifiedNuget()
        {
            Id = x.Id,
            Name = x.Name,
            Version = x.Version,
            Md5Hash = x.Md5,
            CreatedAt = x.CreatedAt,
            VerifiedAt = x.VerifiedAt,
            DownloadedAt = x.DownloadedAt
        }).ToList();
        return Task.FromResult(result);
    }
}