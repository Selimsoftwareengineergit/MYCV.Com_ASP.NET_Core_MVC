using MYCV.Application.DTOs;
using MYCV.Application.Interfaces;
using MYCV.Domain.Entities;

namespace MYCV.Application.Services
{
    /// <summary>
    /// Service for managing user subscriptions
    /// </summary>
    public class UserSubscriptionService : IUserSubscriptionService
    {
        private readonly IUserSubscriptionRepository _userSubscriptionRepository;

        public UserSubscriptionService(IUserSubscriptionRepository userSubscriptionRepository)
        {
            _userSubscriptionRepository = userSubscriptionRepository;
        }

        /// <summary>
        /// Get subscription details for a specific user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>UserSubscriptionDto if found, otherwise null</returns>
        public async Task<UserSubscriptionDto?> GetUserSubscriptionAsync(int userId)
        {
            var subscriptions = await _userSubscriptionRepository.GetByUserIdAsync(userId);

            var activeSubscription = subscriptions
                .OrderByDescending(s => s.StartDate)
                .FirstOrDefault();

            return activeSubscription == null ? null : MapToDto(activeSubscription);
        }

        /// <summary>
        /// Save or update subscription information for a user
        /// </summary>
        /// <param name="dto">Subscription details to save</param>
        /// <returns>The saved UserSubscriptionDto with updated information</returns>
        public async Task<UserSubscriptionDto> SaveUserSubscriptionAsync(UserSubscriptionDto dto)
        {
            UserSubscription? entity = null;

            if (dto.Id > 0)
                entity = await _userSubscriptionRepository.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                entity = new UserSubscription
                {
                    UserId = dto.UserId,
                    Plan = dto.Plan,
                    StartDate = dto.StartDate,
                    PaymentMethod = dto.PaymentMethod,
                    PaymentTransactionId = dto.PaymentTransactionId,
                    Remarks = dto.Remarks
                };

                entity.CalculateEndDate();

                await _userSubscriptionRepository.AddAsync(entity);
            }
            else
            {
                entity.Plan = dto.Plan;
                entity.StartDate = dto.StartDate;
                entity.PaymentMethod = dto.PaymentMethod;
                entity.PaymentTransactionId = dto.PaymentTransactionId;
                entity.Remarks = dto.Remarks;

                entity.CalculateEndDate();

                await _userSubscriptionRepository.UpdateAsync(entity);
            }

            return MapToDto(entity);
        }

        /// <summary>
        /// Map UserSubscription entity to DTO
        /// </summary>
        private static UserSubscriptionDto MapToDto(UserSubscription entity)
        {
            return new UserSubscriptionDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Plan = entity.Plan,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                PaymentMethod = entity.PaymentMethod,
                PaymentTransactionId = entity.PaymentTransactionId,
                Remarks = entity.Remarks
            };
        }
    }
}