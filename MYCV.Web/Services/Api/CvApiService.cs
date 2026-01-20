using MYCV.Application.DTOs;
using System.Net.Http.Headers;

namespace MYCV.Web.Services.Api
{
    public class CvApiService : ICvApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CvApiService> _logger;

        public CvApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<CvApiService> logger)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ApiResponse<UserCvPersonalInfoDto>> GetUserCvAsync(Guid userId)
        {
            try
            {
                AddAuthHeader();

                var response = await _httpClient.GetAsync($"api/cv/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserCvPersonalInfoDto>>();
                    return result ?? ApiResponse<UserCvPersonalInfoDto>.ErrorResponse("Invalid response");
                }

                var error = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("GetUserCvAsync failed: {Error}", error);
                return ApiResponse<UserCvPersonalInfoDto>.ErrorResponse("Failed to fetch CV");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserCvAsync");
                return ApiResponse<UserCvPersonalInfoDto>.ErrorResponse("Network error");
            }
        }

        public async Task<ApiResponse<UserCvPersonalInfoDto>> SavePersonalInfoAsync(UserCvPersonalInfoDto dto)
        {
            try
            {
                AddAuthHeader();

                var response = await _httpClient.PostAsJsonAsync("api/cv/personal-info", dto);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserCvPersonalInfoDto>>();
                    return result ?? ApiResponse<UserCvPersonalInfoDto>.ErrorResponse("Invalid response");
                }

                var error = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("SavePersonalInfoAsync failed: {Error}", error);
                return ApiResponse<UserCvPersonalInfoDto>.ErrorResponse("Failed to save personal info");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SavePersonalInfoAsync");
                return ApiResponse<UserCvPersonalInfoDto>.ErrorResponse("Network error");
            }
        }

        private void AddAuthHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}