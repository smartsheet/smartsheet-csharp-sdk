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
    /// Represents specific elements to include in a children response.
    /// </summary>
    public enum ChildrenInclusion
    {
        /// <summary>
        /// Adds the Source object indicating which object this resource was created from, if any.
        /// </summary>
        [EnumMember(Value = "source")]
        SOURCE,

        /// <summary>
        /// Returns the user with owner permissions, or the user with admin permissions if there is no owner assigned.
        /// If no owner or admins are assigned, the Plan Asset Admin is returned. If no Plan Asset Admin is assigned, the System Admin is returned.
        /// </summary>
        [EnumMember(Value = "ownerInfo")]
        OWNER_INFO
    }
}