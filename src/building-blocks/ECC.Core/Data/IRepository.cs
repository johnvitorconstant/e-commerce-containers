using ECC.Core.DomainObjects;

namespace ECC.Core.Data;

public interface IRepository<T> : IDisposable where T : IAggregateRoot
{
}