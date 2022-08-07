using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECC.Core.Messages.Integration;
using ECC.Identity.API.Models;
using ECC.MessageBus;
using ECC.WebAPI.Core.Controllers;
using ECC.WebAPI.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ECC.Identity.API.Controllers;

[Route("api/identity")]
public class AuthController : MainController
{
    private readonly AppSettings _appSettings;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    private readonly IMessageBus _bus;

    public AuthController(SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IOptions<AppSettings> appSettings,
        IMessageBus bus)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _appSettings = appSettings.Value;
        _bus = bus;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup(UserSignUp userSignUp)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);


        var user = new IdentityUser
        {
            UserName = userSignUp.Email,
            Email = userSignUp.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, userSignUp.Password);

        if (result.Succeeded)
        {
            //Lançar evento de integração
           var clientResult = await RegisterClient(userSignUp);

           if (!clientResult.ValidationResult.IsValid)
           {
               await _userManager.DeleteAsync(user);
               return CustomResponse(clientResult.ValidationResult);
           }

            return CustomResponse(await GenerateJwt(userSignUp.Email));
        }

        foreach (var error in result.Errors) AddProcessError(error.Description);

        return CustomResponse();
    }


    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(UserSignIn userSignIn)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);


        var result = await _signInManager.PasswordSignInAsync(userSignIn.Email, userSignIn.Password, false, true);

        if (result.Succeeded) return CustomResponse(await GenerateJwt(userSignIn.Email));

        if (result.IsLockedOut)
        {
            AddProcessError("User temporarily blocked for multiple login attempts");
            return CustomResponse();
        }


        AddProcessError("User or password wrong");
        return CustomResponse();
    }

    private async Task<ResponseMessage> RegisterClient(UserSignUp registerUser)
    {
        var user = await _userManager.FindByEmailAsync(registerUser.Email);

        var registeredUser = new RegisteredUserIntegrationEvent(Guid.Parse(user.Id), registerUser.Name,
            registerUser.Email, registerUser.Cpf);

        try
        {
            return await _bus.RequestAsync<RegisteredUserIntegrationEvent, ResponseMessage>(registeredUser);
        }
        catch
        {
            await _userManager.DeleteAsync(user);
            throw;
        }
        
    }


    private async Task<UserResponseLogin> GenerateJwt(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var claims = await _userManager.GetClaimsAsync(user);

        var identityClaims = await GetUserClaims(claims, user);
        var encodedToken = EncodeToken(identityClaims);

        return GetTokenResponse(encodedToken, user, claims);
    }

    private async Task<ClaimsIdentity> GetUserClaims(ICollection<Claim> claims, IdentityUser user)
    {
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.Now).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.Now).ToString(),
            ClaimValueTypes.Integer64));


        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var userRole in userRoles) claims.Add(new Claim("role", userRole));
        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);
        return identityClaims;
    }

    private string EncodeToken(ClaimsIdentity identityClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

        var token = tokenHandler.CreateToken(
            new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.ValidAt,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.HoursToExpire),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            });

        return tokenHandler.WriteToken(token);
    }

    private UserResponseLogin GetTokenResponse(string encodedToken, IdentityUser user, IEnumerable<Claim> claims)
    {
        return new UserResponseLogin
        {
            AccessToken = encodedToken,
            ExpiresIn = TimeSpan.FromHours(_appSettings.HoursToExpire).TotalSeconds,
            UserToken = new UserToken
            {
                Id = user.Id,
                Email = user.Email,
                UserClaims = claims.Select(c => new UserClaim {Type = c.Type, Value = c.Value})
            }
        };
    }

    private static long ToUnixEpochDate(DateTime date)
    {
        return (long) Math
            .Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}