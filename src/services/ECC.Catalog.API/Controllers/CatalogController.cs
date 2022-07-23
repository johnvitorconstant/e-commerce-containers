using ECC.Catalog.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECC.Catalog.API.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductRepository _productRepository;

        public CatalogController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        [HttpGet("catalog/products")]
        public async Task<IEnumerable<Product>> Index()
        {
            return await _productRepository.FindAll();
        }

        [HttpGet("catalog/products/{id}")]
        public async Task<Product> ProductDetail(Guid id)
        {
            return await _productRepository.FindById(id);
        }

    }
}
