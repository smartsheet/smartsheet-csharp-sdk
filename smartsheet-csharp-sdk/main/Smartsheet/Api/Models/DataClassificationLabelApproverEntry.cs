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
    /// Associates a classification label with its set of approvers for downgrade.
    /// </summary>
    public class DataClassificationLabelApproverEntry
    {
        /// <summary>
        /// Gets or sets the label ID this approver entry applies to.
        /// </summary>
        public string? LabelId { get; set; }

        /// <summary>
        /// Gets or sets the list of approver entries for this label.
        /// </summary>
        public IList<DataClassificationApproverEntry>? Approvers { get; set; }
    }
}
