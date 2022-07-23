using ECC.Catalog.API.Data;
using ECC.Catalog.API.Data.Repository;
using ECC.Catalog.API.Models;
using ECC.WebAPI.Core.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ECC.Catalog.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        ConfigureDataBase(builder);
        
        ConfigureDependencyInjection(builder);
        ConfigureServices(builder);

        builder.Services.ConfigureJwt(builder.Configuration);
        ConfigureSwagger(builder);

        ConfigureApp(builder);
    }


    private static void ConfigureDependencyInjection(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<CatalogContext>();
    }


    private static void ConfigureDataBase(WebApplicationBuilder builder)
    {
        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<CatalogContext>(options => { options.UseSqlServer(connectionString); });
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Total", builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    }

    private static void ConfigureSwagger(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "ECommerceOnContainers Catalog API",
            Description = "Catalog API of ECommerceOnContainers",
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
        app.UseCors("Total");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.Run();
    }
}