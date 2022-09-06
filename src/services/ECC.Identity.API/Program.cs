using ECC.Identity.API.Data;
using ECC.MessageBus;
using ECC.WebAPI.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ECC.Identity.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddMessageBus("host=localhost:5672;publisherConfirms=true;timeout=10");

        ConfigureDataBase(builder);
        ConfigureIdentity(builder);
        ConfigureServices(builder);
        builder.Services.ConfigureJwt(builder.Configuration);
        ConfigureSwagger(builder);
        ConfigureApp(builder);
    }

  

    private static void ConfigureDataBase(WebApplicationBuilder builder)
    {
        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(connectionString); });
    }

    private static void ConfigureIdentity(WebApplicationBuilder builder)
    {
        builder.Services.AddDefaultIdentity<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
       
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
            Title = "ECommerceOnContainers Identity API",
            Description = "Identity API of ECommerceOnContainers",
            Contact = new OpenApiContact
            {
                Name = "John Vitor Constant de Oliveira Lourenço",
                Email = "johnvitorconstant@gmail.com",
                Url = new Uri("https://github.com/johnvitorconstant")
            },
            License = new OpenApiLicense
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
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