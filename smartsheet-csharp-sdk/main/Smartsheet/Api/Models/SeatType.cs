
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

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Represents all possible seat types for users in Smartsheet.
    /// </summary>
    public enum SeatType
    {
        /// <summary>
        /// Viewer seat type - can view sheets but has limited permissions
        /// </summary>
        VIEWER,

        /// <summary>
        /// Guest seat type - external user with limited access
        /// </summary>
        GUEST,

        /// <summary>
        /// Member seat type - full user with standard permissions
        /// </summary>
        MEMBER,

        /// <summary>
        /// Provisional member seat type - member with trial access
        /// </summary>
        PROVISIONAL_MEMBER
    }

    /// <summary>
    /// Represents seat types that can be used for downgrading users.
    /// </summary>
    public enum DowngradeSeatType
    {
        /// <summary>
        /// Downgrade to viewer seat type
        /// </summary>
        VIEWER,

        /// <summary>
        /// Downgrade to guest seat type
        /// </summary>
        GUEST
    }

    /// <summary>
    /// Represents seat types that can be used for upgrading users.
    /// </summary>
    public enum UpgradeSeatType
    {
        /// <summary>
        /// Upgrade to guest seat type
        /// </summary>
        GUEST,

        /// <summary>
        /// Upgrade to member seat type
        /// </summary>
        MEMBER
    }
}
