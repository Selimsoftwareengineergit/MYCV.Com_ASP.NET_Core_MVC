using MYCV.Domain.Entities;
using MYCV.Domain.Enums;

namespace MYCV.Application.DTOs
{
    public class UserSubscriptionDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public SubscriptionPlan Plan { get; set; } = SubscriptionPlan.Monthly;

        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        public DateTime EndDate { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string? PaymentTransactionId { get; set; }

        public string? Remarks { get; set; }

        public bool IsExpired => DateTime.UtcNow > EndDate;

        public decimal Amount => UserSubscription.GetAmountByPlan(Plan);
    }
}