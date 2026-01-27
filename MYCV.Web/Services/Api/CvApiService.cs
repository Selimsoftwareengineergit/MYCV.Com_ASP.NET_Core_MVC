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
                // Prepare multipart/form-data content
                using var content = new MultipartFormDataContent();

                // Add text fields
                content.Add(new StringContent(dto.FullName ?? ""), "FullName");
                content.Add(new StringContent(dto.ProfessionalTitle ?? ""), "ProfessionalTitle");
                content.Add(new StringContent(dto.DateOfBirth?.ToString("yyyy-MM-dd") ?? ""), "DateOfBirth");
                content.Add(new StringContent(dto.Gender ?? ""), "Gender");
                content.Add(new StringContent(dto.Email ?? ""), "Email");
                content.Add(new StringContent(dto.PhoneNumber ?? ""), "PhoneNumber");
                content.Add(new StringContent(dto.Country ?? ""), "Country");
                content.Add(new StringContent(dto.City ?? ""), "City");
                content.Add(new StringContent(dto.Address ?? ""), "Address");
                content.Add(new StringContent(dto.LinkedIn ?? ""), "LinkedIn");
                content.Add(new StringContent(dto.GitHub ?? ""), "GitHub");
                content.Add(new StringContent(dto.Portfolio ?? ""), "Portfolio");
                content.Add(new StringContent(dto.Website ?? ""), "Website");
                content.Add(new StringContent(dto.LinkedInHeadline ?? ""), "LinkedInHeadline");
                content.Add(new StringContent(dto.Summary ?? ""), "Summary");

                // Add Profile Picture if exists
                if (dto.ProfilePicture != null)
                {
                    var streamContent = new StreamContent(dto.ProfilePicture.OpenReadStream());
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(dto.ProfilePicture.ContentType);
                    content.Add(streamContent, "ProfilePicture", dto.ProfilePicture.FileName);
                }

                // Send request
                var response = await _httpClient.PostAsync("api/cv/personal-info", content);

                if (!response.IsSuccessStatusCode)
                {
                    var respContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API returned error. Status: {StatusCode}, Content: {Content}",
                        response.StatusCode, respContent);
                    return ApiResponse<UserCvPersonalInfoDto>.ErrorResponse("API returned error: " + respContent);
                }

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