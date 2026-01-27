using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYCV.Application.DTOs;
using MYCV.Application.Interfaces;
using System.Net;

namespace MYCV.API.Controllers
{
    [ApiController]
    [Route("api/cv")]
    [Authorize] 
    public class UserCvController : ControllerBase
    {
        private readonly IUserCvService _cvService;
        private readonly ILogger<UserCvController> _logger;

        public UserCvController(IUserCvService cvService, ILogger<UserCvController> logger)
        {
            _cvService = cvService;
            _logger = logger;
        }

        /// <summary>
        /// Save user CV personal information
        /// </summary>
        /// <param name="dto">User CV personal info DTO</param>
        /// <returns>ApiResponse with saved CV data</returns>
        [HttpPost("personal-info")]
        public async Task<IActionResult> SavePersonalInfo([FromForm] UserCvPersonalInfoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<UserCvResponseDto>.ErrorResponse("Please fill all required fields."));

            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                                  ?? User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(ApiResponse<UserCvResponseDto>.ErrorResponse("User not authorized"));

                dto.UserId = int.Parse(userIdClaim);
                var savedCv = await _cvService.SavePersonalInfoAsync(dto);

                return Ok(ApiResponse<UserCvResponseDto>.SuccessResponse(savedCv, "Personal information saved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving personal info for user {UserId}", dto.UserId);
                return StatusCode(500, ApiResponse<UserCvResponseDto>.ErrorResponse("Internal server error"));
            }
        }
    }
}