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
    public class UserEducationService : IUserEducationService
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

            return educations.Select(MapToDto).ToList();
        }

        public async Task<List<UserEducationDto>> SaveUserEducationsAsync(
            List<UserEducationDto> dtoList,
            int userId)
        {
            var result = new List<UserEducationDto>();

            foreach (var dto in dtoList)
            {
                dto.UserId = userId;

                var savedEntity = await SaveOrUpdateAsync(dto);

                result.Add(MapToDto(savedEntity));
            }

            return result;
        }

        private async Task<UserEducation> SaveOrUpdateAsync(UserEducationDto dto)
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

            return entity;
        }

        private static UserEducationDto MapToDto(UserEducation entity)
        {
            return new UserEducationDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                EducationLevel = entity.EducationLevel,
                ExamName = entity.ExamName,
                BoardOrUniversity = entity.BoardOrUniversity,
                GroupOrMajor = entity.GroupOrMajor,
                Result = entity.Result,
                PassingYear = entity.PassingYear,
                CertificateFile = entity.CertificateFile,
                Remarks = entity.Remarks
            };
        }
    }
}