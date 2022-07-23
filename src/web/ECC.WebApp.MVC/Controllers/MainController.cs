using ECC.WebApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECC.WebApp.MVC.Controllers
{
    public class MainController : Controller
    {
        protected bool ResponseHasErrors(ResponseResult response)
        {
            if (response == null) return false;

            foreach(var message in response.Errors.Messages)
            {
                ModelState.AddModelError(string.Empty, message);
            }

            if(response != null && response.Errors.Messages.Any())
            {
                return true;
            }

            return false;
        }
    }
}
