using MYCV.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Application.Interfaces
{
    public interface IUserEducationService
    {
        Task<List<UserEducationDto>> GetUserEducationAsync(int userId);

        Task<List<UserEducationDto>> SaveUserEducationsAsync(
            List<UserEducationDto> dtoList,
            int userId);
    }
}