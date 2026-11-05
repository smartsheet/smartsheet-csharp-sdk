using System.Collections.Generic;

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Represents the downgrade approval settings for a data classification plan.
    /// </summary>
    public class DataClassificationDowngradeApprovalSettings
    {
        /// <summary>
        /// Gets or sets the approval mode.
        /// </summary>
        public DataClassificationDowngradeApprovalMode? Mode { get; set; }

        /// <summary>
        /// Gets or sets the global approver entries.
        /// </summary>
        public IList<DataClassificationApproverEntry>? Approvers { get; set; }

        /// <summary>
        /// Gets or sets the per-label approver entries.
        /// </summary>
        public IList<DataClassificationLabelApproverEntry>? LabelApprovers { get; set; }
    }
}
