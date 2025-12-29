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
                return View(model);
            }
            try
            {
                var loginRequest = new LoginRequestDto
                {
                    Email = model.Email,
                    Password = model.Password
                };

                var apiResponse = await _authApiService.LoginAsync(loginRequest);

                if (!apiResponse.Success || apiResponse.Data == null) 
                {
                    ModelState.AddModelError("", apiResponse.Message ?? "Invalid login attempt.");
                    return View(model);
                }

                var authData = apiResponse.Data;

                var claims = new List<Claim>
                {
                    new Claim (ClaimTypes.NameIdentifier, authData.User.Id.ToString()),
                    new Claim (ClaimTypes.Email, authData.User.Email),
                    new Claim (ClaimTypes.Name, $"{authData.User.FullName}"),
                    //new Claim (ClaimTypes.Role, authData.Role ?? "User"),
                    new Claim ("Token", authData.Token)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authproperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                    RedirectUri = returnUrl ?? "/"
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}