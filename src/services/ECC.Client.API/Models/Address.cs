using ECC.Core.DomainObjects;

namespace ECC.Client.API.Models;

public class Address : Entity
{
    public string? Place { get; private set; }
    public string? Number { get; private set; }
    public string? Complement { get; private set; }
    public string? District { get; private set; }
    public string? Cep { get; private set; }
    public string? City { get; private set; }
    public string? State { get; private set; }

    //EF Relation
    public Guid ClientId { get; private set; }
    public Client Client { get; protected set; }

    public Address(string? place, string? number, string? complement, string? district, string? cep, string? city, string? state)
    {
        Place = place;
        Number = number;
        Complement = complement;
        District = district;
        Cep = cep;
        City = city;
        State = state;
    }
}