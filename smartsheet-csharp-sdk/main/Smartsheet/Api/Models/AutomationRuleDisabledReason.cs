//    #[license]
//    SmartsheetClient SDK for C#
//    %%
//    Copyright (C) 2018 SmartsheetClient
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Enum for possible reasons for automation rules to be disabled
    /// </summary>
    public enum AutomationRuleDisabledReason
    {
        /// <summary>
        /// Automation not enabled for the current org
        /// </summary>
        AUTOMATION_NOT_ENABLED_FOR_ORG,
        /// <summary>
        /// Missing a column
        /// </summary>
        COLUMN_MISSING,
        /// <summary>
        /// Type of column is incompatible
        /// </summary>
        COLUMN_TYPE_INCOMPATIBLE,
        /// <summary>
        /// No recipients 
        /// </summary>
        NO_POTENTIAL_RECIPIENTS,
        /// <summary>
        /// No columns selected that are valid
        /// </summary>
        NO_VALID_SELECTED_COLUMNS,
        /// <summary>
        /// Missing an approval column
        /// </summary>
        APPROVAL_COLUMN_MISSING,
        /// <summary>
        /// Approval column is of wrong type
        /// </summary>
        APPROVAL_COLUMN_WRONG_TYPE
    }
}
