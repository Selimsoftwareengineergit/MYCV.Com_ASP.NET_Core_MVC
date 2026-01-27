using MYCV.Application.DTOs;
using MYCV.Application.Interfaces;
using MYCV.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Application.Services
{
    public class UserCvService : IUserCvService
    {
        private readonly IUserCvRepository _cvRepository;

        public UserCvService(IUserCvRepository cvRepository)
        {
            _cvRepository = cvRepository;
        }

        public async Task<UserCvResponseDto> SavePersonalInfoAsync(UserCvPersonalInfoDto dto)
        {
            var existingCv = await _cvRepository.GetByUserIdAsync(dto.UserId);

            if (existingCv == null)
            {
                var newCv = new UserCv
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

            // Update existing
            existingCv.FullName = dto.FullName;
            existingCv.ProfessionalTitle = dto.ProfessionalTitle;
            existingCv.DateOfBirth = dto.DateOfBirth;
            existingCv.Gender = dto.Gender;
            existingCv.PhoneNumber = dto.PhoneNumber;
            existingCv.Country = dto.Country;
            existingCv.City = dto.City;
            existingCv.Address = dto.Address;
            existingCv.ProfilePictureUrl = dto.ProfilePictureUrl;
            existingCv.Summary = dto.Summary;
            existingCv.LinkedIn = dto.LinkedIn;
            existingCv.GitHub = dto.GitHub;
            existingCv.Portfolio = dto.Portfolio;
            existingCv.Website = dto.Website;
            existingCv.LinkedInHeadline = dto.LinkedInHeadline;
            existingCv.UpdatedDate = DateTime.UtcNow;

            await _cvRepository.UpdateAsync(existingCv);
            return MapToResponseDto(existingCv);
        }

        private static UserCvResponseDto MapToResponseDto(UserCv cv)
        {
            return new UserCvResponseDto
            {
                Id = cv.Id,
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
                LinkedIn = cv.LinkedIn,
                GitHub = cv.GitHub,
                Portfolio = cv.Portfolio,
                Website = cv.Website,
                LinkedInHeadline = cv.LinkedInHeadline
            };
        }
    }
}