using MYCV.Application.DTOs;
using MYCV.Application.Interfaces;
using MYCV.Domain.Entities;

namespace MYCV.Application.Services
{
    public class UserSkillService : IUserSkillService
    {
        private readonly IUserSkillRepository _userSkillRepository;

        public UserSkillService(IUserSkillRepository userSkillRepository)
        {
            _userSkillRepository = userSkillRepository;
        }

        /// <summary>
        /// Get all skill records for a user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>List of UserSkillDto</returns>
        public async Task<List<UserSkillDto>> GetUserSkillAsync(int userId)
        {
            var skills = await _userSkillRepository.GetByUserIdAsync(userId);

            if (skills == null || !skills.Any())
                return new List<UserSkillDto>();

            return skills.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Save multiple skill records for a user
        /// </summary>
        /// <param name="dtoList">List of UserSkillDto to save</param>
        /// <param name="userId">The ID of the user</param>
        /// <returns>Saved list of UserSkillDto</returns>
        public async Task<List<UserSkillDto>> SaveUserSkillAsync(
            List<UserSkillDto> dtoList,
            int userId)
        {
            var result = new List<UserSkillDto>();

            foreach (var dto in dtoList)
            {
                dto.UserId = userId;

                var savedEntity = await SaveOrUpdateAsync(dto);

                result.Add(MapToDto(savedEntity));
            }

            return result;
        }

        /// <summary>
        /// Save or update a single UserSkill entity
        /// </summary>
        private async Task<UserSkill> SaveOrUpdateAsync(UserSkillDto dto)
        {
            UserSkill? entity = null;

            if (dto.Id > 0)
                entity = await _userSkillRepository.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                entity = new UserSkill
                {
                    UserId = dto.UserId,
                    SkillName = dto.SkillName,
                    //Level = dto.Level,
                    //SkillType = dto.SkillType,
                    Remarks = dto.Remarks,
                    YearsOfExperience = dto.YearsOfExperience,
                    CertificateFile = dto.CertificateFile,
                    Priority = dto.Priority,
                    ProficiencyPercentage = dto.ProficiencyPercentage
                };

                await _userSkillRepository.AddAsync(entity);
            }
            else
            {
                entity.SkillName = dto.SkillName;
                //entity.Level = dto.Level;
                //entity.SkillType = dto.SkillType;
                entity.Remarks = dto.Remarks;
                entity.YearsOfExperience = dto.YearsOfExperience;
                entity.CertificateFile = dto.CertificateFile;
                entity.Priority = dto.Priority;
                entity.ProficiencyPercentage = dto.ProficiencyPercentage;

                await _userSkillRepository.UpdateAsync(entity);
            }

            return entity;
        }

        /// <summary>
        /// Map UserSkill entity to DTO
        /// </summary>
        private static UserSkillDto MapToDto(UserSkill entity)
        {
            return new UserSkillDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                SkillName = entity.SkillName,
                //Level = entity.Level,
                //SkillType = entity.SkillType,
                Remarks = entity.Remarks,
                YearsOfExperience = entity.YearsOfExperience,
                CertificateFile = entity.CertificateFile,
                Priority = entity.Priority,
                ProficiencyPercentage = entity.ProficiencyPercentage
            };
        }
    }
}