using ECC.WebApp.MVC.Models;

namespace ECC.WebApp.MVC.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<ProductViewModel>> FindAll();

        Task<ProductViewModel> FindById(Guid id);

    }
}
