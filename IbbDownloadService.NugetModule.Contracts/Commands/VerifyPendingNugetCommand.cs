using MediatR;

namespace IbbDownloadService.NugetModule.Contracts.Commands;

public class VerifyPendingNugetCommand : IRequest
{
    public Guid Id { get; set; }
}