using MYCV.Application.DTOs;

namespace MYCV.Application.Interfaces
{
    /// <summary>
    /// Service interface for managing user subscriptions
    /// </summary>
    public interface IUserSubscriptionService
    {
        /// <summary>
        /// Get subscription details for a specific user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>UserSubscriptionDto if found, otherwise null</returns>
        Task<UserSubscriptionDto?> GetUserSubscriptionAsync(int userId);

        /// <summary>
        /// Save or update subscription information for a user
        /// </summary>
        /// <param name="dto">Subscription details to save</param>
        /// <returns>The saved UserSubscriptionDto with updated information</returns>
        Task<UserSubscriptionDto> SaveUserSubscriptionAsync(UserSubscriptionDto dto);
    }
}