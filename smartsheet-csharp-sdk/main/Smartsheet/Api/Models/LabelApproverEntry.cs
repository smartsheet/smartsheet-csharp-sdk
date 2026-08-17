using System.Collections.Generic;

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Associates a classification label with its set of approvers for downgrade.
    /// </summary>
    public class LabelApproverEntry
    {
        /// <summary>
        /// Gets or sets the label ID this approver entry applies to.
        /// </summary>
        public string? LabelId { get; set; }

        /// <summary>
        /// Gets or sets the list of approver entries for this label.
        /// </summary>
        public IList<ApproverEntry>? Approvers { get; set; }
    }
}
