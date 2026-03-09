using MYCV.Domain.Entities;

namespace MYCV.Application.Interfaces
{
    public interface IUserProjectRepository
    {
        Task<UserProject?> GetByIdAsync(int id);
        Task<List<UserProject>> GetByUserIdAsync(int userId);
        Task AddAsync(UserProject project);
        Task UpdateAsync(UserProject project);
        Task DeleteAsync(UserProject project);
    }
}