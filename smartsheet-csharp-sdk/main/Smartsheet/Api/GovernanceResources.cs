using Smartsheet.Api.Models;

namespace Smartsheet.Api
{
    /// <summary>
    /// Interface for governance resources.
    /// </summary>
    public interface GovernanceResources
    {
        /// <summary>
        /// Gets the data classification settings for a plan.
        /// GET /2.0/governance/data-classification/settings?planId={planId}
        /// </summary>
        /// <param name="planId"> the plan ID </param>
        /// <returns> the data classification settings </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or an empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        DataClassificationSettings GetDataClassificationSettings(long planId);

        /// <summary>
        /// Gets the data classification settings by resolving the plan from an asset.
        /// Accepted assetType values: "sheet", "report", "sight" (dashboard).
        /// GET /2.0/governance/data-classification/settings?assetType={assetType}&amp;assetId={assetId}
        /// </summary>
        /// <param name="assetType"> the type of the asset (sheet, report, sight) </param>
        /// <param name="assetId"> the ID of the asset </param>
        /// <returns> the data classification settings </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or an empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        DataClassificationSettings GetDataClassificationSettings(string assetType, long assetId);
    }
}
