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

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Possible search filters to use to narrow results.
    /// </summary>
    public enum SearchScope
    {
        /// <summary>
        /// Search for names and descriptions of attachments
        /// </summary>
        [EnumMember(Value = "attachments")]
        ATTACHMENTS,
        /// <summary>
        /// Search for data within rows
        /// </summary>
        [EnumMember(Value = "cellData")]
        CELL_DATA,
        /// <summary>
        /// Search for comments including replies to an initial comment
        /// </summary>
        [EnumMember(Value = "comments")]
        COMMENTS,
        /// <summary>
        /// Search for names of folders
        /// </summary>
        [EnumMember(Value = "folderNames")]
        FOLDER_NAMES,
        /// <summary>
        /// Search for names of reports
        /// </summary>
        [EnumMember(Value = "reportNames")]
        REPORT_NAMES,
        /// <summary>
        /// Search for names of sheets
        /// </summary>
        [EnumMember(Value = "sheetNames")]
        SHEET_NAMES,
        /// <summary>
        /// Search for names of sights, also known as dashboards
        /// </summary>
        [EnumMember(Value = "sightNames")]
        SIGHT_NAMES,
        /// <summary>
        /// Search for summary fields
        /// </summary>
        [EnumMember(Value = "summaryFields")]
        SUMMARY_FIELDS,
        /// <summary>
        /// Search for names of templates 
        /// </summary>
        [EnumMember(Value = "templateNames")]
        TEMPLATE_NAMES,
        /// <summary>
        /// Search for names of workspaces
        /// </summary>
        [EnumMember(Value = "workspaceNames")]
        WORKSPACE_NAMES
    }
}
