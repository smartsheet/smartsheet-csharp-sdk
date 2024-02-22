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
    /// Represents the Tags to indicate a special column.
    /// </summary>
    public enum ColumnTag
    {
        /// <summary>
        /// Start date of calendar
        /// </summary>
        CALENDAR_START_DATE,
        /// <summary>
        /// End date of calendar
        /// </summary>
        CALENDAR_END_DATE,
        /// <summary>
        /// Checkbox type that allows for card view to show a done status
        /// </summary>
        CARD_DONE,
        /// <summary>
        /// Start date of gantt chart
        /// </summary>
        GANTT_START_DATE,
        /// <summary>
        /// End date of gantt chart
        /// </summary>
        GANTT_END_DATE,
        /// <summary>
        /// Completion so far in percent for a gantt project
        /// </summary>
        GANTT_PERCENT_COMPLETE,
        /// <summary>
        /// Display for gantt chart
        /// </summary>
        GANTT_DISPLAY_LABEL,
        /// <summary>
        /// Used to define predecessors in gantt chart
        /// </summary>
        GANTT_PREDECESSOR,
        /// <summary>
        /// Duration in gantt chart
        /// </summary>
        GANTT_DURATION,
        /// <summary>
        /// Assigned resources in gantt chart
        /// </summary>
        GANTT_ASSIGNED_RESOURCE,
        /// <summary>
        /// Allocation in gantt chart
        /// </summary>
        GANTT_ALLOCATION
        
    }
}