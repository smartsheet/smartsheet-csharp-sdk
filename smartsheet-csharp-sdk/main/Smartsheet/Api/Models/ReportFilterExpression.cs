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
    /// Represents a report filter expression. It is a recursive object that allows at most 3 levels.
    /// At least one of Criteria or NestedCriteria has to be provided in addition to Operator.
    ///
    /// Example:
    /// ("Price" > 11 AND "Primary" CONTAINS "PROJ-1")
    /// OR
    /// ("Quantity" &lt; 12 AND "Sold Out" IS_CHECKED)
    /// </summary>
    public class ReportFilterExpression
    {
        /// <summary>
        /// The boolean operator that will be applied to the list of criteria and nested criteria.
        /// </summary>
        private ReportFilterOperator operatorValue;

        /// <summary>
        /// A recursive list of report filter expressions. Each item will be joined to the filter expression
        /// with the AND/OR operator defined on this level.
        /// </summary>
        private IList<ReportFilterExpression>? nestedCriteria;

        /// <summary>
        /// Criteria objects specifying custom criteria against which to match cell values. Each item will be
        /// joined to the filter expression with the AND/OR operator defined on this level.
        /// </summary>
        private IList<ReportFilterCriterion>? criteria;

        /// <summary>
        /// Gets the boolean operator that will be applied to the list of criteria and nested criteria.
        /// </summary>
        /// <returns> the operator </returns>
        public ReportFilterOperator Operator
        {
            get { return operatorValue; }
            set { operatorValue = value; }
        }

        /// <summary>
        /// Gets the recursive list of report filter expressions.
        /// </summary>
        /// <returns> the nested criteria </returns>
        public IList<ReportFilterExpression>? NestedCriteria
        {
            get { return nestedCriteria; }
            set { nestedCriteria = value; }
        }

        /// <summary>
        /// Gets the criteria objects specifying custom criteria against which to match cell values.
        /// </summary>
        /// <returns> the criteria </returns>
        public IList<ReportFilterCriterion>? Criteria
        {
            get { return criteria; }
            set { criteria = value; }
        }
    }
}
