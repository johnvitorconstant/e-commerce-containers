using Microsoft.AspNetCore.Mvc;

namespace ECC.WebApp.MVC.Extensions;

public class SummaryViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}