using Microsoft.Extensions.Logging;
using MYCV.Application.DTOs;
using MYCV.Application.Interfaces;
using MYCV.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MYCV.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository repository, ILogger<UserService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<UserResponseDto>> GetUsersAsync()
        {
            var users = await _repository.GetAllAsync();

            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                IsEmailVerified = u.IsEmailVerified,
                CreatedDate = u.CreatedDate
            }).ToList();
        }

        public async Task CreateUserAsync(UserCreateRequestDto dto)
        {
            var existingUser = await _repository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("Email already exists");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = HashPassword(dto.Password),
                VerificationCode = Guid.NewGuid().ToString("N")
            };

            await _repository.AddAsync(user);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}