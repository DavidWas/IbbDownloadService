using MediatR;

namespace IbbDownloadService.NugetModule.Contracts.Events;

public class NugetChangedEvent: INotification
{
    public Guid Id { get; set; }
}


public class NugetUpdatedEvent
{
    public event Func<Task>? OnNugetUpdated;
    
    public void Notify()
    {
        OnNugetUpdated?.Invoke();
    }
}