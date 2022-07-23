using ECC.WebApp.MVC.Extensions;
using ECC.WebApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace ECC.WebApp.MVC.Services;

public class AuthenticationService : Service, IAuthenticationService
{
    private readonly HttpClient _httpClient;

    public AuthenticationService(HttpClient httpClient, IOptions<AppSettings> settings)
    {
        httpClient.BaseAddress = new Uri(settings.Value.AuthenticationUrl);
        _httpClient = httpClient;
    }

    public async Task<UserResponseSignIn> Login(UserSignIn user)
    {
        var content = GetSerializedDataContent(user);

        var response = await _httpClient.PostAsync("/api/identity/signin", content);


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

        var response = await _httpClient.PostAsync("/api/identity/signup", content);
        var result = await response.Content.ReadAsStringAsync();


        if (!HandleErrorResponses(response))
            return new UserResponseSignIn
            {
                ResponseResult = await GetDeserializedDataResponse<ResponseResult>(response)
            };
        return await GetDeserializedDataResponse<UserResponseSignIn>(response);
    }
}