using ECC.WebApp.MVC.Models;
using System.Text;
using System.Text.Json;

namespace ECC.WebApp.MVC.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly HttpClient _httpClient;

        public AuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserResponseSignIn> Login(UserSignIn user)
        {

            var content = new StringContent(
                JsonSerializer.Serialize(user),
                Encoding.UTF8,
                "application/json"
                );

            var response = await _httpClient.PostAsync("https://localhost:44379/api/identity/signin", content);
            var result = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };


            var deserializedResult = JsonSerializer.Deserialize<UserResponseSignIn>(result, options);


            return deserializedResult;
        }

        public async Task<UserResponseSignIn> Register(UserSignUp user)
        {
            var content = new StringContent(
             JsonSerializer.Serialize(user),
             Encoding.UTF8,
             "application/json"
             );

            var response = await _httpClient.PostAsync("https://localhost:44379/api/identity/signup", content);
            var result = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };


            var deserializedResult = JsonSerializer.Deserialize<UserResponseSignIn>(result, options);

            return deserializedResult;

        }
    }
}


