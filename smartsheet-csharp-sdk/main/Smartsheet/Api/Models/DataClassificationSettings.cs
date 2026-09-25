using System.Collections.Generic;

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Represents the data classification settings for a plan.
    /// Returned by GET /2.0/governance/data-classification/settings?planId={planId}.
    /// </summary>
    public class DataClassificationSettings
    {
        /// <summary>
        /// Gets or sets the organization ID.
        /// </summary>
        public long? OrgId { get; set; }

        /// <summary>
        /// Gets or sets the plan ID.
        /// </summary>
        public long? PlanId { get; set; }

        /// <summary>
        /// Gets or sets whether data classification is disabled for this plan.
        /// </summary>
        public bool? IsDisabled { get; set; }

        /// <summary>
        /// Gets or sets the URL to the classification guidelines.
        /// </summary>
        public string? GuidelinesUrl { get; set; }

        /// <summary>
        /// Gets or sets whether users are allowed to manually change their classification.
        /// </summary>
        public bool? AllowManualChange { get; set; }

        /// <summary>
        /// Gets or sets the list of classification labels defined for this plan.
        /// </summary>
        public IList<ClassificationLabel>? Labels { get; set; }

        /// <summary>
        /// Gets or sets the downgrade approval settings.
        /// </summary>
        public DowngradeApprovalSettings? DowngradeApprovalSettings { get; set; }
    }
}
