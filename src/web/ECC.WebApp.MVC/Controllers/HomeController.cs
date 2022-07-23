using ECC.WebApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECC.WebApp.MVC.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Route("error/{id:length(3,3)}")]
    public IActionResult Error(int id)
    {
        var modelError = new ErrorViewModel();

        if (id == 500)
        {
            modelError.ErrorCode = id;
            modelError.Message = "Error 500 message";
            modelError.Title = "Error 500 title";
        }
        else if (id == 404)
        {
            modelError.ErrorCode = id;
            modelError.Message = "This page does not exist";
            modelError.Title = "Error 404 title";
        }
        else if (id == 403)
        {
            modelError.ErrorCode = id;
            modelError.Message = "You do not have authorization";
            modelError.Title = "Error 403 title";
        }
        else
        {
            return StatusCode(404);
        }

        return View("Error", modelError);
    }
}