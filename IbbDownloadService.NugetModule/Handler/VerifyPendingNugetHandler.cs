using IbbDownloadService.NugetModule.Contracts.Commands;
using IbbDownloadService.NugetModule.Entities;
using MediatR;

namespace IbbDownloadService.NugetModule.Handler;

internal class VerifyPendingNugetHandler(NugetDbContext context) : IRequestHandler<VerifyPendingNugetCommand>
{
    public async Task Handle(VerifyPendingNugetCommand request, CancellationToken cancellationToken)
    {
        var nuget = context.Nugets.FirstOrDefault(n => n.Id == request.Id);
        if (nuget is null) return;
        nuget.VerifiedAt = DateTime.UtcNow;
        await context.SaveChangesAsync(cancellationToken);
    }
}