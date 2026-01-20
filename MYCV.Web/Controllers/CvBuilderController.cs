using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYCV.Web.Services.Api;
using MYCV.Application.DTOs;
using MYCV.Web.ViewModels;
using MYCV.Domain.Entities;
using System.Security.Claims;
using MYCV.Web.Helpers;

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
                {
                    _logger.LogWarning("UserId claim not found. Redirecting to login.");
                    return RedirectToAction("Login", "Account");
                }

                var userId = Guid.Parse(userIdClaim);

                // Fetch existing CV data if available
                var cvResponse = await _cvApiService.GetUserCvAsync(userId);
                if (!cvResponse.Success)
                {
                    _logger.LogWarning("Failed to fetch CV for user {UserId}: {Message}", userId, cvResponse.Message);
                    TempData["ErrorMessage"] = cvResponse.Message ?? "Unable to load CV data.";
                    return View(new UserCvPersonalInfoDto());
                }

                _logger.LogInformation("Loaded CV data for user {UserId}", userId);
                return View(cvResponse.Data ?? new UserCvPersonalInfoDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error loading CV Builder for user {User}", User.Identity?.Name);
                TempData["ErrorMessage"] = "An unexpected error occurred while loading your CV.";
                return View(new UserCvPersonalInfoDto());
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


        // GET: /CvBuilder/Step/{stepNumber}
        [HttpGet]
        public IActionResult Step(int stepNumber)
        {
            if (!CvStepHelper.IsValidStep(stepNumber))
            {
                return NotFound();
            }

            ViewData["StepNumber"] = stepNumber;
            return View();
        }
    }
}
