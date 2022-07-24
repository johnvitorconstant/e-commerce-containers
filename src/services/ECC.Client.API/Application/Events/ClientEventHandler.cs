using MediatR;

namespace ECC.Client.API.Application.Events;

public class ClientEventHandler : INotificationHandler<ClientRegisteredEvent>
{
    public Task Handle(ClientRegisteredEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}