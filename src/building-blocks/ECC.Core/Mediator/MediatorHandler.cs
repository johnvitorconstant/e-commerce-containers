using ECC.Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace ECC.Core.Mediator;

public class MediatorHandler: IMediatorHandler

{
    private readonly IMediator _mediator;

    public MediatorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ValidationResult> SendCommand<T>(T command) where T : Command
    {
        return await _mediator.Send(command);
    }

    public async Task PublishEvent<T>(T publishEvent) where T : Event
    {
        await _mediator.Publish(publishEvent);
    }

 
}