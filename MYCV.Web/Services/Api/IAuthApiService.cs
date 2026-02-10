using MYCV.Application.DTOs;

namespace MYCV.Web.Services.Api
{
    public interface IAuthApiService
    {
        Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto dto);
        Task<ApiResponse<UserDto>> RegisterAsync(UserCreateRequestDto dto);
        Task<ApiResponse<bool>> LogoutAsync();
        Task<ApiResponse<bool>> ForgotPasswordAsync(string email);
        Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequestDto dto);
    }
}