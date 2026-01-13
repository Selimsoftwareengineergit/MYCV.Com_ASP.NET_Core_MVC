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
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            // Fix: Preserve ReturnUrl from parameter if model doesn't have it
            model.ReturnUrl = returnUrl ?? model.ReturnUrl;
            ViewData["ReturnUrl"] = model.ReturnUrl;

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Login model validation failed for email: {Email}", model.Email);
                return View(model);
            }

            try
            {
                // Create DTO for API call
                var loginRequest = new LoginRequestDto
                {
                    Email = model.Email,
                    Password = model.Password,
                    VerificationCode = model.VerificationCode
                };

                _logger.LogInformation("Attempting login for user: {Email}", model.Email);

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
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                    RedirectUri = model.ReturnUrl ?? "/",

                    // Security settings
                    AllowRefresh = true,
                    IssuedUtc = DateTimeOffset.UtcNow
                };

                // Sign in user with cookie authentication
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                _logger.LogInformation("User {Email} logged in successfully", model.Email);
                TempData["SuccessMessage"] = "Login successful! Welcome back.";

                return RedirectToLocal(model.ReturnUrl);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API connection error during login for {Email}", model.Email);
                ModelState.AddModelError("", "Unable to connect to authentication service. Please try again.");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login for {Email}", model.Email);
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                return View(model);
            }
        }

        //GET:/Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken] // CSRF protection
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Registration model validation failed for email: {Email}", model.Email);
                return View(model);
            }

            try
            {
                // Create DTO for API call
                var registerRequest = new UserCreateRequestDto
                {
                    FullName = model.FullName.Trim(),
                    Email = model.Email.Trim().ToLower(),
                    Password = model.Password,
                };

                _logger.LogInformation("Attempting registration for email: {Email}", model.Email);

                // Call API service
                var apiResponse = await _authApiService.RegisterAsync(registerRequest);

                if (!apiResponse.Success)
                {
                    _logger.LogWarning("Registration failed for {Email}: {Message}",
                        model.Email, apiResponse.Message);

                    ModelState.AddModelError("", apiResponse.Message ?? "Registration failed.");
                    return View(model);
                }

                _logger.LogInformation("User {Email} registered successfully", model.Email);
                TempData["SuccessMessage"] = "Registration successful! You can now login.";

                return RedirectToAction("Login");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API connection error during registration for {Email}", model.Email);
                ModelState.AddModelError("", "Unable to connect to registration service. Please try again.");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration for {Email}", model.Email);
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                return View(model);
            }
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF protection
        [Authorize] // Only authenticated users can logout
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);

                // Call API logout
                await _authApiService.LogoutAsync();

                // Clear local authentication cookie
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                _logger.LogInformation("User {Email} logged out successfully", userEmail);
                TempData["SuccessMessage"] = "You have been logged out successfully.";

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                TempData["ErrorMessage"] = "Error during logout. Please try again.";
                return RedirectToAction("Index", "Home");
            }
        }

        //GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            _logger.LogWarning("Access denied for user: {UserName}", User.Identity?.Name);
            return View();
        }

        //GET: /Account/Profile
        [HttpGet]
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        //GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Note: For now, we'll just show success message
            //In production, intregrate with email service
            TempData["SuccessMessage"] = "If an account exists with this email, you will receive password reset instructions";
            return RedirectToAction("Login");
        }

        //GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Invalid reset password link.";
                return RedirectToAction("Login");
            }

            var model = new ResetPasswordViewModel
            {
                Token = token,
                Email = email
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            { 
                return View(model);
            }
            TempData["SuccessMessage"] = "Password reset successful! You can login with your new password.";
            return RedirectToAction("Login");
        }

        //Safely redirects to a local URL
        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}