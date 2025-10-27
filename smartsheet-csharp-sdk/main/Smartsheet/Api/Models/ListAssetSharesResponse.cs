using System;
using System.Collections.Generic;

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// <para>Object returned for the get /shares endpoint.</para>
    /// This object provides metadata which can be used to perform paging on potentially large data sets.
    /// </summary>
    public class ListAssetSharesResponse
    {
        
        /// <summary>
        /// A list of objects representing the current page of data in the result set.
        /// </summary>
        public IList<Share> Items { get; set; }

        /// <summary>
        /// Pagination token that can be used to retrieve the next set of results.
        /// </summary>
        public String LastKey { get; set; }
    }
}
