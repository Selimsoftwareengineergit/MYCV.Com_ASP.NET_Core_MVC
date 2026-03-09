using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYCV.Shared.Extensions;
using MYCV.Application.DTOs;
using MYCV.Application.Interfaces;

namespace MYCV.API.Controllers
{
    [ApiController]
    [Route("api/cv")]
    [Authorize] 
    public class UserCvController : ControllerBase
    {
        private readonly ILogger<UserCvController> _logger;
        private readonly IUserPersonalDetailService _userPersonalDetail;
        private readonly IUserEducationService _userEducationService;
        private readonly IUserExperienceService _userExperienceService;
        private readonly IUserSkillService _userSkillService;
        private readonly IUserProjectService _userProjectService;
        public UserCvController(ILogger<UserCvController> logger, IUserPersonalDetailService userPersonalDetail,
            IUserEducationService userEducationService, IUserExperienceService userExperienceService, IUserSkillService userSkillService, IUserProjectService userProjectService)
        {
            _logger = logger;
            _userPersonalDetail = userPersonalDetail;
            _userEducationService = userEducationService;
            _userExperienceService = userExperienceService;
            _userSkillService = userSkillService;
            _userProjectService = userProjectService;
        }

        /// <summary>
        /// Get personal detail for a user by userId (int)
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>ApiResponse with user's personal detail</returns>
        [HttpGet("{userId:int}/personalDetail")]
        public async Task<IActionResult> GetUserPersonalDetail(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching personal detail for user {UserId}", userId);

                var personalDetail = await _userPersonalDetail.GetUserPersonalDetailAsync(userId);
                if (personalDetail == null)
                {
                    _logger.LogWarning("No personal detail found for user {UserId}", userId);
                    return NotFound(ApiResponse<UserPersonalDetailDto>.ErrorResponse("Personal detail not found"));
                }

                return Ok(ApiResponse<UserPersonalDetailDto>.SuccessResponse(personalDetail, "Personal detail fetched successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching personal detail for user {UserId}", userId);
                return StatusCode(500, ApiResponse<UserPersonalDetailDto>.ErrorResponse("Internal server error"));
            }
        }

        /// <summary>
        /// Save user CV personal information
        /// </summary>
        [HttpPost("personal-detail")]
        public async Task<IActionResult> SaveUserPersonalDetail([FromForm] UserPersonalDetailDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<UserPersonalDetailDto>
                    .ErrorResponse("Please fill all required fields."));

