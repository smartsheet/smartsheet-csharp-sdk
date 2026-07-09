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
using System.Linq;

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Node in a report path response. Contains recursive folders leading to the target report.
    /// </summary>
    public class ReportPathNode : PathNode
    {
        /// <summary>
        /// The list of nested folder nodes on the path from this node to the target report; may be null.
        /// </summary>
        public IList<ReportPathNode>? Folders { get; set; }

        /// <summary>
        /// The list of report leaf nodes within this node; non-null only at the deepest folder level.
        /// </summary>
        public IList<PathLeaf>? Reports { get; set; }

        /// <summary>
        /// Walks down through folders until reaching the node that contains reports,
        /// then returns the first report.
        /// </summary>
        /// <returns>
        /// The first <see cref="PathLeaf"/> report found at the deepest level, or <c>null</c>
        /// if the path contains no reports.
        /// </returns>
        public PathLeaf? GetLeafReport()
        {
            if (Reports != null && Reports.Count > 0)
            {
                return Reports.First();
            }

            if (Folders != null && Folders.Count > 0)
            {
                return Folders.First().GetLeafReport();
            }

            return null;
        }

        /// <summary>
        /// Returns a <c>/</c>-separated path of node names from this node down to the target report.
        /// </summary>
        /// <returns>
        /// A string such as <c>"/Workspace/Folder/Report"</c>, or <c>null</c> if the path contains
        /// no reports.
        /// </returns>
        /// <example>
        /// Workspace → Folder → Report returns <c>"/Workspace/Folder/Report"</c>.
        /// Workspace → Report (no intermediate folders) returns <c>"/Workspace/Report"</c>.
        /// </example>
        public string? GetLeafReportPath()
        {
            if (Reports != null && Reports.Count > 0)
            {
                return $"/{Name}/{Reports.First().Name}";
            }

            if (Folders != null && Folders.Count > 0)
            {
                return $"/{Name}{Folders.First().GetLeafReportPath()}";
            }

            return null;
        }
    }
}
