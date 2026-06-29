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
    /// Node in a folder path response. Contains recursive folders leading to the target folder.
    /// </summary>
    public class FolderPathNode : PathNode
    {
        /// <summary>
        /// The list of nested folder nodes on the path from this node to the target folder; may be null.
        /// </summary>
        public IList<FolderPathNode>? Folders { get; set; }

        /// <summary>
        /// Walks down through nested folders until reaching the deepest (target) folder node,
        /// then returns it.
        /// </summary>
        /// <returns>
        /// The deepest <see cref="FolderPathNode"/> in the chain, which represents the target folder.
        /// </returns>
        public FolderPathNode GetLeafFolder()
        {
            if (Folders == null || Folders.Count == 0)
            {
                return this;
            }

            return Folders.First().GetLeafFolder();
        }

        /// <summary>
        /// Returns a <c>/</c>-separated path of folder names from this node down to the target folder.
        /// </summary>
        /// <returns>
        /// A string such as <c>"/Workspace/FolderA/FolderB/Target"</c>.
        /// </returns>
        /// <example>
        /// Workspace → FolderA → FolderB → Target returns <c>"/Workspace/FolderA/FolderB/Target"</c>.
        /// Workspace → Target (no intermediate folders) returns <c>"/Workspace/Target"</c>.
        /// </example>
        public string GetLeafFolderPath()
        {
            if (Folders == null || Folders.Count == 0)
            {
                return $"/{Name}";
            }

            return $"/{Name}{Folders.First().GetLeafFolderPath()}";
        }
    }
}
