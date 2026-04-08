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
    /// Represents a numeric filter value.
    /// </summary>
    public class NumberFilterValue : FilterValue
    {
        private double value;

        /// <summary>
        /// Constructor for numeric filter value.
        /// </summary>
        /// <param name="value"></param>
        public NumberFilterValue(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets or sets the numeric value.
        /// </summary>
        public double Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Gets the filter value type.
        /// </summary>
        public FilterValueType? ValueType
        {
            get { return FilterValueType.NUMBER; }
        }
    }
}
