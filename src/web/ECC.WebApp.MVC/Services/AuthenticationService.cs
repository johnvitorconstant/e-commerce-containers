using ECC.WebApp.MVC.Extensions;
using ECC.WebApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace ECC.WebApp.MVC.Services;

public class AuthenticationService : Service, IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly AppSettingsWeb _settings;
    private readonly string _authenticationUrl;

    public AuthenticationService(HttpClient httpClient, IOptions<AppSettingsWeb> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;
        _authenticationUrl = _settings.AuthenticationUrl;

    }

    public async Task<UserResponseSignIn> Login(UserSignIn user)
    {
        var content = GetSerializedDataContent(user);
        

        var response = await _httpClient.PostAsync($"{_authenticationUrl}/api/identity/signin", content);


        var result = await response.Content.ReadAsStringAsync();


        if (!HandleErrorResponses(response))
            return new UserResponseSignIn
            {
                ResponseResult = await GetDeserializedDataResponse<ResponseResult>(response)
            };


        return await GetDeserializedDataResponse<UserResponseSignIn>(response);
    }

    public async Task<UserResponseSignIn> Register(UserSignUp user)
    {
        var content = GetSerializedDataContent(user);

        var response = await _httpClient.PostAsync($"{_authenticationUrl}/api/identity/signup", content);
        var result = await response.Content.ReadAsStringAsync();


        if (!HandleErrorResponses(response))
            return new UserResponseSignIn
            {
                ResponseResult = await GetDeserializedDataResponse<ResponseResult>(response)
            };
        return await GetDeserializedDataResponse<UserResponseSignIn>(response);
    }
}