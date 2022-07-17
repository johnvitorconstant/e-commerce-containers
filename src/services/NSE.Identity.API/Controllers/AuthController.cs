using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSE.Identity.API.Models;

namespace NSE.Identity.API.Controllers
{
    [ApiController]
    [Route("api/identity")]
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(UserSignUp userSignUp)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = new IdentityUser
            {
                UserName = userSignUp.Email,
                Email = userSignUp.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, userSignUp.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok();
            }

            return BadRequest(result.Errors);


        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(UserSignIn userSignIn)
        {
            if (!ModelState.IsValid) return BadRequest();
            var result = await _signInManager.PasswordSignInAsync(userSignIn.Email, userSignIn.Password, false, true);
            
            if (result.Succeeded) return Ok();
            return BadRequest();

        }



    }
}
