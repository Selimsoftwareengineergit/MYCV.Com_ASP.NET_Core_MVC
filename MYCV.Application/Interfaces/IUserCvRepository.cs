using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Application.Interfaces
{
    public interface IUserCvRepository
    {
        Task<UserCv?> GetByEmailAsync(string email);
        Task<UserCv?> GetByIdAsync(int id);
        Task AddAsync(UserCv userCv);
        Task UpdateAsync(UserCv userCv);
    }
}