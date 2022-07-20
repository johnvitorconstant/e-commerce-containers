using ECC.WebApp.MVC.Models;
using ECC.WebApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECC.WebApp.MVC.Controllers
{
    public class IdentityController : Controller
    {

        private readonly IAuthenticationService _service;

        public IdentityController(IAuthenticationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("register")]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserSignUp user)
        {
            if (!ModelState.IsValid) return View(user);

            //API Register
            var response = await _service.Register(user);

            // User login
            return RedirectToAction("Index", "Home");


        }

        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login()
        {
            return View();
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserSignIn user)
        {
            if (!ModelState.IsValid) return View(user);

            //API login
            var response = await _service.Login(user);




            return RedirectToAction("Index", "Home");


        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Index", "Home");
        }


    }
}