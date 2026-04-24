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
    /// Represents a request to create a new report.
    /// </summary>
    public class CreateReportRequest
    {
        /// <summary>
        /// Report name (required, 1-50 characters).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of report columns (required, minItems: 1, maxItems: 400).
        /// </summary>
        public IList<ReportColumn> Columns { get; set; }

        /// <summary>
        /// List of sheets and/or workspaces to include in the report scope (required, minItems: 1, maxItems: 100).
        /// </summary>
        public IList<ReportScopeInclusion> Scope { get; set; }

        /// <summary>
        /// The report definition containing filters, grouping, summarizing, and sorting criteria (optional).
        /// </summary>
        public ReportDefinition ReportDefinition { get; set; }

        /// <summary>
        /// Set to true if the report is a sheet summary report; otherwise it is a row report (default: false).
        /// </summary>
        public bool? IsSummaryReport { get; set; }

        /// <summary>
        /// The destination container for the new report (required).
        /// </summary>
        public ReportDestination Destination { get; set; }
    }
}
