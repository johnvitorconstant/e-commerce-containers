using ECC.Core.DomainObjects;

namespace ECC.Catalog.API.Models
{
    public interface IProductRepository :IRepository<Product>
    {
        Task<IEnumerable<Product>> FindAll();
        Task<Product> FindById(Guid id);

        void Add(Product product);
        void Update(Product product);  

    }
}
