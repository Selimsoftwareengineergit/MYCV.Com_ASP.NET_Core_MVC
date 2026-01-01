using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYCV.Application.DTOs;
using MYCV.Web.Services.Api;
using MYCV.Web.ViewModels;
using System.Security.Claims;

namespace MYCV.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthApiService _authApiService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthApiService authApiService, ILogger<AccountController> logger)
        {
            _authApiService = authApiService;
            _logger = logger;
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                _logger.LogInformation("User already authenticated, redirecting to home");
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Login model validation failed for email: {Email}",model.Email);
                return View(model);
            }
            try
            {
                // Create DTO for API call
                var loginRequest = new LoginRequestDto
                {
                    Email = model.Email,
                    Password = model.Password
                };
                _logger.LogInformation("Attempting login for user: {Email}",model.Email);

                // Call API service (Web → API communication pattern)
                var apiResponse = await _authApiService.LoginAsync(loginRequest);

                // Check API response
                if (!apiResponse.Success || apiResponse.Data == null) 
                {
                    _logger.LogWarning("Login failed for {Email}: {Message}", model.Email, apiResponse.Message);
                    ModelState.AddModelError("", apiResponse.Message ?? "Invalid login attempt.");
                    return View(model);
                }

                var authData = apiResponse.Data;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, authData.User.Id.ToString()),
                    new Claim(ClaimTypes.Email, authData.User.Email),
                    new Claim(ClaimTypes.Name, $"{authData.User.FullName}"),
                    //new Claim(ClaimTypes.Role, authData.User.Role ?? "User"), // Role-based authorization
                    new Claim("Token", authData.Token), // Store JWT token
                    new Claim("UserId", authData.User.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Create authentication properties with security settings
                var authproperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                    RedirectUri = returnUrl ?? "/",

                    // Security settings
                    AllowRefresh = true,
                    IssuedUtc = DateTimeOffset.UtcNow
                };

                // Sign in user with cookie authentication
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity),
                    authproperties);

                _logger.LogInformation("User {Email} logged in successfully", model.Email);
                TempData["SuccessMessage"] = "Login successful! Welcome back.";

                return RedirectToLocal(returnUrl);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IActionResult RedirectToLocal(string? )
    }
}