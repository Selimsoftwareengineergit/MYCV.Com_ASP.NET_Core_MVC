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
        /// Get User personal detail by userId 
        /// </summary>
        public async Task<ApiResponse<UserPersonalDetailDto>> GetUserPersonalDetailAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching personal detail for user {UserId}", userId);

                var response = await _httpClient.GetAsync($"api/cv/{userId}/personalDetail");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("GetUserPersonalDetailAsync failed for user {UserId}: {Error}", userId, error);

                    return ApiResponse<UserPersonalDetailDto>.ErrorResponse(error);
                }

                var result = await response.Content
                    .ReadFromJsonAsync<ApiResponse<UserPersonalDetailDto>>();

                if (result == null)
                {
                    _logger.LogWarning("GetUserPersonalDetailAsync returned null for user {UserId}", userId);
                    return ApiResponse<UserPersonalDetailDto>.ErrorResponse("Invalid response from API");
                }

                _logger.LogInformation("Successfully fetched personal detail for user {UserId}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetUserPersonalDetailAsync for user {UserId}", userId);
                return ApiResponse<UserPersonalDetailDto>.ErrorResponse("Network or API error");
            }
        }

        /// <summary>
        /// Save personal detail for a user
        /// </summary>
        public async Task<ApiResponse<UserPersonalDetailDto>> SaveUserPersonalDetailAsync(UserPersonalDetailDto dto)
        {
            try
            {
                _logger.LogInformation("Saving personal detail for user {UserId}", dto.UserId);

                using var content = new MultipartFormDataContent
                {
                    { new StringContent(dto.UserId.ToString()), "UserId" },
                    { new StringContent(dto.Id.ToString()), "Id" },
                    { new StringContent(dto.FullName ?? string.Empty), "FullName" },
                    { new StringContent(dto.ProfessionalTitle ?? string.Empty), "ProfessionalTitle" },
                    { new StringContent(dto.DateOfBirth.ToString("yyyy-MM-dd")), "DateOfBirth" },
                    { new StringContent(dto.Gender ?? string.Empty), "Gender" },
                    { new StringContent(dto.Email ?? string.Empty), "Email" },
                    { new StringContent(dto.PhoneNumber ?? string.Empty), "PhoneNumber" },
                    { new StringContent(dto.Country ?? string.Empty), "Country" },
                    { new StringContent(dto.City ?? string.Empty), "City" },
                    { new StringContent(dto.Address ?? string.Empty), "Address" },
                    { new StringContent(dto.Summary ?? string.Empty), "Summary" },
                    { new StringContent(dto.Objective ?? string.Empty), "Objective" },
                    { new StringContent(dto.LinkedIn ?? string.Empty), "LinkedIn" },
                    { new StringContent(dto.GitHub ?? string.Empty), "GitHub" },
                    { new StringContent(dto.Portfolio ?? string.Empty), "Portfolio" },
                    { new StringContent(dto.Website ?? string.Empty), "Website" },
                    { new StringContent(dto.LinkedInHeadline ?? string.Empty), "LinkedInHeadline" }
                };

                if (dto.ProfilePicture != null)
                {
                    var streamContent = new StreamContent(dto.ProfilePicture.OpenReadStream());
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(dto.ProfilePicture.ContentType);
                    content.Add(streamContent, "ProfilePicture", dto.ProfilePicture.FileName);
                }

                var response = await _httpClient.PostAsync("api/cv/personal-detail", content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("SaveUserPersonalDetailAsync failed. Status: {StatusCode}, Content: {Content}",
                        response.StatusCode, error);
                    return ApiResponse<UserPersonalDetailDto>.ErrorResponse(error);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserPersonalDetailDto>>();

                if (result == null)
                {
                    _logger.LogWarning("SaveUserPersonalDetailAsync returned null response");
                    return ApiResponse<UserPersonalDetailDto>.ErrorResponse("Invalid response from API");
                }

                _logger.LogInformation("Personal detail saved successfully for user {UserId}", dto.UserId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in SaveUserPersonalDetailAsync for user {UserId}", dto.UserId);
                return ApiResponse<UserPersonalDetailDto>.ErrorResponse("Network or API error: " + ex.Message);
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
        public async Task<ApiResponse<List<UserEducationDto>>> SaveuserEducationAsync(List<UserEducationDto> educationList)
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

        /// <summary>
        /// Get User Skill by userId
        /// </summary>
        public async Task<ApiResponse<List<UserSkillDto>>> GetUserSkillAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching skill records for user {UserId}", userId);

                var response = await _httpClient.GetAsync($"api/cv/{userId}/skill");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("GetUserSkillAsync failed for user {UserId}: {Error}", userId, error);
                    return ApiResponse<List<UserSkillDto>>.ErrorResponse(error);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<UserSkillDto>>>();

                if (result == null)
                {
                    _logger.LogWarning("GetUserSkillAsync returned null for user {UserId}", userId);
                    return ApiResponse<List<UserSkillDto>>.ErrorResponse("Invalid response from API");
                }

                _logger.LogInformation("Successfully fetched skill records for user {UserId}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetUserSkillAsync for user {UserId}", userId);
                return ApiResponse<List<UserSkillDto>>.ErrorResponse("Network or API error");
            }
        }

        /// <summary>
        /// Save user skill records
        /// </summary>
        public async Task<ApiResponse<List<UserSkillDto>>> SaveUserSkillAsync(List<UserSkillDto> skillList)
        {
            try
            {
                _logger.LogInformation("Saving user skill. Count: {Count}", skillList.Count);

                var response = await _httpClient.PostAsJsonAsync("api/cv/skill", skillList);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("SaveUserSkillAsync failed: {Error}", error);
                    return ApiResponse<List<UserSkillDto>>.ErrorResponse(error);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<UserSkillDto>>>();

                if (result == null)
                {
                    _logger.LogWarning("SaveSkillAsync returned null response");
                    return ApiResponse<List<UserSkillDto>>.ErrorResponse("Invalid response from API");
                }

                _logger.LogInformation("User skill saved successfully");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in SaveUserSkillAsync");
                return ApiResponse<List<UserSkillDto>>.ErrorResponse("Network or API error");
            }
        }
    }
}