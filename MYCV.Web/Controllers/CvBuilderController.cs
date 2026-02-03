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

        // GET: /CvBuilder
        public async Task<IActionResult> Index()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return RedirectToAction("Login", "Account");

                var userId = int.Parse(userIdClaim);

                var personalInfoResponse = await _cvApiService.GetUserCvAsync(userId);
                bool step1Completed = personalInfoResponse.Success && personalInfoResponse.Data != null;

                var educationResponse = await _cvApiService.GetUserEducationAsync(userId);
                bool step2Completed = educationResponse.Success && educationResponse.Data != null && educationResponse.Data.Any();

                //bool step3Completed = ...

                if (!step1Completed) return RedirectToAction("Index");
                if (!step2Completed) return RedirectToAction("Education");
                //if (!step3Completed) return RedirectToAction("Experience");

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
        public async Task<IActionResult> SavePersonalInfo([FromBody] UserCvPersonalInfoDto model)
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

                var result = await _cvApiService.SavePersonalInfoAsync(model);

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


        // POST: /CvBuilder/SaveEducation
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

                // 🔹 Get User CV first (because UserEducation requires UserCvId)
                var cvResponse = await _cvApiService.GetUserCvAsync(userId);
                if (!cvResponse.Success || cvResponse.Data == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "CV not found. Please complete personal information first."
                    });
                }

                var userCvId = cvResponse.Data.Id;

                // 🔹 Assign UserCvId to each education record
                foreach (var edu in educationList)
                {
                    edu.UserCvId = userCvId;
                }

                // 🔹 Save education records
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
        public async Task<IActionResult> Step(int stepNumber)
        {
            if (!CvStepHelper.IsValidStep(stepNumber))
                return NotFound();

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return RedirectToAction("Login", "Account");

            var userId = int.Parse(userIdClaim);

            // Check completion
            var personalInfoResponse = await _cvApiService.GetUserCvAsync(userId);
            bool step1Completed = personalInfoResponse.Success && personalInfoResponse.Data != null;

            var educationResponse = await _cvApiService.GetUserEducationAsync(userId);
            bool step2Completed = educationResponse.Success && educationResponse.Data != null && educationResponse.Data.Any();

            //bool step3Completed = ...

            // 🔐 Lock logic:
            if (stepNumber == (int)CvStep.Education && !step1Completed)
                return RedirectToAction("Index");

            if (stepNumber == (int)CvStep.WorkExperience && !step2Completed)
                return RedirectToAction("Education");

            if (stepNumber == (int)CvStep.PreviewDownload && !step2Completed)
                return RedirectToAction("Education");

            // ✅ Allow navigation
            return stepNumber switch
            {
                (int)CvStep.PersonalInformation => RedirectToAction("Index"),
                (int)CvStep.Education => RedirectToAction("Education"),
                //(int)CvStep.WorkExperience => RedirectToAction("Experience"),
                (int)CvStep.PreviewDownload => RedirectToAction("PreviewDownload"),
                _ => NotFound()
            };
        }
    }
}
