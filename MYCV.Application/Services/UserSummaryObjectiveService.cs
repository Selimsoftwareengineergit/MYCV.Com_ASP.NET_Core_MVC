using MYCV.Application.DTOs;
using MYCV.Application.Interfaces;
using MYCV.Domain.Entities;

namespace MYCV.Application.Services
{
    public class UserSummaryObjectiveService : IUserSummaryObjectiveService
    {
        private readonly IUserSummaryObjectiveRepository _userSummaryObjectiveRepository;

        public UserSummaryObjectiveService(IUserSummaryObjectiveRepository userSummaryObjectiveRepository)
        {
            _userSummaryObjectiveRepository = userSummaryObjectiveRepository;
        }

        /// <summary>
        /// Get all summary & objective records for a user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>List of UserSummaryObjectiveDto</returns>
        public async Task<List<UserSummaryObjectiveDto>> GetUserSummaryObjectiveAsync(int userId)
        {
            var summaryObjective = await _userSummaryObjectiveRepository.GetByUserIdAsync(userId);

            if (summaryObjective == null || !summaryObjective.Any())
                return new List<UserSummaryObjectiveDto>();

            return summaryObjective.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Save multiple summary & objective records for a user
        /// </summary>
        /// <param name="dtoList">List of UserSummaryObjectiveDto</param>
        /// <param name="userId">The ID of the user</param>
        /// <returns>Saved list of UserSummaryObjectiveDto</returns>
        public async Task<List<UserSummaryObjectiveDto>> SaveUserSummaryObjectiveAsync(
            List<UserSummaryObjectiveDto> dtoList,
            int userId)
        {
            var result = new List<UserSummaryObjectiveDto>();

            foreach (var dto in dtoList)
            {
                dto.UserId = userId;

                var savedEntity = await SaveOrUpdateAsync(dto);

                result.Add(MapToDto(savedEntity));
            }

            return result;
        }

        /// <summary>
        /// Save or update a single UserSummaryObjective entity
        /// </summary>
        private async Task<UserSummaryObjective> SaveOrUpdateAsync(UserSummaryObjectiveDto dto)
        {
            UserSummaryObjective? entity = null;

            if (dto.Id > 0)
                entity = await _userSummaryObjectiveRepository.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                entity = new UserSummaryObjective
                {
                    UserId = dto.UserId,
                    Summary = dto.Summary,
                    Objective = dto.Objective,
                    Priority = dto.Priority
                };

                await _userSummaryObjectiveRepository.AddAsync(entity);
            }
            else
            {
                entity.Summary = dto.Summary;
                entity.Objective = dto.Objective;
                entity.Priority = dto.Priority;

                await _userSummaryObjectiveRepository.UpdateAsync(entity);
            }

            return entity;
        }

        /// <summary>
        /// Map UserSummaryObjective entity to DTO
        /// </summary>
        private static UserSummaryObjectiveDto MapToDto(UserSummaryObjective entity)
        {
            return new UserSummaryObjectiveDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Summary = entity.Summary,
                Objective = entity.Objective,
                Priority = entity.Priority
            };
        }
    }
}