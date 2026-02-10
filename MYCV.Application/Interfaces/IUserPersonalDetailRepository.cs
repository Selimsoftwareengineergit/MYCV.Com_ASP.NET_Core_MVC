using MYCV.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Application.Interfaces
{
    public interface IUserPersonalDetailRepository
    {
        Task<UserPersonalDetail?> GetByUserIdAsync(int userId);
        Task<UserPersonalDetail?> GetByIdAsync(int id);
        Task AddAsync(UserPersonalDetail userCv);
        Task UpdateAsync(UserPersonalDetail userCv);
    }
}