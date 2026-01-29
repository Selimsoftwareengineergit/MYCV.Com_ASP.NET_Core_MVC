using MYCV.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Application.Interfaces
{
    public interface IUserEducationRepository
    {
        Task<List<UserEducation>> GetByUserCvIdAsync(int userCvId);
        Task<List<UserEducation>> GetByUserIdAsync(int userId);
        Task<UserEducation?> GetByIdAsync(int id);
        Task AddAsync(UserEducation education);
        Task UpdateAsync(UserEducation education);
        Task DeleteAsync(UserEducation education);
    }
}