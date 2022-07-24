using ECC.WebAPI.Core.Identity;
using ECC.WebApp.MVC.Extensions;
using ECC.WebApp.MVC.Services;
using ECC.WebApp.MVC.Services.Handlers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Polly;

namespace ECC.WebApp.MVC;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/access-denied";
            });
        // Add services to the container.

        builder.Services.AddControllersWithViews();

        builder.Services.Configure<AppSettingsWeb>(builder.Configuration);


        // builder.Services.ConfigureJwt(builder.Configuration);

        ConfigureDepedencyInjection(builder);

        var app = builder.Build();


        // Configure the HTTP request pipeline.
        //   if (app.Environment.IsDevelopment())
        //   {
        app.UseExceptionHandler("/error/500");
        app.UseStatusCodePagesWithRedirects("/error/{0}");

        // The defaulst HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
        //   }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<ExceptionMiddleware>();

        app.MapControllerRoute(
            "default",
            "{controller=Catalog}/{action=Index}/{id?}");

        app.Run();
    }

    private static void ConfigureDepedencyInjection(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<HttpClientAuthorizationDelegationHandler>();
        builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>();

        builder.Services.AddHttpClient<ICatalogService, CatalogService>()
             .AddHttpMessageHandler<HttpClientAuthorizationDelegationHandler>()
             .AddTransientHttpErrorPolicy(p => 
                 p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));

        //    builder.Services.AddHttpClient("Refit", options =>
        //        {
        //            options.BaseAddress = new Uri(builder.Configuration.GetSection("CatalogUrl").Value);
        //        })
        //        .AddHttpMessageHandler<HttpClientAuthorizationDelegationHandler>()
        //        .AddTypedClient(Refit.RestService.For<ICatalogServiceRefit>);


        builder.Services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddScoped<IUser, AspNetUser>();
    }


}