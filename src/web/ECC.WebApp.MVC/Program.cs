using ECC.WebApp.MVC.Extensions;
using ECC.WebApp.MVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace NSE.WebApp.MVC
{
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

            builder.Services.Configure<AppSettings>(builder.Configuration);
            

            builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IUser, AspNetUser>();

            


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
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}