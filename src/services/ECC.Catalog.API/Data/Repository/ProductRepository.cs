using ECC.Catalog.API.Models;

namespace ECC.Catalog.API.Data.Repository
{
    public class ProductRepository : IProductRepository
    {

        public Task<IEnumerable<Product>> FindAll()
        {
            throw new NotImplementedException();
        }

        public Task<Product> FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Add(Product product)
        {
            throw new NotImplementedException();
        }

        public void Update(Product product)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }
}
