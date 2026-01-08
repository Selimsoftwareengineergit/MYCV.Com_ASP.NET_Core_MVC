using Microsoft.AspNetCore.Mvc;
using MYCV.Application.DTOs;
using MYCV.Application.Services;
using System.Net;

namespace MYCV.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase 
    {
        private readonly IUserService _service;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService service, ILogger<UsersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _service.GetUsersAsync();
                return Ok(ApiResponse<List<UserResponseDto>>.SuccessResponse(users, "Users retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users");
                return StatusCode(500, ApiResponse<List<UserResponseDto>>.ErrorResponse("Internal server error"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateRequestDto dto)
        {
            try
            {
                _logger.LogInformation("Creating user: {Email}", dto.Email);

                var createdUser = await _service.CreateUserAsync(dto);

                _logger.LogInformation("User created successfully with ID: {UserId}", createdUser.Id);

                return Ok(ApiResponse<UserResponseDto>.SuccessResponse(createdUser, "User created successfully"));
            }
            catch (Exception ex) when (ex.Message.Contains("Email already exists"))
            {
                _logger.LogWarning("Email already exists: {Email}", dto.Email);
                return Conflict(ApiResponse<UserResponseDto>.ErrorResponse("Email already exists"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user: {Email}", dto.Email);
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ApiResponse<UserResponseDto>.ErrorResponse($"Error creating user: {ex.Message}"));
            }
        }
    }
}