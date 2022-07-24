using ECC.Catalog.API.Models;
using ECC.WebAPI.Core.Controllers;
using ECC.WebAPI.Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECC.Catalog.API.Controllers;


[Authorize]
public class CatalogController : MainController
{
    private readonly IProductRepository _productRepository;

    public CatalogController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [AllowAnonymous]
    [HttpGet("api/catalog/products")]
    public async Task<IEnumerable<Product>> Index()
    {
        return await _productRepository.FindAll();
    }

    [ClaimsAuthorize("Catalog", "Read")]
    [HttpGet("api/catalog/products/{id}")]
    public async Task<Product> ProductDetail(Guid id)
    {
        return await _productRepository.FindById(id);
    }
}