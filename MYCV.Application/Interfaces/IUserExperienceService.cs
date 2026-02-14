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
        Task<List<UserExperienceDto>> GetUserExperienceAsync(int userId);
        Task<UserExperienceDto> SaveUserExperienceAsync(UserExperienceDto dto);
    }
}