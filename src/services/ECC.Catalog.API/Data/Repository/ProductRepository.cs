using ECC.Catalog.API.Models;
using ECC.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace ECC.Catalog.API.Data.Repository
{
    public class ProductRepository : IProductRepository
    {

        private readonly CatalogContext _context;

        public ProductRepository(CatalogContext context)
        {
            _context = context;
        }

        public IUnityOfWork UnityOfWork => _context;

        public async Task<IEnumerable<Product>> FindAll()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Product> FindById(Guid id)
        {
           return await _context.Products.FindAsync(id);
        }

        public void Add(Product product)
        {
            _context.Products.AddAsync(product);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

    }
}
