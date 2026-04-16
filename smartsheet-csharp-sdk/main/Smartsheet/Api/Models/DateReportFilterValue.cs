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
    /// Represents a date report filter value with objectType.
    /// </summary>
    public class DateReportFilterValue : ReportFilterValue
    {
        private string objectType;
        private string value;

        /// <summary>
        /// Constructor for date report filter value.
        /// </summary>
        /// <param name="value">The date string value</param>
        public DateReportFilterValue(string value)
        {
            this.objectType = "DATE";
            this.value = value;
        }

        /// <summary>
        /// Gets the objectType (always "DATE").
        /// </summary>
        public string ObjectType
        {
            get { return this.objectType; }
        }

        /// <summary>
        /// Gets or sets the date value.
        /// </summary>
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Gets the report filter value type.
        /// </summary>
        public ReportFilterValueType? ValueType
        {
            get { return ReportFilterValueType.DATE; }
        }
    }
}
