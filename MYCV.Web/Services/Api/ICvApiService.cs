using MYCV.Application.DTOs;

namespace MYCV.Web.Services.Api
{
    public interface ICvApiService
    {
        Task<ApiResponse<UserCvPersonalInfoDto>> GetUserCvAsync(int userId);
        Task<ApiResponse<UserCvPersonalInfoDto>> SavePersonalInfoAsync(UserCvPersonalInfoDto dto);
        Task<ApiResponse<List<UserEducationDto>>> GetUserEducationAsync(int userId);
        Task<ApiResponse<List<UserEducationDto>>> SaveEducationAsync(List<UserEducationDto> educationList);
        Task<ApiResponse<List<UserExperienceDto>>> GetUserExperiencesAsync(int userId);
        Task<ApiResponse<List<UserExperienceDto>>> SaveUserExperiencesAsync(List<UserExperienceDto> experienceList);
    }
}