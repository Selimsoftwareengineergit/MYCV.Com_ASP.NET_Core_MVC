using MYCV.Application.DTOs;
using MYCV.Application.Interfaces;
using MYCV.Domain.Entities;

namespace MYCV.Application.Services
{
    public class UserProjectService : IUserProjectService
    {
        private readonly IUserProjectRepository _userProjectRepository;

        public UserProjectService(IUserProjectRepository userProjectRepository)
        {
            _userProjectRepository = userProjectRepository;
        }

        /// <summary>
        /// Get all project records for a user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>List of UserProjectDto</returns>
        public async Task<List<UserProjectDto>> GetUserProjectAsync(int userId)
        {
            var projects = await _userProjectRepository.GetByUserIdAsync(userId);

            if (projects == null || !projects.Any())
                return new List<UserProjectDto>();

            return projects.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Save multiple project records for a user
        /// </summary>
        /// <param name="dtoList">List of UserProjectDto to save</param>
        /// <param name="userId">The ID of the user</param>
        /// <returns>Saved list of UserProjectDto</returns>
        public async Task<List<UserProjectDto>> SaveUserProjectAsync(
            List<UserProjectDto> dtoList,
            int userId)
        {
            var result = new List<UserProjectDto>();

            foreach (var dto in dtoList)
            {
                dto.UserId = userId;

                var savedEntity = await SaveOrUpdateAsync(dto);

                result.Add(MapToDto(savedEntity));
            }

            return result;
        }

        /// <summary>
        /// Save or update a single UserProject entity
        /// </summary>
        private async Task<UserProject> SaveOrUpdateAsync(UserProjectDto dto)
        {
            UserProject? entity = null;

            if (dto.Id > 0)
                entity = await _userProjectRepository.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                entity = new UserProject
                {
                    UserId = dto.UserId,
                    ProjectTitle = dto.ProjectTitle,
                    Role = dto.Role,
                    ProjectType = dto.ProjectType,
                    Description = dto.Description,
                    Technologies = dto.Technologies,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    ProjectLink = dto.ProjectLink,
                    Remarks = dto.Remarks,
                    Priority = dto.Priority
                };

                await _userProjectRepository.AddAsync(entity);
            }
            else
            {
                entity.ProjectTitle = dto.ProjectTitle;
                entity.Role = dto.Role;
                entity.ProjectType = dto.ProjectType;
                entity.Description = dto.Description;
                entity.Technologies = dto.Technologies;
                entity.StartDate = dto.StartDate;
                entity.EndDate = dto.EndDate;
                entity.ProjectLink = dto.ProjectLink;
                entity.Remarks = dto.Remarks;
                entity.Priority = dto.Priority;

                await _userProjectRepository.UpdateAsync(entity);
            }

            return entity;
        }

        /// <summary>
        /// Map UserProject entity to DTO
        /// </summary>
        private static UserProjectDto MapToDto(UserProject entity)
        {
            return new UserProjectDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                ProjectTitle = entity.ProjectTitle,
                Role = entity.Role,
                ProjectType = entity.ProjectType,
                Description = entity.Description,
                Technologies = entity.Technologies,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                ProjectLink = entity.ProjectLink,
                Remarks = entity.Remarks,
                Priority = entity.Priority
            };
        }
    }
}