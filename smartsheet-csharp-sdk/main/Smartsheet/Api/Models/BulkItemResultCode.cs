namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Types of values an object can have.
    /// </summary>
    public enum BulkItemResultCode
    {
        /// <summary>
        /// a successful bulk operation occurred with no errors.
        /// </summary>
        SUCCESS = 0,
        /// <summary>
        /// a bulk operation partially succeeded.
        /// </summary>
        PARTIAL_SUCCESS = 3
    }
}