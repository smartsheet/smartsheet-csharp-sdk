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
    /// Represents a set of approvers (by type and IDs) for downgrade approval settings.
    /// </summary>
    public class DataClassificationApproverEntry
    {
        /// <summary>
        /// Gets or sets the approver type.
        /// </summary>
        public DataClassificationApproverType? Type { get; set; }

        /// <summary>
        /// Gets or sets the list of approver IDs.
        /// </summary>
        public IList<long>? Ids { get; set; }
    }
}
