using ECC.WebApp.MVC.Models;

namespace ECC.WebApp.MVC.Services
{
    public interface IAuthenticationService
    {
        Task<UserResponseSignIn> Login(UserSignIn user);

        Task<UserResponseSignIn> Register(UserSignUp user);
    }
}


