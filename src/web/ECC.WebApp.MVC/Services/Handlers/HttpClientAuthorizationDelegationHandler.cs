using System.Net.Http.Headers;
using ECC.WebAPI.Core.User;

namespace ECC.WebApp.MVC.Services.Handlers
{
    public class HttpClientAuthorizationDelegationHandler:DelegatingHandler
    {
        private readonly IAspNetUser _user;


        public HttpClientAuthorizationDelegationHandler(IAspNetUser user)
        {
            _user = user;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var athorizationHeader = _user.GetHttpContext().Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(athorizationHeader))
            {
                request.Headers.Add("Authorization", new List<string>(){athorizationHeader});
            }

            var token = _user.GetUserToken();

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }


            return base.SendAsync(request, cancellationToken);
        }
    }
}
