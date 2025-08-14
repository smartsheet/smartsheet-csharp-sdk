using Smartsheet.Api.Internal.Util;
using System.Collections.Generic;

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Builds optional query string parameters for token-based pagination.
    /// </summary>
    public class TokenPaginationParameters
    {
        /// <summary>
        /// the token to continue pagination from
        /// </summary>
        private string? lastKey;

        /// <summary>
        /// the maximum number of items to return
        /// </summary>
        private int? maxItems;

        /// <summary>
        /// the pagination type
        /// </summary>
        private string paginationType;

        /// <summary>
        /// Builds optional query string parameters for token-based pagination.
        /// </summary>
        /// <param name="lastKey">The token to continue pagination from. Null for first page.</param>
        /// <param name="maxItems">The maximum number of items to return per page.</param>
        /// <param name="paginationType">The pagination type. Defaults to "token".</param>
        public TokenPaginationParameters(string? lastKey, int? maxItems, string paginationType = "token")
        {
            this.lastKey = lastKey;
            this.maxItems = maxItems;
            this.paginationType = paginationType;
        }

        /// <summary>
        /// The token to continue pagination from. Null for first page.
        /// </summary>
        public string? LastKey
        {
            get { return lastKey; }
            set { lastKey = value; }
        }

        /// <summary>
        /// The maximum number of items to return per page.
        /// </summary>
        public int? MaxItems
        {
            get { return maxItems; }
            set { maxItems = value; }
        }

        /// <summary>
        /// The pagination type.
        /// </summary>
        public string PaginationType
        {
            get { return paginationType; }
            set { paginationType = value; }
        }

        /// <summary>
        /// Returns a formatted string of query string parameters.
        /// </summary>
        /// <returns>the query string</returns>
        public string ToQueryString()
        {
            IDictionary<string, string> parameters = this.toDictionary();
            return QueryUtil.GenerateUrl(null, parameters);
        }

        /// <summary>
        /// Returns a dictionary of query string parameters.
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> toDictionary()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(lastKey))
            {
                parameters.Add("lastKey", lastKey);
            }

            if (maxItems.HasValue)
            {
                parameters.Add("maxItems", maxItems.ToString());
            }

            if (!string.IsNullOrEmpty(paginationType))
            {
                parameters.Add("paginationType", paginationType);
            }

            return parameters;
        }
    }
}