using MYCV.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Application.Interfaces
{
    public interface IUserExperienceService
    {
        /// <summary>
        /// Get all work experience records for a user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>List of UserExperienceDto</returns>
        Task<List<UserExperienceDto>> GetUserExperiencesAsync(int userId);

        /// <summary>
        /// Save multiple work experience records for a user
        /// </summary>
        /// <param name="dtoList">List of UserExperienceDto to save</param>
        /// <param name="userId">The ID of the user</param>
        /// <returns>Saved list of UserExperienceDto</returns>
        Task<List<UserExperienceDto>> SaveUserExperiencesAsync(List<UserExperienceDto> dtoList, int userId);
    }
}