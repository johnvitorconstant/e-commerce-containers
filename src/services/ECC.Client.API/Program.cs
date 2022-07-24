using ECC.Client.API.Application.Commands;
using ECC.Client.API.Data;
using ECC.Client.API.Models;
using ECC.Core.Mediator;
using ECC.WebAPI.Core.Identity;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ECC.Client.API;

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
        builder.Services.AddMediatR(typeof(Program));

        builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
        builder.Services.AddScoped<IRequestHandler<RegisterClientCommand, ValidationResult>, ClientCommandHandler>();
        builder.Services.AddScoped<IClientRepository, ClientRepository>();
        builder.Services.AddScoped<ClientsContext>();

    }
    private static void ConfigureDataBase(WebApplicationBuilder builder)
    {
        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ClientsContext>(options => { options.UseSqlServer(connectionString); });
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