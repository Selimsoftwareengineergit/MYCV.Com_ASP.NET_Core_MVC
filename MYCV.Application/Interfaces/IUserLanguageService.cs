using MYCV.Application.DTOs;

namespace MYCV.Application.Interfaces
{
    public interface IUserLanguageService
    {
        /// <summary>
        /// Get all language records for a user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>List of UserLanguageDto</returns>
        Task<List<UserLanguageDto>> GetUserLanguageAsync(int userId);

        /// <summary>
        /// Save multiple language records for a user
        /// </summary>
        /// <param name="dtoList">List of UserLanguageDto to save</param>
        /// <param name="userId">The ID of the user</param>
        /// <returns>Saved list of UserLanguageDto</returns>
        Task<List<UserLanguageDto>> SaveUserLanguageAsync(List<UserLanguageDto> dtoList, int userId);
    }
}