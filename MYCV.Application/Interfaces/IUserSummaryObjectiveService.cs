using MYCV.Application.DTOs;

namespace MYCV.Application.Interfaces
{
    public interface IUserSummaryObjectiveService
    {
        /// <summary>
        /// Get all SummaryObjective records for a user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>List of UserSummaryObjectiveDto</returns>
        Task<List<UserSummaryObjectiveDto>> GetUserSummaryObjectiveAsync(int userId);

        /// <summary>
        /// Save multiple SummaryObjective records for a user
        /// </summary>
        /// <param name="dtoList">List of UserSummaryObjectiveDto to save</param>
        /// <param name="userId">The ID of the user</param>
        /// <returns>Saved list of UserSummaryObjectiveDto</returns>
        Task<List<UserSummaryObjectiveDto>> SaveUserSummaryObjectiveAsync(List<UserSummaryObjectiveDto> dtoList, int userId);
    }
}