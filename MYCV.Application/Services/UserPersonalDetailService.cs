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
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Fetch existing personal detail
            var existingPersonalDetail = await _personalDetailRepository.GetByUserIdAsync(dto.UserId);
            bool isNew = existingPersonalDetail is null;

            // Upload profile picture if provided
            if (dto.ProfilePicture != null && dto.ProfilePicture.Length > 0)
            {
                // Ensure the stream is properly disposed after upload
                dto.ProfilePictureUrl = await _fileService.UploadProfilePictureAsync(
                    dto.ProfilePicture,
                    dto.UserId,
                    isNew);
            }

            if (isNew)
            {
                // Create new personal detail
                var newPersonalDetail = new UserPersonalDetail
                {
                    UserId = dto.UserId,
                    FullName = dto.FullName,
                    ProfessionalTitle = dto.ProfessionalTitle,
                    DateOfBirth = dto.DateOfBirth,
                    Gender = dto.Gender,
                    Religion = dto.Religion,
                    PhoneNumber = dto.PhoneNumber,
                    Country = dto.Country,
                    City = dto.City,
                    PresentAddress = dto.PresentAddress,
                    PermanentAddress = dto.PermanentAddress,
                    ProfilePictureUrl = dto.ProfilePictureUrl,
                    LinkedIn = dto.LinkedIn,
                    GitHub = dto.GitHub,
                    Portfolio = dto.Portfolio,
                    Website = dto.Website,
                    LinkedInHeadline = dto.LinkedInHeadline,
                    Nationality = dto.Nationality,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                await _personalDetailRepository.AddAsync(newPersonalDetail);

                return MapToDto(newPersonalDetail);
            }

            // Update existing personal detail
            existingPersonalDetail.FullName = dto.FullName;
            existingPersonalDetail.ProfessionalTitle = dto.ProfessionalTitle;
            existingPersonalDetail.DateOfBirth = dto.DateOfBirth;
            existingPersonalDetail.Gender = dto.Gender;
            existingPersonalDetail.Religion = dto.Religion;
            existingPersonalDetail.PhoneNumber = dto.PhoneNumber;
            existingPersonalDetail.Country = dto.Country;
            existingPersonalDetail.City = dto.City;
            existingPersonalDetail.PresentAddress = dto.PresentAddress;
            existingPersonalDetail.PermanentAddress = dto.PermanentAddress;
            existingPersonalDetail.LinkedIn = dto.LinkedIn;
            existingPersonalDetail.GitHub = dto.GitHub;
            existingPersonalDetail.Portfolio = dto.Portfolio;
            existingPersonalDetail.Website = dto.Website;
            existingPersonalDetail.LinkedInHeadline = dto.LinkedInHeadline;

            // Only update the profile picture if a new one was uploaded
            if (!string.IsNullOrEmpty(dto.ProfilePictureUrl))
                existingPersonalDetail.ProfilePictureUrl = dto.ProfilePictureUrl;

            existingPersonalDetail.Nationality = dto.Nationality;
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
                Religion = personalDetail.Religion,
                PhoneNumber = personalDetail.PhoneNumber,
                Country = personalDetail.Country,
                City = personalDetail.City,
                PresentAddress = personalDetail.PresentAddress,
                PermanentAddress = personalDetail.PermanentAddress,
                ProfilePictureUrl = personalDetail.ProfilePictureUrl,
                LinkedIn = personalDetail.LinkedIn,
                GitHub = personalDetail.GitHub,
                Portfolio = personalDetail.Portfolio,
                Website = personalDetail.Website,
                LinkedInHeadline = personalDetail.LinkedInHeadline,
                Nationality = personalDetail.Nationality
            };
        }
    }
}