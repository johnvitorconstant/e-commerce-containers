using ECC.WebApp.MVC.Extensions;
using ECC.WebApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace ECC.WebApp.MVC.Services;

class CatalogService :Service, ICatalogService
{
    private readonly HttpClient _httpClient;
    private readonly AppSettingsWeb _settings;
    private readonly string _catalogUrl;

    public CatalogService(HttpClient httpClient, IOptions<AppSettingsWeb> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;
        _catalogUrl = _settings.CatalogUrl;

    }
    public async Task<ProductViewModel> FindById(Guid id)
    {
        var response = await _httpClient.GetAsync($"{_catalogUrl}/api/catalog/products/{id}");
        HandleErrorResponses(response); //
        return await GetDeserializedDataResponse<ProductViewModel>(response);
    }
    public async Task<IEnumerable<ProductViewModel>> FindAll()
    {
        var url = _catalogUrl;
        var response = await _httpClient.GetAsync($"{_catalogUrl}/api/catalog/products/");
        HandleErrorResponses(response);
        return await GetDeserializedDataResponse<IEnumerable<ProductViewModel>>(response);
    }

}