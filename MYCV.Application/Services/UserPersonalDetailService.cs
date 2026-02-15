using MYCV.Application.DTOs;
using MYCV.Application.Interfaces;
using MYCV.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace MYCV.Application.Services
{
    public class UserPersonalDetailService : IUserPersonalDetailService
    {
        private readonly IUserPersonalDetailRepository _cvRepository;
        private readonly IFileService _fileService;

        public UserPersonalDetailService(
            IUserPersonalDetailRepository cvRepository,
            IFileService fileService)
        {
            _cvRepository = cvRepository;
            _fileService = fileService;
        }

        /// <summary>
        /// Save or update user personal CV information
        /// </summary>
        public async Task<UserPersonalDetailDto> SaveUserPersonalDetailAsync(UserPersonalDetailDto dto)
        {
            // ✅ Upload profile picture if provided
            if (dto.ProfilePicture != null)
            {
                dto.ProfilePictureUrl =
                    await _fileService.UploadProfilePictureAsync(dto.ProfilePicture, dto.UserId);
            }

            var existingCv = await _cvRepository.GetByUserIdAsync(dto.UserId);

            // =========================
            // CREATE
            // =========================
            if (existingCv == null)
            {
                var newCv = new UserPersonalDetail
                {
                    UserId = dto.UserId,
                    FullName = dto.FullName,
                    ProfessionalTitle = dto.ProfessionalTitle,
                    DateOfBirth = dto.DateOfBirth,
                    Gender = dto.Gender,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Country = dto.Country,
                    City = dto.City,
                    Address = dto.Address,
                    ProfilePictureUrl = dto.ProfilePictureUrl,
                    Summary = dto.Summary,
                    Objective = dto.Objective, // ✅ FIXED
                    LinkedIn = dto.LinkedIn,
                    GitHub = dto.GitHub,
                    Portfolio = dto.Portfolio,
                    Website = dto.Website,
                    LinkedInHeadline = dto.LinkedInHeadline,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow
                };

                await _cvRepository.AddAsync(newCv);
                return MapToResponseDto(newCv);
            }

            // =========================
            // UPDATE
            // =========================
            existingCv.FullName = dto.FullName;
            existingCv.ProfessionalTitle = dto.ProfessionalTitle;
            existingCv.DateOfBirth = dto.DateOfBirth;
            existingCv.Gender = dto.Gender;
            existingCv.Email = dto.Email;
            existingCv.PhoneNumber = dto.PhoneNumber;
            existingCv.Country = dto.Country;
            existingCv.City = dto.City;
            existingCv.Address = dto.Address;
            existingCv.Summary = dto.Summary;
            existingCv.Objective = dto.Objective; 
            existingCv.LinkedIn = dto.LinkedIn;
            existingCv.GitHub = dto.GitHub;
            existingCv.Portfolio = dto.Portfolio;
            existingCv.Website = dto.Website;
            existingCv.LinkedInHeadline = dto.LinkedInHeadline;

            if (!string.IsNullOrEmpty(dto.ProfilePictureUrl))
                existingCv.ProfilePictureUrl = dto.ProfilePictureUrl;

            existingCv.UpdatedDate = DateTime.UtcNow;

            await _cvRepository.UpdateAsync(existingCv);
            return MapToResponseDto(existingCv);
        }

        /// <summary>
        /// Get CV for a user by userId
        /// </summary>
        public async Task<UserPersonalDetailDto> GetUserPersonalDetailAsync(int userId)
        {
            var cv = await _cvRepository.GetByUserIdAsync(userId);

            if (cv == null)
                return null;

            return MapToResponseDto(cv);
        }

        /// <summary>
        /// Map Entity to DTO
        /// </summary>
        private static UserPersonalDetailDto MapToResponseDto(UserPersonalDetail cv)
        {
            return new UserPersonalDetailDto
            {
                Id = cv.Id,
                UserId = cv.UserId, 
                FullName = cv.FullName,
                ProfessionalTitle = cv.ProfessionalTitle,
                DateOfBirth = cv.DateOfBirth,
                Gender = cv.Gender,
                Email = cv.Email,
                PhoneNumber = cv.PhoneNumber,
                Country = cv.Country,
                City = cv.City,
                Address = cv.Address,
                ProfilePictureUrl = cv.ProfilePictureUrl,
                Summary = cv.Summary,
                Objective = cv.Objective, 
                LinkedIn = cv.LinkedIn,
                GitHub = cv.GitHub,
                Portfolio = cv.Portfolio,
                Website = cv.Website,
                LinkedInHeadline = cv.LinkedInHeadline
            };
        }
    }
}