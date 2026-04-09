using MYCV.Domain.Entities;

namespace MYCV.Application.Interfaces
{
    public interface IUserSubscriptionRepository
    {
        Task<UserSubscription?> GetByIdAsync(int id);
        Task<List<UserSubscription>> GetByUserIdAsync(int userId);
        Task AddAsync(UserSubscription subscription);
        Task UpdateAsync(UserSubscription subscription);
        Task DeleteAsync(UserSubscription subscription);
    }
}