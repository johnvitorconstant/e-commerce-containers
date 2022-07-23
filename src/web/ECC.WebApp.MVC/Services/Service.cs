using ECC.WebApp.MVC.Extensions;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Options;

namespace ECC.WebApp.MVC.Services
{
    public abstract class Service
    {
        protected bool HandleErrorResponses(HttpResponseMessage response)
        {
            switch ((int)response.StatusCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(response.StatusCode);

                case 400:
                    return false;

            }

            response.EnsureSuccessStatusCode();
            return true;
        }

        protected StringContent GetSerializedDataContent(object data)
        {
           return new StringContent(
              JsonSerializer.Serialize(data),
              Encoding.UTF8,
              "application/json"
              );
        }

        protected async Task<T> GetDeserializedDataResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var result = await responseMessage.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(result, options);
        }

    }
}
