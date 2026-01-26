using System.Net.Http.Headers;

namespace MYCV.Web.Services.Api
{
    public class JwtTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtTokenHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine("JWT added to request: " + token.Substring(0, 20) + "..."); 
            }
            else
            {
                Console.WriteLine("No AuthToken cookie found!");
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}