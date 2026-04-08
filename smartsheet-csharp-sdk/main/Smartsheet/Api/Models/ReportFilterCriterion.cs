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

using System.Collections.Generic;

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Represents a single filter criterion within a report filter expression.
    /// </summary>
    public class ReportFilterCriterion
    {
        /// <summary>
        /// The column identifier.
        /// </summary>
        private ReportColumnIdentifier column;

        /// <summary>
        /// Condition operator.
        /// </summary>
        private ReportFilterCriteriaOperator operatorValue;

        /// <summary>
        /// List of filter values. Each value can be a string, number, null, or an object with objectType.
        /// </summary>
        private IList<FilterValue> values;

        /// <summary>
        /// Gets the column identifier.
        /// </summary>
        /// <returns> the column </returns>
        public ReportColumnIdentifier Column
        {
            get { return column; }
            set { column = value; }
        }

        /// <summary>
        /// Gets the condition operator.
        /// </summary>
        /// <returns> the operator </returns>
        public ReportFilterCriteriaOperator Operator
        {
            get { return operatorValue; }
            set { operatorValue = value; }
        }

        /// <summary>
        /// Gets the list of filter values. Each value can be a string, number, null, or an object with objectType.
        /// </summary>
        /// <returns> the values </returns>
        public IList<FilterValue> Values
        {
            get { return values; }
            set { values = value; }
        }
    }
}
