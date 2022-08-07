using ECC.Client.API.Application.Commands;
using ECC.Core.Mediator;
using ECC.Core.Messages.Integration;
using ECC.MessageBus;
using FluentValidation.Results;

namespace ECC.Client.API.Services;

public class RegisterClientIntegrationHandler : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public RegisterClientIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
       
        _bus.RespondAsync<RegisteredUserIntegrationEvent, ResponseMessage>(async request =>
            await RegisterClient(request));

        return Task.CompletedTask;
    }

    private async Task<ResponseMessage> RegisterClient(RegisteredUserIntegrationEvent message)
    {
        var clientCommand = new RegisterClientCommand(message.Id, message.Name, message.Email, message.Cpf);
        ValidationResult success;

        using (var scope = _serviceProvider.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
            success = await mediator.SendCommand(clientCommand);
        }

        return new ResponseMessage(success) ;
    }
}