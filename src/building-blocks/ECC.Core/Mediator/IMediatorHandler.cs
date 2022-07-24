using ECC.Core.Messages;
using FluentValidation.Results;

namespace ECC.Core.Mediator;

public interface IMediatorHandler
{

    Task PublishEvent<T>(T publishEvent) where T : Event;
    Task<ValidationResult> SendCommand<T>(T command) where T : Command;
   

    }