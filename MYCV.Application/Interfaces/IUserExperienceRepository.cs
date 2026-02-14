using MYCV.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Application.Interfaces
{
    public interface IUserExperienceRepository
    {
        Task<UserExperience?> GetByIdAsync(int id);
        Task<List<UserExperience>> GetByUserIdAsync(int userId);
        Task AddAsync(UserExperience experience);
        Task UpdateAsync(UserExperience experience);
        Task DeleteAsync(UserExperience experience);
    }
}