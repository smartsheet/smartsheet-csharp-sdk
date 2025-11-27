using System;

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Represents a user's plan information.
    /// </summary>
    public class UserPlan
    {
        /// <summary>
        /// Gets or sets the plan ID.
        /// </summary>
        public long PlanId { get; set; }

        /// <summary>
        /// Gets or sets the seat type.
        /// </summary>
        public SeatType SeatType { get; set; }

        /// <summary>
        /// Gets or sets the seat type last changed at timestamp.
        /// </summary>
        public DateTime? SeatTypeLastChangedAt { get; set; }

        /// <summary>
        /// Gets or sets the expiration date of the provisional seat type of the user.
        /// </summary>
        public DateTime? ProvisionalExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets whether the user is internal.
        /// </summary>
        public bool IsInternal { get; set; }
    }
}