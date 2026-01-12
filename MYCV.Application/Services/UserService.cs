using Microsoft.Extensions.Logging;
using MYCV.Application.DTOs;
using MYCV.Application.Interfaces;
using MYCV.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MYCV.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<UserService> _logger;
        private readonly IEmailService _emailService;

        public UserService(
            IUserRepository repository,
            ILogger<UserService> logger,
            IEmailService emailService)  
        {
            _repository = repository;
            _logger = logger;
            _emailService = emailService;  
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

        public async Task<UserResponseDto> CreateUserAsync(UserCreateRequestDto dto)
        {
            var existingUser = await _repository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("Email already exists");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = HashPassword(dto.Password),
                VerificationCode = GenerateVerificationCode(),
            };

            // 1️⃣ Save user FIRST
            await _repository.AddAsync(user);

            // 2️⃣ Fire-and-forget email (DO NOT await)
            _ = _emailService.SendVerificationEmailAsync(
                user.Email,
                user.FullName,
                user.VerificationCode
            );

            // 3️⃣ Return response immediately
            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                IsEmailVerified = user.IsEmailVerified,
                CreatedDate = user.CreatedDate
            };
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 10);
        }
        private string GenerateVerificationCode()
        {
            return RandomNumberGenerator.GetInt32(10000000, 100000000).ToString();
        }
    }
}