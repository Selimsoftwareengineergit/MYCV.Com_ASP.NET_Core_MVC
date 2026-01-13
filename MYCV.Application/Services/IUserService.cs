using MYCV.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Application.Services
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetUsersAsync();
        Task<UserResponseDto> CreateUserAsync(UserCreateRequestDto dto);
        Task<UserResponseDto?> CheckEmailAsync(string email);
    }
}