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
    /// Represents the report definition containing filters, grouping and sorting properties of the report.
    /// Note: When groupingCriteria is defined the primary column of the report will move to the index 0
    /// when it is first rendered by the app.
    /// </summary>
    public class ReportDefinition
    {
        /// <summary>
        /// Represents the filters for the report.
        /// </summary>
        private ReportFilterExpression filters;

        /// <summary>
        /// Represents the list of report grouping criteria.
        /// </summary>
        private IList<ReportGroupingCriterion> groupingCriteria;

        /// <summary>
        /// Represents the list of report aggregation criteria.
        /// </summary>
        private IList<ReportAggregationCriterion> aggregationCriteria;

        /// <summary>
        /// Represents the list of report sorting criteria.
        /// </summary>
        private IList<ReportSortingCriterion> sortingCriteria;

        /// <summary>
        /// Gets the filters for the report.
        /// </summary>
        /// <returns> the filters </returns>
        public ReportFilterExpression Filters
        {
            get { return filters; }
            set { filters = value; }
        }

        /// <summary>
        /// Gets the list of report grouping criteria.
        /// </summary>
        /// <returns> the grouping criteria </returns>
        public IList<ReportGroupingCriterion> GroupingCriteria
        {
            get { return groupingCriteria; }
            set { groupingCriteria = value; }
        }

        /// <summary>
        /// Gets the list of report aggregation criteria.
        /// </summary>
        /// <returns> the aggregation criteria </returns>
        public IList<ReportAggregationCriterion> AggregationCriteria
        {
            get { return aggregationCriteria; }
            set { aggregationCriteria = value; }
        }

        /// <summary>
        /// Gets the list of report sorting criteria.
        /// </summary>
        /// <returns> the sorting criteria </returns>
        public IList<ReportSortingCriterion> SortingCriteria
        {
            get { return sortingCriteria; }
            set { sortingCriteria = value; }
        }
    }
}
