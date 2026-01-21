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
        public async Task<IActionResult> SavePersonalInfo([FromBody] UserCvPersonalInfoDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid personal info submitted for user {User}", User.Identity?.Name);
                return BadRequest(ApiResponse<UserCvResponseDto>.ErrorResponse("Please fill all required fields."));
            }

            try
            {
                // Extract UserId from JWT claims
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(ApiResponse<UserCvResponseDto>.ErrorResponse("User not authorized"));

                dto.UserId = int.Parse(userIdClaim);

                _logger.LogInformation("Saving personal info for user {UserId} ({Email})", dto.UserId, dto.Email);

                // Call service to save personal info
                var savedCv = await _cvService.SavePersonalInfoAsync(dto);

                _logger.LogInformation("Personal info saved successfully with CV ID: {CvId} for user {UserId}", savedCv.Id, dto.UserId);

                return Ok(ApiResponse<UserCvResponseDto>.SuccessResponse(savedCv, "Personal information saved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving personal info for user {Email}", dto.Email);
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ApiResponse<UserCvResponseDto>.ErrorResponse("Internal server error"));
            }
        }
    }
}