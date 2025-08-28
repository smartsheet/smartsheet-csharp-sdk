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

using System.Runtime.Serialization;

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Represents the types of child resources that can be included in children endpoints.
    /// </summary>
    public enum ChildrenResourceType
    {
        /// <summary>
        /// Include sheets in the response.
        /// </summary>
        [EnumMember(Value = "sheets")]
        SHEETS,

        /// <summary>
        /// Include reports in the response.
        /// </summary>
        [EnumMember(Value = "reports")]
        REPORTS,

        /// <summary>
        /// Include sights (dashboards) in the response.
        /// </summary>
        [EnumMember(Value = "sights")]
        SIGHTS,

        /// <summary>
        /// Include folders in the response.
        /// </summary>
        [EnumMember(Value = "folders")]
        FOLDERS
    }
}