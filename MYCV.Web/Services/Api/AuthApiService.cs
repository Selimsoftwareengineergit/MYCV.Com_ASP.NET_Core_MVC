namespace MYCV.Web.Services.Api
{
    public class AuthApiService:IAuthApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthApiService> _logger;
    }
}