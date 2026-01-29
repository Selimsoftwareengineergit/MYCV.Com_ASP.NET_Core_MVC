using MYCV.Application.DTOs;
using MYCV.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Application.Services
{
    public class UserEducationService: IUserEducationService
    {
        private readonly IUserEducationRepository _userEducationRepository;
        public UserEducationService(IUserEducationRepository userEducationRepository)
        {
            _userEducationRepository = userEducationRepository;
        }

        public async Task<List<UserEducationDto>> GetUserEducationAsync(int userId)
        {
            var educations = await _userEducationRepository.GetByUserIdAsync(userId);

            if (educations == null || !educations.Any())
                return new List<UserEducationDto>();

            return educations.Select(e => new UserEducationDto
            {
                Id = e.Id,
                UserCvId = e.UserCvId,
                EducationLevel = e.EducationLevel,
                ExamName = e.ExamName,
                BoardOrUniversity = e.BoardOrUniversity,
                GroupOrMajor = e.GroupOrMajor,
                Result = e.Result,
                PassingYear = e.PassingYear,
                CertificateFile = e.CertificateFile,
                Remarks = e.Remarks
            }).ToList();
        }
    }
}