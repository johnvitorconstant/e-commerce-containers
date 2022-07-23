using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ECC.WebAPI.Core.Identity;

public static class JwtConfig
{
    private static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
    {
            
        IConfigurationSection appSettingsSection = configuration.GetSection("AppSettings");
        services.Configure<AppSettings>(appSettingsSection);



        AppSettings appSettings = appSettingsSection.Get<AppSettings>();
        byte[] key = Encoding.ASCII.GetBytes(appSettings.Secret);





        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearerOptions =>
        {
            bearerOptions.RequireHttpsMetadata = true;
            bearerOptions.SaveToken = true;
            bearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),

                ValidateIssuer = true,
                ValidIssuer = appSettings.Issuer,

                ValidateAudience = true,
                ValidAudience = appSettings.ValidAt,

            };
        });


    }
}