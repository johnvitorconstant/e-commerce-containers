using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;
using ECC.MessageBus;
using ECC.ShoppingCart.API.Data;
using ECC.WebAPI.Core.Identity;
using ECC.WebAPI.Core.User;

namespace ECC.ShoppingCart.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        ConfigureDataBase(builder);
        ConfigureRabbitMq(builder);
        ConfigureDependencyInjection(builder);
        ConfigureServices(builder);
        builder.Services.ConfigureJwt(builder.Configuration);
        ConfigureSwagger(builder);
        ConfigureApp(builder);


    }
    private static void ConfigureDataBase(WebApplicationBuilder builder)
    {
        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ShoppingCartContext>(options => { options.UseSqlServer(connectionString); });
    }
    private static void ConfigureRabbitMq(WebApplicationBuilder builder)
    {
        var rabbitMqConnString = builder.Configuration.GetConnectionString("RabbitMqConnection");
        builder.Services.AddMessageBus(rabbitMqConnString);
    }

    private static void ConfigureDependencyInjection(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddScoped<IAspNetUser, AspNetUser>();
        builder.Services.AddScoped<ShoppingCartContext>();
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
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ECommerceOnContainers Client API",
                Description = "Client API of ECommerceOnContainers",
                Contact = new OpenApiContact
                {
                    Name = "John Vitor Constant de Oliveira Lourenço",
                    Email = "johnvitorconstant@gmail.com",
                    Url = new Uri("https://github.com/johnvitorconstant")
                },
                License = new OpenApiLicense
                { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Bearer {your token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });

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