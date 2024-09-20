
using IbbDownloadService.NugetModule.Contracts.Commands;
using IbbDownloadService.NugetModule.Entities;
using MediatR;

namespace IbbDownloadService.NugetModule.Handler;

internal class AddPendingNugetHandler : IRequestHandler<AddPendingNugetCommand, Guid>
{
    private readonly NugetDbContext _context;

    public AddPendingNugetHandler(NugetDbContext context)
    {
        _context = context;
    }
    public async Task<Guid> Handle(AddPendingNugetCommand request, CancellationToken cancellationToken)
    {
        
        var storedEntity = _context.Nugets.FirstOrDefault(x =>
           x.Name.ToLower() == request.Name.Trim().ToLower() && x.Version.ToLower() == request.Version.Trim().ToLower());
        if (storedEntity != null) return storedEntity.Id;
        storedEntity = new Nuget()
        {
            Name = request.Name,
            Version = request.Version,
            Md5 = request.Md5,
            CreatedAt = DateTime.Now
        };
        _context.Nugets.Add(storedEntity);
        await _context.SaveChangesAsync(cancellationToken);

        return storedEntity.Id;
    }
}