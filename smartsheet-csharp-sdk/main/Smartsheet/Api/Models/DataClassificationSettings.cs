//    #[license]
//    SmartsheetClient SDK for C#
//    %%
//    Copyright (C) 2014 SmartsheetClient
//    %%
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//        
//            http://www.apache.org/licenses/LICENSE-2.0
//        
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//    %[license]

using System.Collections.Generic;

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Represents the data classification settings for a plan.
    /// Returned by GET /2.0/governance/data-classification/settings.
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
        public IList<DataClassificationLabel>? Labels { get; set; }

        /// <summary>
        /// Gets or sets the downgrade approval settings.
        /// </summary>
        public DataClassificationDowngradeApprovalSettings? DowngradeApprovalSettings { get; set; }
    }
}
