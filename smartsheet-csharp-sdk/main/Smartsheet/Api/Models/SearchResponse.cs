//    #[license]
//    SmartsheetClient SDK for C#
//    %%
//    Copyright (C) 2026 SmartsheetClient
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
    /// Top-level response returned by the search endpoint (unified search-service schema).
    /// Use <see cref="SearchResultItem.ObjectType"/> to distinguish result types and
    /// access type-specific fields.
    /// </summary>
    public class SearchResponse
    {
        private int? totalCount;
        private IList<SearchResultItem> searchResults;
        private IList<object> workspaces;
        private string personalWorkspaceId;

        /// <summary>Number of items returned in <see cref="SearchResults"/>.</summary>
        public int? TotalCount { get { return totalCount; } set { totalCount = value; } }

        /// <summary>Array of matched search result items.</summary>
        public IList<SearchResultItem> SearchResults { get { return searchResults; } set { searchResults = value; } }

        /// <summary>Workspaces associated with the results.</summary>
        public IList<object> Workspaces { get { return workspaces; } set { workspaces = value; } }

        /// <summary>The authenticated user's personal workspace ID, if present in results.</summary>
        public string PersonalWorkspaceId { get { return personalWorkspaceId; } set { personalWorkspaceId = value; } }
    }
}
