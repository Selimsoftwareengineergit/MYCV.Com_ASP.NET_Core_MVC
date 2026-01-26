using MYCV.Application.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MYCV.Web.Services.Api
{
    public class CvApiService : ICvApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CvApiService> _logger;

        public CvApiService(HttpClient httpClient, ILogger<CvApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Get CV for a user by userId
        /// </summary>
        public async Task<ApiResponse<UserCvPersonalInfoDto>> GetUserCvAsync(Guid userId)
        {
            try
            {
                _logger.LogInformation("Fetching CV for user {UserId}", userId);

                var response = await _httpClient.GetAsync($"api/cv/{userId}");
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("GetUserCvAsync failed for user {UserId}: {Error}", userId, error);
                    return ApiResponse<UserCvPersonalInfoDto>.ErrorResponse(error);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserCvPersonalInfoDto>>();
                if (result == null)
                {
                    _logger.LogWarning("GetUserCvAsync returned null for user {UserId}", userId);
                    return ApiResponse<UserCvPersonalInfoDto>.ErrorResponse("Invalid response from API");
                }

                _logger.LogInformation("Successfully fetched CV for user {UserId}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetUserCvAsync for user {UserId}", userId);
                return ApiResponse<UserCvPersonalInfoDto>.ErrorResponse("Network or API error");
            }
        }

        /// <summary>
        /// Save personal info for a user's CV
        /// </summary>
        public async Task<ApiResponse<UserCvPersonalInfoDto>> SavePersonalInfoAsync(UserCvPersonalInfoDto dto)
        {
            try
            {
                // This is the actual API call
                var response = await _httpClient.PostAsJsonAsync("api/cv/personal-info", dto);

                // Log status for debugging
                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API returned error. Status: {StatusCode}, Content: {Content}",
                        response.StatusCode, content);
                    return ApiResponse<UserCvPersonalInfoDto>.ErrorResponse("API returned error: " + content);
                }

                // Deserialize response
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserCvPersonalInfoDto>>();
                return result ?? ApiResponse<UserCvPersonalInfoDto>.ErrorResponse("Invalid response from API");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when calling API SavePersonalInfo");
                return ApiResponse<UserCvPersonalInfoDto>.ErrorResponse("Exception: " + ex.Message);
            }
        }
    }
}