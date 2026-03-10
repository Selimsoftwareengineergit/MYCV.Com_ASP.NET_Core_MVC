using MYCV.Application.DTOs;
using MYCV.Application.Interfaces;
using MYCV.Domain.Entities;

namespace MYCV.Application.Services
{
    public class UserLanguageService : IUserLanguageService
    {
        private readonly IUserLanguageRepository _userLanguageRepository;

        public UserLanguageService(IUserLanguageRepository userLanguageRepository)
        {
            _userLanguageRepository = userLanguageRepository;
        }

        /// <summary>
        /// Get all language records for a user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>List of UserLanguageDto</returns>
        public async Task<List<UserLanguageDto>> GetUserLanguageAsync(int userId)
        {
            var languages = await _userLanguageRepository.GetByUserIdAsync(userId);

            if (languages == null || !languages.Any())
                return new List<UserLanguageDto>();

            return languages.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Save multiple language records for a user
        /// </summary>
        /// <param name="dtoList">List of UserLanguageDto</param>
        /// <param name="userId">The ID of the user</param>
        /// <returns>Saved list of UserLanguageDto</returns>
        public async Task<List<UserLanguageDto>> SaveUserLanguageAsync(
            List<UserLanguageDto> dtoList,
            int userId)
        {
            var result = new List<UserLanguageDto>();

            foreach (var dto in dtoList)
            {
                dto.UserId = userId;

                var savedEntity = await SaveOrUpdateAsync(dto);

                result.Add(MapToDto(savedEntity));
            }

            return result;
        }

        /// <summary>
        /// Save or update a single UserLanguage entity
        /// </summary>
        private async Task<UserLanguage> SaveOrUpdateAsync(UserLanguageDto dto)
        {
            UserLanguage? entity = null;

            if (dto.Id > 0)
                entity = await _userLanguageRepository.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                entity = new UserLanguage
                {
                    UserId = dto.UserId,
                    Language = dto.Language,
                    Proficiency = dto.Proficiency,
                    Priority = dto.Priority
                };

                await _userLanguageRepository.AddAsync(entity);
            }
            else
            {
                entity.Language = dto.Language;
                entity.Proficiency = dto.Proficiency;
                entity.Priority = dto.Priority;

                await _userLanguageRepository.UpdateAsync(entity);
            }

            return entity;
        }

        /// <summary>
        /// Map UserLanguage entity to DTO
        /// </summary>
        private static UserLanguageDto MapToDto(UserLanguage entity)
        {
            return new UserLanguageDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Language = entity.Language,
                Proficiency = entity.Proficiency,
                Priority = entity.Priority
            };
        }
    }
}