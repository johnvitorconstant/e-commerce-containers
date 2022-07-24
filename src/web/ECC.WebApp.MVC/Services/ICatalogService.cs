using ECC.WebApp.MVC.Models;
using Refit;

namespace ECC.WebApp.MVC.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<ProductViewModel>> FindAll();

        Task<ProductViewModel> FindById(Guid id);

    }

    public interface ICatalogServiceRefit
    {
        [Get("/api/catalog/products/")]
        Task<IEnumerable<ProductViewModel>> FindAll();

        [Get("/api/catalog/products/{id}")]
        Task<ProductViewModel> FindById(Guid id);
    }
}
