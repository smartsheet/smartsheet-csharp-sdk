using System.Collections.Generic;

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Represents the downgrade approval settings for a data classification plan.
    /// </summary>
    public class DowngradeApprovalSettings
    {
        /// <summary>
        /// Gets or sets the approval mode (e.g. "NONE", "CUSTOM").
        /// </summary>
        public string? Mode { get; set; }

        /// <summary>
        /// Gets or sets the global approver entries.
        /// </summary>
        public IList<ApproverEntry>? Approvers { get; set; }

        /// <summary>
        /// Gets or sets the per-label approver entries.
        /// </summary>
        public IList<LabelApproverEntry>? LabelApprovers { get; set; }
    }
}