            try
            {
                dto.UserId = User.GetUserId();

                var savedCv = await _userPersonalDetail.SaveUserPersonalDetailAsync(dto);

                return Ok(ApiResponse<UserPersonalDetailDto>
                    .SuccessResponse(savedCv, "Personal information saved successfully"));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(ApiResponse<UserPersonalDetailDto>
                    .ErrorResponse("User not authorized"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving personal info for user {UserId}", dto.UserId);
                return StatusCode(500,
                    ApiResponse<UserPersonalDetailDto>
                        .ErrorResponse("Internal server error"));
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

        /// <summary>
        /// Save user education information
        /// </summary>
        [HttpPost("education")]
        public async Task<IActionResult> SaveUserEducation([FromBody] List<UserEducationDto> dtoList)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<List<UserEducationDto>>
                    .ErrorResponse("Please fill all required fields."));

            try
            {
                int userId = User.GetUserId();

                var savedList = await _userEducationService
                    .SaveUserEducationsAsync(dtoList, userId);

                return Ok(ApiResponse<List<UserEducationDto>>
                    .SuccessResponse(savedList, "Education information saved successfully"));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(ApiResponse<List<UserEducationDto>>
                    .ErrorResponse("User not authorized"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving education info for user {UserId}", User.Identity?.Name);
                return StatusCode(500,
                    ApiResponse<List<UserEducationDto>>
                        .ErrorResponse("Internal server error"));
            }
        }

        /// <summary>
        /// Get all work experience records for a user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>ApiResponse with user's experience list</returns>
        [HttpGet("{userId:int}/experience")]
        public async Task<IActionResult> GetUserExperience(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching work experience records for user {UserId}", userId);

                var experienceList = await _userExperienceService.GetUserExperiencesAsync(userId);

                if (experienceList == null || !experienceList.Any())
                {
                    _logger.LogWarning("No work experience records found for user {UserId}", userId);
                    return NotFound(ApiResponse<List<UserExperienceDto>>
                        .ErrorResponse("No work experience records found"));
                }

                return Ok(ApiResponse<List<UserExperienceDto>>
                    .SuccessResponse(experienceList, "Work experience records fetched successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching work experience records for user {UserId}", userId);
                return StatusCode(500, ApiResponse<List<UserExperienceDto>>
                    .ErrorResponse("Internal server error"));
            }
        }

        /// <summary>
        /// Save user experience information
        /// </summary>
        [HttpPost("experience")]
        public async Task<IActionResult> SaveUserExperience([FromBody] List<UserExperienceDto> dtoList)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<List<UserExperienceDto>>
                    .ErrorResponse("Please fill all required fields."));

            try
            {
                int userId = User.GetUserId(); 

                var savedList = await _userExperienceService
                    .SaveUserExperiencesAsync(dtoList, userId); 

                return Ok(ApiResponse<List<UserExperienceDto>>
                    .SuccessResponse(savedList, "Work experience saved successfully"));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(ApiResponse<List<UserExperienceDto>>
                    .ErrorResponse("User not authorized"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving experience info for user {UserId}", User.Identity?.Name);
                return StatusCode(500,
                    ApiResponse<List<UserExperienceDto>>
                        .ErrorResponse("Internal server error"));
            }
        }

        /// <summary>
        /// Get all skills records for a user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>ApiResponse with user's skill list</returns>
        [HttpGet("{userId:int}/skill")]
        public async Task<IActionResult> GetUserSkill(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching user skill records for user {UserId}", userId);

                var skillList = await _userSkillService.GetUserSkillAsync(userId);

                if (skillList == null || !skillList.Any())
                {
                    _logger.LogWarning("No skill records found for user {UserId}", userId);
                    return NotFound(ApiResponse<List<UserSkillDto>>
                        .ErrorResponse("No skill records found"));
                }

                return Ok(ApiResponse<List<UserSkillDto>>
                    .SuccessResponse(skillList, "Skill records fetched successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching skill records for user {UserId}", userId);
                return StatusCode(500, ApiResponse<List<UserSkillDto>>
                    .ErrorResponse("Internal server error"));
            }
        }

        /// <summary>
        /// Save user skill information
        /// </summary>
        [HttpPost("skill")]
        public async Task<IActionResult> SaveUserSkill([FromBody] List<UserSkillDto> dtoList)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<List<UserSkillDto>>
                    .ErrorResponse("Please fill all required fields."));

            try
            {
                int userId = User.GetUserId();

                var savedList = await _userSkillService
                    .SaveUserSkillAsync(dtoList, userId);

                return Ok(ApiResponse<List<UserSkillDto>>
                    .SuccessResponse(savedList, "Skill saved successfully"));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(ApiResponse<List<UserSkillDto>>
                    .ErrorResponse("User not authorized"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving skill info for user {UserId}", User.Identity?.Name);
                return StatusCode(500,
                    ApiResponse<List<UserSkillDto>>
                        .ErrorResponse("Internal server error"));
            }
        }

        /// <summary>
        /// Get all projects records for a user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>ApiResponse with user's project list</returns>
        [HttpGet("{userId:int}/project")]
        public async Task<IActionResult> GetUserProject(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching user project records for user {UserId}", userId);

                var projectList = await _userProjectService.GetUserProjectAsync(userId);

                if (projectList == null || !projectList.Any())
                {
                    _logger.LogWarning("No project records found for user {UserId}", userId);
                    return NotFound(ApiResponse<List<UserProjectDto>>
                        .ErrorResponse("No project records found"));
                }

                return Ok(ApiResponse<List<UserProjectDto>>
                    .SuccessResponse(projectList, "Project records fetched successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching project records for user {UserId}", userId);
                return StatusCode(500, ApiResponse<List<UserProjectDto>>
                    .ErrorResponse("Internal server error"));
            }
        }

        /// <summary>
        /// Save user skill information
        /// </summary>
        [HttpPost("project")]
        public async Task<IActionResult> SaveUserProject([FromBody] List<UserProjectDto> dtoList)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<List<UserProjectDto>>
                    .ErrorResponse("Please fill all required fields."));

            try
            {
                int userId = User.GetUserId();

                var savedList = await _userProjectService
                    .SaveUserProjectAsync(dtoList, userId);

                return Ok(ApiResponse<List<UserProjectDto>>
                    .SuccessResponse(savedList, "Project saved successfully"));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(ApiResponse<List<UserProjectDto>>
                    .ErrorResponse("User not authorized"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving project info for user {UserId}", User.Identity?.Name);
                return StatusCode(500,
                    ApiResponse<List<UserProjectDto>>
                        .ErrorResponse("Internal server error"));
            }
        }
    }
}