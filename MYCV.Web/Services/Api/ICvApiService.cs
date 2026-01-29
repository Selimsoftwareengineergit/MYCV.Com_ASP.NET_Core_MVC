using MYCV.Application.DTOs;

namespace MYCV.Web.Services.Api
{
    public interface ICvApiService
    {
        Task<ApiResponse<UserCvPersonalInfoDto>> GetUserCvAsync(int userId);
        Task<ApiResponse<UserCvPersonalInfoDto>> SavePersonalInfoAsync(UserCvPersonalInfoDto dto);
        Task<ApiResponse<List<UserEducationDto>>> GetUserEducationAsync(int userId);
    }
}