//    #[license]
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
    /// Represents specific elements to include in a response.
    /// </summary>
    public enum SummaryFieldInclusion
    {
        /// <summary>
        ///  includes format info for cells, rows, and summary fields
        /// </summary>
        FORMAT,
        /// <summary>
        /// includes createdBy and modifiedBy attributes for rows and summary fields
        /// </summary>
        WRITER_INFO
    }
}
