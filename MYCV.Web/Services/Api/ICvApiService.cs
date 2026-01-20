using MYCV.Application.DTOs;

namespace MYCV.Web.Services.Api
{
    public interface ICvApiService
    {
        Task<ApiResponse<UserCvPersonalInfoDto>> GetUserCvAsync(Guid userId);
        Task<ApiResponse<UserCvPersonalInfoDto>> SavePersonalInfoAsync(UserCvPersonalInfoDto dto);

        // Later: Add methods for other steps
        // Task<ApiResponse<UserCvContactInfoDto>> SaveContactInfoAsync(UserCvContactInfoDto dto);
        // Task<ApiResponse<UserCvSocialInfoDto>> SaveSocialInfoAsync(UserCvSocialInfoDto dto);
        // ...and so on for all CvStep
    }
}