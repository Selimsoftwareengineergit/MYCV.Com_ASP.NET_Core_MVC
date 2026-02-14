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
    public class UserExperienceService : IUserExperienceService
    {
        private readonly IUserExperienceRepository _userExperienceRepository;
        public UserExperienceService(IUserExperienceRepository userExperienceRepository)
        {
            _userExperienceRepository = userExperienceRepository;
        }

        public async Task<List<UserExperienceDto>> GetUserExperienceAsync(int userId)
        {
            var experiences = await _userExperienceRepository.GetByUserIdAsync(userId);

            if (experiences == null || !experiences.Any())
                return new List<UserExperienceDto>();

            return experiences.Select(e => new UserExperienceDto
            {
                Id = e.Id,
                UserId = e.UserId,
                Company = e.Company,
                Position = e.Position,
                Department = e.Department,
                Location = e.Location,
                EmploymentType = e.EmploymentType,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Responsibilities = e.Responsibilities,
                Remarks = e.Remarks,
                ProjectLink = e.ProjectLink,
                Priority = e.Priority
            }).ToList();
        }

        public async Task<UserExperienceDto> SaveUserExperienceAsync(UserExperienceDto dto)
        {
            UserExperience? entity = null;

            if (dto.Id > 0)
                entity = await _userExperienceRepository.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                entity = new UserExperience
                {
                    UserId = dto.UserId,
                    Company = dto.Company,
                    Position = dto.Position,
                    Department = dto.Department,
                    Location = dto.Location,
                    EmploymentType = dto.EmploymentType,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    Responsibilities = dto.Responsibilities,
                    Remarks = dto.Remarks,
                    ProjectLink = dto.ProjectLink,
                    Priority = dto.Priority
                };

                await _userExperienceRepository.AddAsync(entity);
            }
            else
            {
                entity.Company = dto.Company;
                entity.Position = dto.Position;
                entity.Department = dto.Department;
                entity.Location = dto.Location;
                entity.EmploymentType = dto.EmploymentType;
                entity.StartDate = dto.StartDate;
                entity.EndDate = dto.EndDate;
                entity.Responsibilities = dto.Responsibilities;
                entity.Remarks = dto.Remarks;
                entity.ProjectLink = dto.ProjectLink;
                entity.Priority = dto.Priority;

                await _userExperienceRepository.UpdateAsync(entity);
            }

            dto.Id = entity.Id;
            return dto;
        }
    }
}