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
    /// Represents a grouping criterion for a report.
    /// </summary>
    public class ReportGroupingCriterion
    {
        /// <summary>
        /// Grouping criteria id (read-only).
        /// </summary>
        private string? id;

        /// <summary>
        /// The column identifier.
        /// </summary>
        private ReportColumnIdentifier column;

        /// <summary>
        /// Sorting direction within the group.
        /// </summary>
        private SortDirection sortingDirection;

        /// <summary>
        /// Indicates whether the group is expanded in the UI.
        /// </summary>
        private bool? isExpanded;

        /// <summary>
        /// Gets the grouping criteria id.
        /// </summary>
        /// <returns> the id </returns>
        public string? Id
        {
            get { return id; }
            set { id = value; }
        }

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
        /// Gets the sorting direction within the group.
        /// </summary>
        /// <returns> the sorting direction </returns>
        public SortDirection SortingDirection
        {
            get { return sortingDirection; }
            set { sortingDirection = value; }
        }

        /// <summary>
        /// Gets whether the group is expanded in the UI.
        /// </summary>
        /// <returns> true if expanded, false otherwise </returns>
        public bool? IsExpanded
        {
            get { return isExpanded; }
            set { isExpanded = value; }
        }
    }
}
