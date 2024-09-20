using MediatR;

namespace IbbDownloadService.NugetModule.Contracts.Commands;

public class AddPendingNugetCommand : IRequest<Guid>
{
    public string Name { get; set; } = "";
    public string Version { get; set; } = "";
    public string Md5 { get; set; } = "";
}