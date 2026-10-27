namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Possible result codes for BulkItemResult or RequestResult
    /// </summary>
    public static class ResultCode
    {
        /// <summary>
        /// a successful operation occurred with no errors.
        /// </summary>
        public const int SUCCESS = 0;
        /// <summary>
        /// an operation partially succeeded.
        /// </summary>
        public const int PARTIAL_SUCCESS = 3;
    }
}