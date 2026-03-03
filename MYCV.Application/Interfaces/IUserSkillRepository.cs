using MYCV.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Application.Interfaces
{
    public interface IUserSkillRepository
    {
        Task<UserSkill?> GetByIdAsync(int id);
        Task<List<UserSkill>> GetByUserIdAsync(int userId);
        Task AddAsync(UserSkill skill);
        Task UpdateAsync(UserSkill skill);
        Task DeleteAsync(UserSkill skill);
    }
}