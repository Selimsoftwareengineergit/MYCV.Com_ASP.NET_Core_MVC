using MYCV.Application.Interfaces;
using MYCV.Application.DTOs;
using MYCV.Domain.Entities;

namespace MYCV.Application.Services
{
    public class UserPersonalDetailService : IUserPersonalDetailService
    {
        private readonly IUserPersonalDetailRepository _personalDetailRepository;
        private readonly IFileService _fileService;

        public UserPersonalDetailService(
            IUserPersonalDetailRepository cvRepository,
            IFileService fileService)
        {
            _personalDetailRepository = cvRepository ?? throw new ArgumentNullException(nameof(cvRepository));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        public async Task<UserPersonalDetailDto?> GetUserPersonalDetailAsync(int userId)
        {
            var personalDetail = await _personalDetailRepository.GetByUserIdAsync(userId);
            return personalDetail is null ? null : MapToDto(personalDetail);
        }

        public async Task<UserPersonalDetailDto> SaveUserPersonalDetailAsync(UserPersonalDetailDto dto)
        {
            // Fetch existing personal detail
            var existingPersonalDetail = await _personalDetailRepository.GetByUserIdAsync(dto.UserId);
            bool isNew = existingPersonalDetail is null;

            // Upload profile picture if provided
            if (dto.ProfilePicture != null)
            {
                dto.ProfilePictureUrl = await _fileService.UploadProfilePictureAsync(
                    dto.ProfilePicture,
                    dto.UserId,
                    isNew);
            }

            // Create new personal detail
            if (isNew)
            {
                var newPersonalDetail = new UserPersonalDetail
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
                    Objective = dto.Objective,
                    LinkedIn = dto.LinkedIn,
                    GitHub = dto.GitHub,
                    Portfolio = dto.Portfolio,
                    Website = dto.Website,
                    LinkedInHeadline = dto.LinkedInHeadline,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow
                };

                await _personalDetailRepository.AddAsync(newPersonalDetail);
                return MapToDto(newPersonalDetail);
            }

            // Update existing personal detail
            existingPersonalDetail!.FullName = dto.FullName;
            existingPersonalDetail.ProfessionalTitle = dto.ProfessionalTitle;
            existingPersonalDetail.DateOfBirth = dto.DateOfBirth;
            existingPersonalDetail.Gender = dto.Gender;
            existingPersonalDetail.Email = dto.Email;
            existingPersonalDetail.PhoneNumber = dto.PhoneNumber;
            existingPersonalDetail.Country = dto.Country;
            existingPersonalDetail.City = dto.City;
            existingPersonalDetail.Address = dto.Address;
            existingPersonalDetail.Summary = dto.Summary;
            existingPersonalDetail.Objective = dto.Objective;
            existingPersonalDetail.LinkedIn = dto.LinkedIn;
            existingPersonalDetail.GitHub = dto.GitHub;
            existingPersonalDetail.Portfolio = dto.Portfolio;
            existingPersonalDetail.Website = dto.Website;
            existingPersonalDetail.LinkedInHeadline = dto.LinkedInHeadline;

            if (!string.IsNullOrEmpty(dto.ProfilePictureUrl))
                existingPersonalDetail.ProfilePictureUrl = dto.ProfilePictureUrl;

            existingPersonalDetail.UpdatedDate = DateTime.UtcNow;

            await _personalDetailRepository.UpdateAsync(existingPersonalDetail);

            return MapToDto(existingPersonalDetail);
        }

        private static UserPersonalDetailDto MapToDto(UserPersonalDetail personalDetail)
        {
            return new UserPersonalDetailDto
            {
                Id = personalDetail.Id,
                UserId = personalDetail.UserId,
                FullName = personalDetail.FullName,
                ProfessionalTitle = personalDetail.ProfessionalTitle,
                DateOfBirth = personalDetail.DateOfBirth, 
                Gender = personalDetail.Gender,
                Email = personalDetail.Email,
                PhoneNumber = personalDetail.PhoneNumber,
                Country = personalDetail.Country,
                City = personalDetail.City,
                Address = personalDetail.Address,
                ProfilePictureUrl = personalDetail.ProfilePictureUrl,
                Summary = personalDetail.Summary,
                Objective = personalDetail.Objective,
                LinkedIn = personalDetail.LinkedIn,
                GitHub = personalDetail.GitHub,
                Portfolio = personalDetail.Portfolio,
                Website = personalDetail.Website,
                LinkedInHeadline = personalDetail.LinkedInHeadline
            };
        }
    }
}