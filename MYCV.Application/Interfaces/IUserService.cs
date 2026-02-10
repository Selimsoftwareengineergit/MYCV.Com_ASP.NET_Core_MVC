using MYCV.Application.DTOs;
using MYCV.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetUsersAsync();
        Task<UserDto> CreateUserAsync(UserCreateRequestDto dto);
        Task<UserDto?> CheckEmailAsync(string email);
        Task<User?> GetUserByEmailAsync(string email);
        bool VerifyPassword(string plainPassword, string passwordHash);
        Task UpdateUserAsync(User user);
    }
}