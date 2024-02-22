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
    /// Represents the Link types.
    /// </summary>
    public enum DayDescriptor
    {
        /// <summary>
        /// Day descriptor for day
        /// </summary>
        DAY,
        /// <summary>
        /// Descriptor for weekdays only.
        /// </summary>
        WEEKDAY,
        /// <summary>
        /// Descriptor for weekends.
        /// </summary>
        WEEKEND,
        /// <summary>
        /// Descriptor for Sunday.
        /// </summary>
        SUNDAY,
        /// <summary>
        /// Descriptor for Monday.
        /// </summary>
        MONDAY,
        /// <summary>
        /// Descriptor for Tuesday.
        /// </summary>
        TUESDAY,
        /// <summary>
        /// Descriptor for Wednesday.
        /// </summary>
        WEDNESDAY,
        /// <summary>
        /// Descriptor for Thursday.
        /// </summary>
        THURSDAY,
        /// <summary>
        /// Descriptor for Friday.
        /// </summary>
        FRIDAY,
        /// <summary>
        /// Descriptor for Saturday.
        /// </summary>
        SATURDAY
    }
}