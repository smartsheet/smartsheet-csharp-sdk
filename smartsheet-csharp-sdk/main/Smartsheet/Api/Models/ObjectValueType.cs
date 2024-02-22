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
    /// Types of values an object can have.
    /// </summary>
    public enum ObjectValueType
    {
        /// <summary>
        /// a date in ISO-8601 format, or a free-form text value.
        /// </summary>
        DATE,
        /// <summary>
        /// a project date and time in ISO-8601 format, or a free-form text value.
        /// </summary>
        DATETIME,
        /// <summary>
        /// a project date and time in ISO-8601 format, or a free-form text value.
        /// </summary>
        ABSTRACT_DATETIME,
        /// <summary>
        /// an email address representing a contact, or a free-form text value.
        /// </summary>
        CONTACT,
        /// <summary>
        /// a duration value such as "4d 6h 30m" in the user's locale, or a free-form text value.
        /// </summary>
        DURATION,
        /// <summary>
        /// a comma-delimited predecessor list such as "12FS +3d 4h, 14SS", or a free-form text value.
        /// </summary>
        PREDECESSOR_LIST,
        /// <summary>
        /// only visible when using a query parameter of level and the value appropriate to the dashboard,
        /// report, or sheet that you are querying, otherwise the column type is TEXT_NUMBER.
        /// </summary>
        MULTI_CONTACT,
        /// <summary>
        /// only visible when using a query parameter of level and the value appropriate to the dashboard,
        /// report, or sheet that you are querying, otherwise the column type is TEXT_NUMBER.
        /// </summary>
        MULTI_PICKLIST,
        /// <summary>
        /// Numeric object 
        /// </summary>
        NUMBER,
        /// <summary>
        /// Object representing a true or false state
        /// </summary>
        BOOLEAN,
        /// <summary>
        /// Object representing a string of characters
        /// </summary>
        STRING,
        /// <summary>
        /// Empty state
        /// </summary>
        NULL
    }
}
