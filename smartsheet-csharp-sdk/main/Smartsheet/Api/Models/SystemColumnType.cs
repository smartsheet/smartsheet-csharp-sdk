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
    /// Represents the system column types. </summary>
    /// <seealso href="http://help.Smartsheet.com/customer/portal/articles/504619-column-types">Column Types Help</seealso>
    public enum SystemColumnType
    {
        /// <summary>
        /// Auto number columns generate values for every row in the sheet that contains data.
        /// </summary>
        AUTO_NUMBER,
        /// <summary>
        /// When a row is modified this column updates the date.
        /// </summary>
        MODIFIED_DATE,
        /// <summary>
        /// When a row is modified this column updates the user that modified it.
        /// </summary>
        MODIFIED_BY,
        /// <summary>
        /// Populated by date that row was created.
        /// </summary>
        CREATED_DATE,
        /// <summary>
        /// Populated by who created the row.
        /// </summary>
        CREATED_BY
    }
}