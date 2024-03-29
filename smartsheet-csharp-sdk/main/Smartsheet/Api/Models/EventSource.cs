﻿//    #[license]
//    SmartsheetClient SDK for C#
//    %%
//    Copyright (C) 2019 SmartsheetClient
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
    /// Enum holding source of an event.
    /// </summary>
    public enum EventSource
    {
        /// <summary>
        /// Smartsheet web application.
        /// </summary>
        WEB_APP,
        /// <summary>
        /// Mobile IOS device.
        /// </summary>
        MOBILE_IOS,
        /// <summary>
        /// Mobile android device.
        /// </summary>
        MOBILE_ANDROID,
        /// <summary>
        /// API integrated application.
        /// </summary>
        API_INTEGRATED_APP,
        /// <summary>
        /// API through undefined application.
        /// </summary>
        API_UNDEFINED_APP,
        /// <summary>
        /// Not known at this point.
        /// </summary>
        UNKNOWN
    }
}