using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYCV.Web.Services.Api;
using MYCV.Application.DTOs;
using MYCV.Web.ViewModels;
using MYCV.Domain.Entities;
using System.Security.Claims;
using MYCV.Web.Helpers;
using MYCV.Domain.Enums;

namespace MYCV.Web.Controllers
{
    [Authorize] 
    public class CvBuilderController : Controller
    {
        private readonly ICvApiService _cvApiService;
        private readonly ILogger<CvBuilderController> _logger;

        public CvBuilderController(ICvApiService cvApiService, ILogger<CvBuilderController> logger)
        {
            _cvApiService = cvApiService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return RedirectToAction("Login", "Account");

                var userId = int.Parse(userIdClaim);

                // Step 1: Personal Information
                var personalInfoResponse = await _cvApiService.GetUserPersonalDetailAsync(userId);
                bool step1Completed = personalInfoResponse.Success && personalInfoResponse.Data != null;

                // Step 2: Education
                var educationResponse = await _cvApiService.GetUserEducationAsync(userId);
                bool step2Completed = educationResponse.Success && educationResponse.Data != null && educationResponse.Data.Any();

                // Step 3: Experience
                var experienceResponse = await _cvApiService.GetUserExperiencesAsync(userId);
                bool step3Completed = experienceResponse.Success && experienceResponse.Data != null && experienceResponse.Data.Any();

                // 🔐 Step lock rules
                if (!step1Completed)
                    return View("Index"); // Step 1 not done, stay on personal info

                if (!step2Completed)
                    return RedirectToAction("Education"); // Step 2 not done, go to Education

                if (!step3Completed)
                    return RedirectToAction("Experience"); // Step 3 not done, go to Experience

                // All steps completed → go to preview/download
                return RedirectToAction("PreviewDownload");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading CV Builder for user {User}", User.Identity?.Name);
                TempData["ErrorMessage"] = "Unexpected error loading CV Builder.";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SavePersonalDetail([FromForm] UserPersonalDetailDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid personal info submitted for user {User}", User.Identity?.Name);
                return BadRequest(new { Success = false, Message = "Please fill all required fields." });
            }

            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized();

                model.UserId = int.Parse(userIdClaim);

                var result = await _cvApiService.SaveUserPersonalDetailAsync(model);

                if (!result.Success)
                {
                    _logger.LogWarning("Failed to save personal info for user {UserId}: {Message}", model.UserId, result.Message);
                    return BadRequest(new { Success = false, Message = result.Message });
                }

                _logger.LogInformation("Saved personal info successfully for user {UserId}", model.UserId);
                return Ok(new { Success = true, Data = result.Data, Message = "Personal info saved successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving personal info for user {User}", User.Identity?.Name);
                return StatusCode(500, new { Success = false, Message = "Internal server error." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Education()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return RedirectToAction("Login", "Account");

                var userId = int.Parse(userIdClaim);

                var response = await _cvApiService.GetUserEducationAsync(userId);

                if (!response.Success)
                {
                    _logger.LogWarning("Failed to load education for user {UserId}: {Message}", userId, response.Message);
                    TempData["ErrorMessage"] = response.Message ?? "Unable to load education data.";
                    return View(new List<UserEducationDto>());
                }

                return View(response.Data ?? new List<UserEducationDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading education for user {User}", User.Identity?.Name);
                TempData["ErrorMessage"] = "An unexpected error occurred while loading education data.";
                return View(new List<UserEducationDto>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveEducation([FromBody] List<UserEducationDto> educationList)
        {
            if (educationList == null || !educationList.Any())
            {
                return BadRequest(new { Success = false, Message = "At least one education record is required." });
            }

            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { Success = false, Message = "User not authorized." });

                var userId = int.Parse(userIdClaim);

                foreach (var edu in educationList)
                {
                    edu.UserId = userId;
                }

                var result = await _cvApiService.SaveEducationAsync(educationList);

                if (!result.Success)
                {
                    _logger.LogWarning("Failed to save education for user {UserId}: {Message}", userId, result.Message);
                    return BadRequest(new { Success = false, Message = result.Message });
                }

                _logger.LogInformation("Saved education successfully for user {UserId}", userId);
                return Ok(new
                {
                    Success = true,
                    Data = result.Data,
                    Message = "Education saved successfully!"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving education for user {User}", User.Identity?.Name);
                return StatusCode(500, new { Success = false, Message = "Internal server error." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Experience()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return RedirectToAction("Login", "Account");

                var userId = int.Parse(userIdClaim);

                // Get existing experiences
                var response = await _cvApiService.GetUserExperiencesAsync(userId);

                if (!response.Success)
                {
                    _logger.LogWarning("Failed to load experiences for user {UserId}: {Message}", userId, response.Message);
                    TempData["ErrorMessage"] = response.Message ?? "Unable to load experience data.";
                    return View(new List<UserExperienceDto>());
                }

                return View(response.Data ?? new List<UserExperienceDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading experiences for user {User}", User.Identity?.Name);
                TempData["ErrorMessage"] = "An unexpected error occurred while loading experience data.";
                return View(new List<UserExperienceDto>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveExperience([FromBody] List<UserExperienceDto> experienceList)
        {
            if (experienceList == null || !experienceList.Any())
            {
                return BadRequest(new { Success = false, Message = "At least one experience record is required." });
            }

            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { Success = false, Message = "User not authorized." });

                var userId = int.Parse(userIdClaim);

                foreach (var exp in experienceList)
                {
                    exp.UserId = userId;
                }

                var result = await _cvApiService.SaveUserExperiencesAsync(experienceList);

                if (!result.Success)
                {
                    _logger.LogWarning("Failed to save experiences for user {UserId}: {Message}", userId, result.Message);
                    return BadRequest(new { Success = false, Message = result.Message });
                }

                _logger.LogInformation("Saved experiences successfully for user {UserId}", userId);
                return Ok(new
                {
                    Success = true,
                    Data = result.Data,
                    Message = "Work experience saved successfully!"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving experiences for user {User}", User.Identity?.Name);
                return StatusCode(500, new { Success = false, Message = "Internal server error." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Step(int stepNumber)
        {
            if (!CvStepHelper.IsValidStep(stepNumber))
                return NotFound();

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return RedirectToAction("Login", "Account");

            var userId = int.Parse(userIdClaim);

            // Step 1: Personal Info
            var personalInfoResponse = await _cvApiService.GetUserPersonalDetailAsync(userId);
            bool step1Completed = personalInfoResponse.Success && personalInfoResponse.Data != null;

            // Step 2: Education
            var educationResponse = await _cvApiService.GetUserEducationAsync(userId);
            bool step2Completed = educationResponse.Success && educationResponse.Data != null && educationResponse.Data.Any();

            // Step 3: Experience
            var experienceResponse = await _cvApiService.GetUserExperiencesAsync(userId);
            bool step3Completed = experienceResponse.Success && experienceResponse.Data != null && experienceResponse.Data.Any();

            // 🔐 Lock rules
            if (stepNumber >= (int)CvStep.Education && !step1Completed)
                return RedirectToAction("Index"); // Can't go to education without personal info

            if (stepNumber >= (int)CvStep.WorkExperience && !step2Completed)
                return RedirectToAction("Education"); // Can't go to experience without education

            if (stepNumber >= (int)CvStep.PreviewDownload && !step3Completed)
                return RedirectToAction("Experience"); // Can't go to preview without experience

            // Navigate to the requested step
            return stepNumber switch
            {
                (int)CvStep.PersonalInformation => RedirectToAction("Index"),
                (int)CvStep.Education => RedirectToAction("Education"),
                (int)CvStep.WorkExperience => RedirectToAction("Experience"),
                (int)CvStep.PreviewDownload => RedirectToAction("PreviewDownload"),
                _ => NotFound()
            };
        }
    }
}
