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
    /// Represents the result of creating a new report.
    /// </summary>
    public class CreateReportResult
    {
        /// <summary>
        /// The report's unique identifier.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// The report's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The user's access level to the report.
        /// </summary>
        public AccessLevel? AccessLevel { get; set; }

        /// <summary>
        /// URL to the report in Smartsheet.
        /// </summary>
        public string Permalink { get; set; }

        /// <summary>
        /// Set to true if the report is a sheet summary report; otherwise it is a row report.
        /// </summary>
        public bool? IsSummaryReport { get; set; }
    }
}
