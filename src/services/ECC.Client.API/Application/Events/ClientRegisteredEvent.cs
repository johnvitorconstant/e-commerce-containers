using ECC.Core.Messages;

namespace ECC.Client.API.Application.Events;

public class ClientRegisteredEvent : Event
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Cpf { get; private set; }

    public ClientRegisteredEvent(Guid id, string name, string email, string cpf)
    {
        AggregateId = id;
        Id = id;
        Name = name;
        Email = email;
        Cpf = cpf;
    }
}