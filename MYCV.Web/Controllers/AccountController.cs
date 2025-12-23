using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYCV.Application.DTOs;
using MYCV.Web.Services.Api;
using MYCV.Web.ViewModels;

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
                var loginrequest = new LoginRequestDto
                {
                    Email = model.Email,
                    Password = model.Password
                };

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}