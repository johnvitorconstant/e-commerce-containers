using ECC.WebApp.MVC.Models;
using ECC.WebApp.MVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IAuthenticationService = ECC.WebApp.MVC.Services.IAuthenticationService;

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
            await DoLogin(response);

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
            await DoLogin(response);



            return RedirectToAction("Index", "Home");


        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Index", "Home");
        }


        private async Task DoLogin(UserResponseSignIn response)
        {
            var token = GetFormatedToken(response.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", response.AccessToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent= true

        };


        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity), authProperties);
        }


        private static JwtSecurityToken GetFormatedToken(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }

    }
}