using MYCV.Domain.Entities;

namespace MYCV.Application.Interfaces
{
    public interface IUserReferenceRepository
    {
        Task<UserReference?> GetByIdAsync(int id);
        Task<List<UserReference>> GetByUserIdAsync(int userId);
        Task AddAsync(UserReference reference);
        Task UpdateAsync(UserReference reference);
        Task DeleteAsync(UserReference reference);
    }
}