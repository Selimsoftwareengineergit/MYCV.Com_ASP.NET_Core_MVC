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
        private readonly ILogger<UserCvController> _logger;
        private readonly IUserPersonalDetailService _cvService;
        private readonly IUserEducationService _userEducationService;
        private readonly IUserExperienceService _userExperienceService;
        public UserCvController(ILogger<UserCvController> logger, IUserPersonalDetailService cvService,
            IUserEducationService userEducationService, IUserExperienceService userExperienceService)
        {
            _logger = logger;
            _cvService = cvService;
            _userEducationService = userEducationService;
            _userExperienceService = userExperienceService;
        }

        /// <summary>
        /// Get CV for a user by userId (int)
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>ApiResponse with user's personal CV info</returns>
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetUserPersonalDetail(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching CV for user {UserId}", userId);

                var cv = await _cvService.GetUserPersonalDetailAsync(userId);
                if (cv == null)
                {
                    _logger.LogWarning("No CV found for user {UserId}", userId);
                    return NotFound(ApiResponse<UserPersonalDetailDto>.ErrorResponse("CV not found"));
                }

                return Ok(ApiResponse<UserPersonalDetailDto>.SuccessResponse(cv, "CV fetched successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching CV for user {UserId}", userId);
                return StatusCode(500, ApiResponse<UserPersonalDetailDto>.ErrorResponse("Internal server error"));
            }
        }

        /// <summary>
        /// Save user CV personal information
        /// </summary>
        /// <param name="dto">User CV personal info DTO</param>
        /// <returns>ApiResponse with saved CV data</returns>
        [HttpPost("personal-detail")]
        public async Task<IActionResult> SaveUserPersonalDetail([FromForm] UserPersonalDetailDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<UserPersonalDetailDto>.ErrorResponse("Please fill all required fields."));

            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                                  ?? User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(ApiResponse<UserPersonalDetailDto>.ErrorResponse("User not authorized"));

                dto.UserId = int.Parse(userIdClaim);
                var savedCv = await _cvService.SaveUserPersonalDetailAsync(dto);

                return Ok(ApiResponse<UserPersonalDetailDto>.SuccessResponse(savedCv, "Personal information saved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving personal info for user {UserId}", dto.UserId);
                return StatusCode(500, ApiResponse<UserPersonalDetailDto>.ErrorResponse("Internal server error"));
            }
        }

        /// <summary>
        /// Get all education records for a user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>ApiResponse with user's education list</returns>
        [HttpGet("{userId:int}/education")]
        public async Task<IActionResult> GetUserEducation(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching education records for user {UserId}", userId);

                var educationList = await _userEducationService.GetUserEducationAsync(userId);

                if (educationList == null || !educationList.Any())
                {
                    _logger.LogWarning("No education records found for user {UserId}", userId);
                    return NotFound(ApiResponse<List<UserEducationDto>>.ErrorResponse("No education records found"));
                }

                return Ok(ApiResponse<List<UserEducationDto>>.SuccessResponse(educationList, "Education records fetched successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching education records for user {UserId}", userId);
                return StatusCode(500, ApiResponse<List<UserEducationDto>>.ErrorResponse("Internal server error"));
            }
        }

        [HttpPost("education")]
        public async Task<IActionResult> SaveEducation([FromBody] List<UserEducationDto> dtoList)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<List<UserEducationDto>>.ErrorResponse("Please fill all required fields."));

            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                                  ?? User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(ApiResponse<List<UserEducationDto>>.ErrorResponse("User not authorized"));

                int userId = int.Parse(userIdClaim);

                var savedList = new List<UserEducationDto>();

                foreach (var dto in dtoList)
                {
                    dto.UserId = userId;
                    var saved = await _userEducationService.SaveUserEducationAsync(dto);
                    savedList.Add(saved);
                }

                return Ok(ApiResponse<List<UserEducationDto>>.SuccessResponse(savedList, "Education information saved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving education info");
                return StatusCode(500, ApiResponse<List<UserEducationDto>>.ErrorResponse("Internal server error"));
            }
        }

        [HttpPost("experience")]
        public async Task<IActionResult> SaveExperience([FromBody] List<UserExperienceDto> dtoList)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<List<UserExperienceDto>>.ErrorResponse("Please fill all required fields."));

            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                                  ?? User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(ApiResponse<List<UserExperienceDto>>.ErrorResponse("User not authorized"));

                int userId = int.Parse(userIdClaim);

                var savedList = new List<UserExperienceDto>();

                foreach (var dto in dtoList)
                {
                    dto.UserId = userId;
                    var saved = await _userExperienceService.SaveUserExperienceAsync(dto);
                    savedList.Add(saved);
                }

                return Ok(ApiResponse<List<UserExperienceDto>>.SuccessResponse(savedList, "Work experience saved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving work experience info");
                return StatusCode(500, ApiResponse<List<UserExperienceDto>>.ErrorResponse("Internal server error"));
            }
        }
    }
}