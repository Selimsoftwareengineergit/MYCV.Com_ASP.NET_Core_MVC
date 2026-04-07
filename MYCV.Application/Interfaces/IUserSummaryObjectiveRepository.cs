using MYCV.Domain.Entities;

namespace MYCV.Application.Interfaces
{
    public interface IUserSummaryObjectiveRepository
    {
        Task<UserSummaryObjective?> GetByIdAsync(int id);
        Task<List<UserSummaryObjective>> GetByUserIdAsync(int userId);
        Task AddAsync(UserSummaryObjective summaryObjective);
        Task UpdateAsync(UserSummaryObjective summaryObjective);
        Task DeleteAsync(UserSummaryObjective summaryObjective);
    }
}