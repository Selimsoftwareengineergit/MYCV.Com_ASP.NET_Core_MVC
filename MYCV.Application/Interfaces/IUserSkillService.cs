using MYCV.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Application.Interfaces
{
    public interface IUserSkillService
    {
        /// <summary>
        /// Get all skill records for a user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>List of UserSkillDto</returns>
        Task<List<UserSkillDto>> GetUserSkillAsync(int userId);

        /// <summary>
        /// Save multiple skill records for a user
        /// </summary>
        /// <param name="dtoList">List of UserskillDto to save</param>
        /// <param name="userId">The ID of the user</param>
        /// <returns>Saved list of UserSkillDto</returns>
        Task<List<UserSkillDto>> SaveUserSkillAsync(List<UserSkillDto> dtoList, int userId);
    }
}