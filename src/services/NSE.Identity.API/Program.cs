using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NSE.Identity.API.Data;
using NSE.Identity.API.Extensions;
using System.Text;

namespace NSE.Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureDataBase(builder);
            ConfigureIdentity(builder); 
            ConfigureServices(builder);
            ConfigureSwagger(builder);
            ConfigureApp(builder);


        }


        private static void ConfigureDataBase(WebApplicationBuilder builder)
        {
            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }

        private static void ConfigureIdentity(WebApplicationBuilder builder)
        {
            builder.Services.AddDefaultIdentity<IdentityUser>()
              .AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();


            IConfigurationSection appSettingsSection = builder.Configuration.GetSection("AppSettings");
            builder.Services.Configure<AppSettings>(appSettingsSection);



            AppSettings appSettings = appSettingsSection.Get<AppSettings>();
            byte[] key = Encoding.ASCII.GetBytes(appSettings.Secret);





            builder.Services.AddAuthentication(options =>
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

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
        }

        private static void ConfigureSwagger(WebApplicationBuilder builder)
        {
            var services = builder.Services;

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "NerdStore Enterprise Identity API",
                Description = "Identity API of NerdStoreEnterprise",
                Contact = new OpenApiContact()
                {
                    Name = "John Vitor Constant de Oliveira Lourenço",
                    Email = "johnvitorconstant@gmail.com",
                    Url = new Uri("https://github.com/johnvitorconstant")
                },
                License = new OpenApiLicense()
                {
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                },
            }));
        }

        private static void ConfigureApp(WebApplicationBuilder builder)
        {
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                builder.Configuration.AddUserSecrets<Program>();
            }
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }


    }
}