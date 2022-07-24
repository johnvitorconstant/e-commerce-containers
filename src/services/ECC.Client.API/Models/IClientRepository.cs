using ECC.Core.Data;

namespace ECC.Client.API.Models;

public interface IClientRepository : IRepository<Client>
{
    void Add(Client client);
    Task<IEnumerable<Client>> FindAll();
    Task<Client> FindByCpf(string cpf);


}