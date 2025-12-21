using Microsoft.AspNetCore.Mvc;
using MYCV.Application.DTOs;
using MYCV.Application.Services;

namespace MYCV.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service) 
        { 
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return Ok(await _service.GetUsersAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateRequestDto dto)
        {
            await _service.CreateUserAsync(dto);
            return Ok("User created successfully");
        }
    }
}