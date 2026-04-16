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
    /// Represents a string report filter value.
    /// </summary>
    public class StringReportFilterValue : ReportFilterValue
    {
        private string value;

        /// <summary>
        /// Constructor for string report filter value.
        /// </summary>
        /// <param name="value"></param>
        public StringReportFilterValue(string value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets or sets the string value.
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
            get { return ReportFilterValueType.STRING; }
        }
    }
}
