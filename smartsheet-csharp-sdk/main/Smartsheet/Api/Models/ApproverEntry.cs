using System.Collections.Generic;

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Represents a set of approvers (by type and IDs) for downgrade approval settings.
    /// </summary>
    public class ApproverEntry
    {
        /// <summary>
        /// Gets or sets the approver type (e.g. "USERS", "GROUPS", "WORKSPACE_ADMINS").
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the list of approver IDs.
        /// </summary>
        public IList<long>? Ids { get; set; }
    }
}
