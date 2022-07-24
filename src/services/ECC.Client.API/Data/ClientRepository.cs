using ECC.Client.API.Models;
using ECC.Core.Data;
using ECC.Core.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace ECC.Client.API.Data;

public class ClientRepository : IClientRepository
{

    private readonly ClientsContext _context;

    public ClientRepository(ClientsContext context)
    {
        _context = context;
    }
    public IUnityOfWork UnityOfWork => _context;
    public async Task<IEnumerable<Models.Client>> FindAll()
    {
        return await _context.Clients.AsNoTracking().ToListAsync();
    }

    public async Task<Models.Client?> FindByCpf(string cpf)
    {
        return await _context.Clients.FirstOrDefaultAsync(c => c.Cpf.Number == cpf);
    }

    public void Add(Models.Client client)
    {
        _context.Clients.Add(client);
    }




    public void Dispose()
    {
    }
}