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
    /// <para>Object returned for GET operations against index endpoints using token-based pagination.</para>
    /// This object provides data and a token for the next page of results.
    /// </summary>
    public class TokenPaginatedResult<T>
    {
        /// <summary>
        /// the result set (array)
        /// </summary>
        private IList<T> data;

        /// <summary>
        /// the token for the next page of results
        /// </summary>
        private string? lastKey;

        /// <summary>
        /// A list of objects representing the current page of data in the result set.
        /// </summary>
        public IList<T> Data
        {
            get { return data; }
            set { data = value; }
        }

        /// <summary>
        /// The token for the next page of results. Null if this is the last page.
        /// </summary>
        public string? LastKey
        {
            get { return lastKey; }
            set { lastKey = value; }
        }
    }
}