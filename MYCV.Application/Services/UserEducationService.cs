using MYCV.Application.DTOs;
using MYCV.Application.Interfaces;
using MYCV.Domain.Entities;
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
                UserId = e.UserId,
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

        public async Task<UserEducationDto> SaveUserEducationAsync(UserEducationDto dto)
        {
            UserEducation? entity = null;

            if (dto.Id > 0)
                entity = await _userEducationRepository.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                entity = new UserEducation
                {
                    UserId = dto.UserId, 
                    EducationLevel = dto.EducationLevel,
                    ExamName = dto.ExamName,
                    BoardOrUniversity = dto.BoardOrUniversity,
                    GroupOrMajor = dto.GroupOrMajor,
                    Result = dto.Result,
                    PassingYear = dto.PassingYear,
                    CertificateFile = dto.CertificateFile,
                    Remarks = dto.Remarks
                };

                await _userEducationRepository.AddAsync(entity);
            }
            else
            {
                entity.EducationLevel = dto.EducationLevel;
                entity.ExamName = dto.ExamName;
                entity.BoardOrUniversity = dto.BoardOrUniversity;
                entity.GroupOrMajor = dto.GroupOrMajor;
                entity.Result = dto.Result;
                entity.PassingYear = dto.PassingYear;
                entity.CertificateFile = dto.CertificateFile;
                entity.Remarks = dto.Remarks;

                await _userEducationRepository.UpdateAsync(entity);
            }

            dto.Id = entity.Id;
            return dto;
        }
    }
}