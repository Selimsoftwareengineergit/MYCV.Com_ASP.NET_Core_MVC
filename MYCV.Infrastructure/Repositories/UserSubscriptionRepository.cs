using Microsoft.EntityFrameworkCore;
using MYCV.Application.Interfaces;
using MYCV.Domain.Entities;
using MYCV.Infrastructure.Data;

namespace MYCV.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing user subscriptions
    /// </summary>
    public class UserSubscriptionRepository : IUserSubscriptionRepository
    {
        private readonly MyCvDbContext _context;

        public UserSubscriptionRepository(MyCvDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get subscription by its unique ID
        /// </summary>
        /// <param name="id">Subscription ID</param>
        /// <returns>UserSubscription or null if not found</returns>
        public async Task<UserSubscription?> GetByIdAsync(int id)
        {
            return await _context.UserSubscriptions
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        /// <summary>
        /// Get all subscriptions for a specific user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of UserSubscription</returns>
        public async Task<List<UserSubscription>> GetByUserIdAsync(int userId)
        {
            return await _context.UserSubscriptions
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Add a new subscription
        /// </summary>
        /// <param name="subscription">Subscription entity to add</param>
        public async Task AddAsync(UserSubscription subscription)
        {
            await _context.UserSubscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Update an existing subscription
        /// </summary>
        /// <param name="subscription">Subscription entity to update</param>
        public async Task UpdateAsync(UserSubscription subscription)
        {
            _context.UserSubscriptions.Update(subscription);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete a subscription (hard delete)
        /// </summary>
        /// <param name="subscription">Subscription entity to delete</param>
        public async Task DeleteAsync(UserSubscription subscription)
        {
            _context.UserSubscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
        }
    }
}