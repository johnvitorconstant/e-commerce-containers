using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECC.Core.DomainObjects;

namespace ECC.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {

    }
}
