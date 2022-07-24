using ECC.WebApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECC.WebApp.MVC.Controllers
{
    public class CatalogController : Controller

    {
       private readonly  ICatalogService _catalogService;
     //  private readonly ICatalogServiceRefit _catalogService;


        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        [Route("")]
        [Route("catalog")]
        public async Task<IActionResult> Index()
        {
            var products = await _catalogService.FindAll();
            return View(products);
        }
        [HttpGet]
        [Route("product-detail/{id}")]
        public async Task<IActionResult> ProductDetail(Guid id)
        {
            var product = await _catalogService.FindById(id);
            return View(product);
        }


    }
}
