using MYCV.Application.DTOs;

namespace MYCV.Application.Interfaces
{
    public interface IUserReferenceService
    {
        /// <summary>
        /// Get all references records for a user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>List of UserReferenceDto</returns>
        Task<List<UserReferenceDto>> GetUserReferenceAsync(int userId);

        /// <summary>
        /// Save multiple reference records for a user
        /// </summary>
        /// <param name="dtoList">List of UserReferenceDto to save</param>
        /// <param name="userId">The ID of the user</param>
        /// <returns>Saved list of UserReferenceDto</returns>
        Task<List<UserReferenceDto>> SaveUserReferenceAsync(List<UserReferenceDto> dtoList, int userId);
    }
}