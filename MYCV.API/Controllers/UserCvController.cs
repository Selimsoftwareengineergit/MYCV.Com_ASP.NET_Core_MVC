using Microsoft.AspNetCore.Mvc;
using MYCV.Application.DTOs;
using MYCV.Application.Interfaces;
using MYCV.Application.Services;
using System.Net;

namespace MYCV.API.Controllers
{
    [ApiController]
    [Route("api/cv")]
    public class UserCvController : ControllerBase
    {
        private readonly IUserCvService _cvService;
        private readonly ILogger<UserCvController> _logger;

        public UserCvController(IUserCvService cvService, ILogger<UserCvController> logger)
        {
            _cvService = cvService;
            _logger = logger;
        }

        [HttpPost("personal-info")]
        public async Task<IActionResult> SavePersonalInfo([FromBody] UserCvPersonalInfoDto dto)
        {
            try
            {
                _logger.LogInformation("Saving personal info for: {Email}", dto.Email);

                var savedCv = await _cvService.SavePersonalInfoAsync(dto);

                _logger.LogInformation("Personal info saved successfully with CV ID: {CvId}", savedCv.Id);

                return Ok(ApiResponse<UserCvResponseDto>.SuccessResponse(savedCv, "Personal information saved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving personal info for: {Email}", dto.Email);
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ApiResponse<UserCvResponseDto>.ErrorResponse("Internal server error"));
            }
        }
    }
}