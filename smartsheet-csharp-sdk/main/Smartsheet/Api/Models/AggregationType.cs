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
    /// Enum representing the type of aggregation for report data.
    /// </summary>
    public enum AggregationType
    {
        /// <summary>
        /// Sum aggregation.
        /// </summary>
        SUM,

        /// <summary>
        /// Average aggregation.
        /// </summary>
        AVG,

        /// <summary>
        /// Minimum value aggregation.
        /// </summary>
        MIN,

        /// <summary>
        /// Maximum value aggregation.
        /// </summary>
        MAX,

        /// <summary>
        /// Count aggregation.
        /// </summary>
        COUNT,

        /// <summary>
        /// First value aggregation.
        /// </summary>
        FIRST,

        /// <summary>
        /// Last value aggregation.
        /// </summary>
        LAST
    }
}
