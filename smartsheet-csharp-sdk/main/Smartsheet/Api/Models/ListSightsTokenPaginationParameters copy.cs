using System.Collections.Generic;

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Token-based pagination parameters specifically for ListWorkspaces operations.
    /// Extends the base TokenPaginationParameters with paginationType support.
    /// </summary>
    public class ListSightsTokenPaginationParameters : TokenPaginationParameters
    {
        /// <summary>
        /// the pagination type
        /// </summary>
        private string paginationType;

        /// <summary>
        /// Builds optional query string parameters for token-based pagination for ListWorkspaces.
        /// </summary>
        /// <param name="lastKey">The token to continue pagination from. Null for first page.</param>
        /// <param name="maxItems">The maximum number of items to return per page.</param>
        /// <param name="paginationType">The pagination type. Defaults to "token".</param>
        public ListSightsTokenPaginationParameters(string? lastKey, int? maxItems, string paginationType = "token")
            : base(lastKey, maxItems)
        {
            this.paginationType = paginationType;
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
        /// Returns a dictionary of query string parameters including paginationType.
        /// </summary>
        /// <returns></returns>
        public override IDictionary<string, string> toDictionary()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>(base.toDictionary());

            if (!string.IsNullOrEmpty(paginationType))
            {
                parameters.Add("paginationType", paginationType);
            }

            return parameters;
        }
    }
}
