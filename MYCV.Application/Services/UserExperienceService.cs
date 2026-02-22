using MYCV.Application.DTOs;
using MYCV.Application.Interfaces;
using MYCV.Domain.Entities;

namespace MYCV.Application.Services
{
    public class UserExperienceService : IUserExperienceService
    {
        private readonly IUserExperienceRepository _userExperienceRepository;

        public UserExperienceService(IUserExperienceRepository userExperienceRepository)
        {
            _userExperienceRepository = userExperienceRepository;
        }

        /// <summary>
        /// Get all work experience records for a user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>List of UserExperienceDto</returns>
        public async Task<List<UserExperienceDto>> GetUserExperiencesAsync(int userId)
        {
            var experiences = await _userExperienceRepository.GetByUserIdAsync(userId);

            if (experiences == null || !experiences.Any())
                return new List<UserExperienceDto>();

            return experiences.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Save multiple work experience records for a user
        /// </summary>
        /// <param name="dtoList">List of UserExperienceDto to save</param>
        /// <param name="userId">The ID of the user</param>
        /// <returns>Saved list of UserExperienceDto</returns>
        public async Task<List<UserExperienceDto>> SaveUserExperiencesAsync(
            List<UserExperienceDto> dtoList,
            int userId)
        {
            var result = new List<UserExperienceDto>();

            foreach (var dto in dtoList)
            {
                dto.UserId = userId;

                var savedEntity = await SaveOrUpdateAsync(dto);

                result.Add(MapToDto(savedEntity));
            }

            return result;
        }

        /// <summary>
        /// Save or update a single UserExperience entity
        /// </summary>
        private async Task<UserExperience> SaveOrUpdateAsync(UserExperienceDto dto)
        {
            UserExperience? entity = null;

            if (dto.Id > 0)
                entity = await _userExperienceRepository.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                entity = new UserExperience
                {
                    UserId = dto.UserId,
                    Company = dto.Company,
                    Position = dto.Position,
                    Department = dto.Department,
                    Location = dto.Location,
                    EmploymentType = dto.EmploymentType,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    Responsibilities = dto.Responsibilities,
                    Remarks = dto.Remarks,
                    ProjectLink = dto.ProjectLink,
                    Priority = dto.Priority
                };

                await _userExperienceRepository.AddAsync(entity);
            }
            else
            {
                entity.Company = dto.Company;
                entity.Position = dto.Position;
                entity.Department = dto.Department;
                entity.Location = dto.Location;
                entity.EmploymentType = dto.EmploymentType;
                entity.StartDate = dto.StartDate;
                entity.EndDate = dto.EndDate;
                entity.Responsibilities = dto.Responsibilities;
                entity.Remarks = dto.Remarks;
                entity.ProjectLink = dto.ProjectLink;
                entity.Priority = dto.Priority;

                await _userExperienceRepository.UpdateAsync(entity);
            }

            return entity;
        }

        /// <summary>
        /// Map UserExperience entity to DTO
        /// </summary>
        private static UserExperienceDto MapToDto(UserExperience entity)
        {
            return new UserExperienceDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Company = entity.Company,
                Position = entity.Position,
                Department = entity.Department,
                Location = entity.Location,
                EmploymentType = entity.EmploymentType,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Responsibilities = entity.Responsibilities,
                Remarks = entity.Remarks,
                ProjectLink = entity.ProjectLink,
                Priority = entity.Priority
            };
        }
    }
}