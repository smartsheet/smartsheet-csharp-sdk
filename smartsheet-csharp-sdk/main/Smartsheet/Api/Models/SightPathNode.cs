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
    /// Node in a sight path response. Contains recursive folders leading to the target sight.
    /// </summary>
    public class SightPathNode : PathNode
    {
        /// <summary>
        /// The list of nested folder nodes on the path from this node to the target sight; may be null.
        /// </summary>
        public IList<SightPathNode>? Folders { get; set; }

        /// <summary>
        /// The list of sight leaf nodes within this node; non-null only at the deepest folder level.
        /// </summary>
        public IList<PathLeaf>? Sights { get; set; }

        /// <summary>
        /// Walks down through folders until reaching the node that contains sights,
        /// then returns the first sight.
        /// </summary>
        /// <returns>
        /// The first <see cref="PathLeaf"/> sight found at the deepest level, or <c>null</c>
        /// if the path contains no sights.
        /// </returns>
        public PathLeaf? GetLeafSight()
        {
            if (Sights != null && Sights.Count > 0)
            {
                return Sights.First();
            }

            if (Folders != null && Folders.Count > 0)
            {
                return Folders.First().GetLeafSight();
            }

            return null;
        }

        /// <summary>
        /// Returns a <c>/</c>-separated path of node names from this node down to the target sight.
        /// </summary>
        /// <returns>
        /// A string such as <c>"/Workspace/Folder/Dashboard"</c>, or <c>null</c> if the path
        /// contains no sights.
        /// </returns>
        /// <example>
        /// Workspace → Folder → Dashboard returns <c>"/Workspace/Folder/Dashboard"</c>.
        /// Workspace → Dashboard (no intermediate folders) returns <c>"/Workspace/Dashboard"</c>.
        /// </example>
        public string? GetLeafSightPath()
        {
            if (Sights != null && Sights.Count > 0)
            {
                return $"/{Name}/{Sights.First().Name}";
            }

            if (Folders != null && Folders.Count > 0)
            {
                return $"/{Name}{Folders.First().GetLeafSightPath()}";
            }

            return null;
        }
    }
}
