using MYCV.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Application.Interfaces
{
    public interface IUserPersonalDetailService
    {
        Task<UserPersonalDetailDto> GetUserPersonalDetailAsync(int userId);
        Task<UserPersonalDetailDto> SaveUserPersonalDetailAsync(UserPersonalDetailDto dto);

    }
}