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
    /// Node in a sheet path response. Contains recursive folders leading to the target sheet.
    /// </summary>
    public class SheetPathNode : PathNode
    {
        /// <summary>
        /// The list of nested folder nodes on the path from this node to the target sheet; may be null.
        /// </summary>
        public IList<SheetPathNode>? Folders { get; set; }

        /// <summary>
        /// The list of sheet leaf nodes within this node; non-null only at the deepest folder level.
        /// </summary>
        public IList<PathLeaf>? Sheets { get; set; }

        /// <summary>
        /// Walks down through folders until reaching the node that contains sheets,
        /// then returns the first sheet.
        /// </summary>
        /// <returns>
        /// The first <see cref="PathLeaf"/> sheet found at the deepest level, or <c>null</c>
        /// if the path contains no sheets.
        /// </returns>
        public PathLeaf? GetLeafSheet()
        {
            if (Sheets != null && Sheets.Count > 0)
            {
                return Sheets.First();
            }

            if (Folders != null && Folders.Count > 0)
            {
                return Folders.First().GetLeafSheet();
            }

            return null;
        }

        /// <summary>
        /// Returns a <c>/</c>-separated path of node names from this node down to the target sheet.
        /// </summary>
        /// <returns>
        /// A string such as <c>"/Workspace/Folder/Sheet"</c>, or <c>null</c> if the path contains
        /// no sheets.
        /// </returns>
        /// <example>
        /// Workspace → Folder → Sheet returns <c>"/Workspace/Folder/Sheet"</c>.
        /// Workspace → Sheet (no intermediate folders) returns <c>"/Workspace/Sheet"</c>.
        /// </example>
        public string? GetLeafSheetPath()
        {
            if (Sheets != null && Sheets.Count > 0)
            {
                return $"/{Name}/{Sheets.First().Name}";
            }

            if (Folders != null && Folders.Count > 0)
            {
                return $"/{Name}{Folders.First().GetLeafSheetPath()}";
            }

            return null;
        }
    }
}
