using MYCV.Application.DTOs;

namespace MYCV.Web.Services.Api
{
    public interface ICvApiService
    {
        Task<ApiResponse<UserPersonalDetailDto>> GetUserPersonalDetailAsync(int userId);
        Task<ApiResponse<UserPersonalDetailDto>> SaveUserPersonalDetailAsync(UserPersonalDetailDto dto);
        Task<ApiResponse<List<UserEducationDto>>> GetUserEducationAsync(int userId);
        Task<ApiResponse<List<UserEducationDto>>> SaveuserEducationAsync(List<UserEducationDto> educationList);
        Task<ApiResponse<List<UserExperienceDto>>> GetUserExperiencesAsync(int userId);
        Task<ApiResponse<List<UserExperienceDto>>> SaveUserExperiencesAsync(List<UserExperienceDto> experienceList);
    }
}