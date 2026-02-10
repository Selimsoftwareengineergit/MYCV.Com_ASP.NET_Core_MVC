using MYCV.Application.DTOs;
using System.Net.Http.Headers;

namespace MYCV.Web.Services.Api
{
    public class AuthApiService:IAuthApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthApiService> _logger;

        public AuthApiService(
            HttpClient httpClient, IHttpContextAccessor httpContextAccessor,
            ILogger<AuthApiService> logger
            )
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/users/login", dto);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDto>>();
                    return result ?? ApiResponse<AuthResponseDto>.ErrorResponse("Invalid response");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Login Failed: {StatusCode}, {Error}", response.StatusCode, errorContent);
                return ApiResponse<AuthResponseDto>.ErrorResponse($"Login Failed: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login Error");
                return ApiResponse<AuthResponseDto>.ErrorResponse("Network error");
            }
        }

        public async Task<ApiResponse<UserDto>> RegisterAsync(UserCreateRequestDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/users", dto);

                if (response.IsSuccessStatusCode) 
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
                    return result ?? ApiResponse<UserDto>.ErrorResponse("Invalid Response");
                }
                return ApiResponse<UserDto>.ErrorResponse("Registration failed: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration error");
                return ApiResponse<UserDto>.ErrorResponse("Network error");
            }
        }

        public async Task<ApiResponse<bool>> LogoutAsync()
        {
            try
            {
                AddAuthHeader();
                var response = await _httpClient.PostAsync("api/auth/logout", null);

                if (response.IsSuccessStatusCode) 
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
                    return result ?? ApiResponse<bool>.ErrorResponse("Invalid response"); 
                }

                return ApiResponse<bool>.ErrorResponse($"Logout Failed: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout error");
                return ApiResponse<bool>.ErrorResponse("Network error");
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

        public Task<ApiResponse<bool>> ForgotPasswordAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequestDto dto)
        {
            throw new NotImplementedException();
        }
    }
}