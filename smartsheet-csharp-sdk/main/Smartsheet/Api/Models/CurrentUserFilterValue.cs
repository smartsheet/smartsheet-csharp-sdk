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
    /// Represents a current user filter value with objectType.
    /// </summary>
    public class CurrentUserFilterValue : FilterValue
    {
        private string objectType;

        /// <summary>
        /// Constructor for current user filter value.
        /// </summary>
        public CurrentUserFilterValue()
        {
            this.objectType = "CURRENT_USER";
        }

        /// <summary>
        /// Gets the objectType (always "CURRENT_USER").
        /// </summary>
        public string ObjectType
        {
            get { return this.objectType; }
        }

        /// <summary>
        /// Gets the filter value type.
        /// </summary>
        public FilterValueType? ValueType
        {
            get { return FilterValueType.CURRENT_USER; }
        }
    }
}
