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
        public async Task<ApiResponse<UserPersonalDetailDto>> GetUserPersonalDetailAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching CV for user {UserId}", userId);

                // Convert int to string for API URL
                var response = await _httpClient.GetAsync($"api/cv/{userId}");
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("GetUserCvAsync failed for user {UserId}: {Error}", userId, error);
                    return ApiResponse<UserPersonalDetailDto>.ErrorResponse(error);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserPersonalDetailDto>>();
                if (result == null)
                {
                    _logger.LogWarning("GetUserCvAsync returned null for user {UserId}", userId);
                    return ApiResponse<UserPersonalDetailDto>.ErrorResponse("Invalid response from API");
                }

                _logger.LogInformation("Successfully fetched CV for user {UserId}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetUserCvAsync for user {UserId}", userId);
                return ApiResponse<UserPersonalDetailDto>.ErrorResponse("Network or API error");
            }
        }

        /// <summary>
        /// Save personal info for a user's CV
        /// </summary>
        public async Task<ApiResponse<UserPersonalDetailDto>> SaveUserPersonalDetailAsync(UserPersonalDetailDto dto)
        {
            try
            {
                using var content = new MultipartFormDataContent();

                // =========================
                // REQUIRED
                // =========================
                content.Add(new StringContent(dto.UserId.ToString()), "UserId");

                // Optional (for update)
                content.Add(new StringContent(dto.Id.ToString()), "Id");

                // =========================
                // Personal Info
                // =========================
                content.Add(new StringContent(dto.FullName ?? ""), "FullName");
                content.Add(new StringContent(dto.ProfessionalTitle ?? ""), "ProfessionalTitle");
                content.Add(new StringContent(dto.DateOfBirth.ToString("yyyy-MM-dd") ?? ""), "DateOfBirth");
                content.Add(new StringContent(dto.Gender ?? ""), "Gender");

                // =========================
                // Contact
                // =========================
                content.Add(new StringContent(dto.Email ?? ""), "Email");
                content.Add(new StringContent(dto.PhoneNumber ?? ""), "PhoneNumber");
                content.Add(new StringContent(dto.Country ?? ""), "Country");
                content.Add(new StringContent(dto.City ?? ""), "City");
                content.Add(new StringContent(dto.Address ?? ""), "Address");

                // =========================
                // Professional
                // =========================
                content.Add(new StringContent(dto.Summary ?? ""), "Summary");
                content.Add(new StringContent(dto.Objective ?? ""), "Objective");

                // =========================
                // Social
                // =========================
                content.Add(new StringContent(dto.LinkedIn ?? ""), "LinkedIn");
                content.Add(new StringContent(dto.GitHub ?? ""), "GitHub");
                content.Add(new StringContent(dto.Portfolio ?? ""), "Portfolio");
                content.Add(new StringContent(dto.Website ?? ""), "Website");
                content.Add(new StringContent(dto.LinkedInHeadline ?? ""), "LinkedInHeadline");

                // =========================
                // Profile Picture
                // =========================
                if (dto.ProfilePicture != null)
                {
                    var streamContent = new StreamContent(dto.ProfilePicture.OpenReadStream());
                    streamContent.Headers.ContentType =
                        new MediaTypeHeaderValue(dto.ProfilePicture.ContentType);

                    content.Add(streamContent, "ProfilePicture", dto.ProfilePicture.FileName);
                }

                // =========================
                // Send Request
                // =========================
                var response = await _httpClient.PostAsync("api/cv/personal-detail", content);

                if (!response.IsSuccessStatusCode)
                {
                    var respContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API returned error. Status: {StatusCode}, Content: {Content}",
                        response.StatusCode, respContent);

                    return ApiResponse<UserPersonalDetailDto>
                        .ErrorResponse("API returned error: " + respContent);
                }

                var result = await response
                    .Content
                    .ReadFromJsonAsync<ApiResponse<UserPersonalDetailDto>>();

                return result ?? ApiResponse<UserPersonalDetailDto>
                    .ErrorResponse("Invalid response from API");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when calling API SavePersonalDetail");

                return ApiResponse<UserPersonalDetailDto>
                    .ErrorResponse("Exception: " + ex.Message);
            }
        }


        /// <summary>
        /// Get User Education by userId 
        /// </summary>
        public async Task<ApiResponse<List<UserEducationDto>>> GetUserEducationAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching education records for user {UserId}", userId);

                var response = await _httpClient.GetAsync($"api/cv/{userId}/education");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("GetUserEducationAsync failed for user {UserId}: {Error}", userId, error);
                    return ApiResponse<List<UserEducationDto>>.ErrorResponse(error);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<UserEducationDto>>>();

                if (result == null)
                {
                    _logger.LogWarning("GetUserEducationAsync returned null for user {UserId}", userId);
                    return ApiResponse<List<UserEducationDto>>.ErrorResponse("Invalid response from API");
                }

                _logger.LogInformation("Successfully fetched education records for user {UserId}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetUserEducationAsync for user {UserId}", userId);
                return ApiResponse<List<UserEducationDto>>.ErrorResponse("Network or API error");
            }
        }

        /// <summary>
        /// Save multiple user education records
        /// </summary>
        public async Task<ApiResponse<List<UserEducationDto>>> SaveEducationAsync(List<UserEducationDto> educationList)
        {
            try
            {
                _logger.LogInformation("Saving education records. Count: {Count}", educationList.Count);

                var response = await _httpClient.PostAsJsonAsync("api/cv/education", educationList);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("SaveEducationAsync failed: {Error}", error);
                    return ApiResponse<List<UserEducationDto>>.ErrorResponse(error);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<UserEducationDto>>>();

                if (result == null)
                {
                    _logger.LogWarning("SaveEducationAsync returned null response");
                    return ApiResponse<List<UserEducationDto>>.ErrorResponse("Invalid response from API");
                }

                _logger.LogInformation("Education records saved successfully");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in SaveEducationAsync");
                return ApiResponse<List<UserEducationDto>>.ErrorResponse("Network or API error");
            }
        }

        /// <summary>
        /// Get User Experiences by userId
        /// </summary>
        public async Task<ApiResponse<List<UserExperienceDto>>> GetUserExperiencesAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching work experience records for user {UserId}", userId);

                var response = await _httpClient.GetAsync($"api/cv/{userId}/experiences");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("GetUserExperiencesAsync failed for user {UserId}: {Error}", userId, error);
                    return ApiResponse<List<UserExperienceDto>>.ErrorResponse(error);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<UserExperienceDto>>>();

                if (result == null)
                {
                    _logger.LogWarning("GetUserExperiencesAsync returned null for user {UserId}", userId);
                    return ApiResponse<List<UserExperienceDto>>.ErrorResponse("Invalid response from API");
                }

                _logger.LogInformation("Successfully fetched work experience records for user {UserId}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetUserExperiencesAsync for user {UserId}", userId);
                return ApiResponse<List<UserExperienceDto>>.ErrorResponse("Network or API error");
            }
        }

        /// <summary>
        /// Save user work/experience records
        /// </summary>
        public async Task<ApiResponse<List<UserExperienceDto>>> SaveUserExperiencesAsync(List<UserExperienceDto> experienceList)
        {
            try
            {
                _logger.LogInformation("Saving user experiences. Count: {Count}", experienceList.Count);

                // POST to API endpoint for saving experiences
                var response = await _httpClient.PostAsJsonAsync("api/cv/experience", experienceList);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("SaveUserExperiencesAsync failed: {Error}", error);
                    return ApiResponse<List<UserExperienceDto>>.ErrorResponse(error);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<UserExperienceDto>>>();

                if (result == null)
                {
                    _logger.LogWarning("SaveUserExperiencesAsync returned null response");
                    return ApiResponse<List<UserExperienceDto>>.ErrorResponse("Invalid response from API");
                }

                _logger.LogInformation("User experiences saved successfully");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in SaveUserExperiencesAsync");
                return ApiResponse<List<UserExperienceDto>>.ErrorResponse("Network or API error");
            }
        }
    }
}