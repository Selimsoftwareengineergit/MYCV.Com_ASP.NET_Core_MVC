using MYCV.Application.DTOs;
using MYCV.Application.Interfaces;
using MYCV.Domain.Entities;

namespace MYCV.Application.Services
{
    public class UserReferenceService : IUserReferenceService
    {
        private readonly IUserReferenceRepository _userReferenceRepository;

        public UserReferenceService(IUserReferenceRepository userReferenceRepository)
        {
            _userReferenceRepository = userReferenceRepository;
        }

        /// <summary>
        /// Get all reference records for a user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>List of UserReferenceDto</returns>
        public async Task<List<UserReferenceDto>> GetUserReferenceAsync(int userId)
        {
            var references = await _userReferenceRepository.GetByUserIdAsync(userId);

            if (references == null || !references.Any())
                return new List<UserReferenceDto>();

            return references.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Save multiple reference records for a user
        /// </summary>
        /// <param name="dtoList">List of UserReferenceDto</param>
        /// <param name="userId">The ID of the user</param>
        /// <returns>Saved list of UserReferenceDto</returns>
        public async Task<List<UserReferenceDto>> SaveUserReferenceAsync(
            List<UserReferenceDto> dtoList,
            int userId)
        {
            var result = new List<UserReferenceDto>();

            foreach (var dto in dtoList)
            {
                dto.UserId = userId;

                var savedEntity = await SaveOrUpdateAsync(dto);

                result.Add(MapToDto(savedEntity));
            }

            return result;
        }

        /// <summary>
        /// Save or update a single UserReference entity
        /// </summary>
        private async Task<UserReference> SaveOrUpdateAsync(UserReferenceDto dto)
        {
            UserReference? entity = null;

            if (dto.Id > 0)
                entity = await _userReferenceRepository.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                entity = new UserReference
                {
                    UserId = dto.UserId,
                    Name = dto.Name,
                    Position = dto.Position,
                    Company = dto.Company,
                    Contact = dto.Contact,
                    Relation = dto.Relation,
                    Priority = dto.Priority
                };

                await _userReferenceRepository.AddAsync(entity);
            }
            else
            {
                entity.Name = dto.Name;
                entity.Position = dto.Position;
                entity.Company = dto.Company;
                entity.Contact = dto.Contact;
                entity.Relation = dto.Relation;
                entity.Priority = dto.Priority;

                await _userReferenceRepository.UpdateAsync(entity);
            }

            return entity;
        }

        /// <summary>
        /// Map UserReference entity to DTO
        /// </summary>
        private static UserReferenceDto MapToDto(UserReference entity)
        {
            return new UserReferenceDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Name = entity.Name,
                Position = entity.Position,
                Company = entity.Company,
                Contact = entity.Contact,
                Relation = entity.Relation,
                Priority = entity.Priority
            };
        }
    }
}