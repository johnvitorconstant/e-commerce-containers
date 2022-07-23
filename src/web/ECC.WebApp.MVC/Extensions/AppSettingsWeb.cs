using static System.Net.WebRequestMethods;

namespace ECC.WebApp.MVC.Extensions;

public class AppSettingsWeb
{
    public string AuthenticationUrl = "https://localhost:7240";
    public string CatalogUrl { get; set; }
}