using IbbDownloadService.NugetModule.Contracts.DTOs;
using MediatR;

namespace IbbDownloadService.NugetModule.Contracts.Queries;

public class GetPendingNugetsQuery : IRequest<IList<PendingNuget>>
{
}