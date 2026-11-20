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

using System.Threading;
using System.Threading.Tasks;
using Smartsheet.Api.Models;

namespace Smartsheet.Api
{
    /// <summary>
    /// Interface for governance resources.
    /// </summary>
    public interface GovernanceResources
    {
        /// <summary>
        /// <para>Gets the data classification settings for a plan.</para>
        ///
        /// <para>Mirrors to the following Smartsheet REST API method: GET /governance/data-classification/settings?planId={planId}</para>
        /// </summary>
        /// <param name="planId"> the ID of the plan whose classification settings to return </param>
        /// <returns> the data classification settings </returns>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        DataClassificationSettings GetDataClassificationSettings(long planId);

        /// <summary>
        /// <para>Gets the data classification settings for a plan asynchronously.</para>
        ///
        /// <para>Mirrors to the following Smartsheet REST API method: GET /governance/data-classification/settings?planId={planId}</para>
        /// </summary>
        /// <param name="planId"> the ID of the plan whose classification settings to return </param>
        /// <param name="cancellationToken"> the cancellation token </param>
        /// <returns> the data classification settings </returns>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        Task<DataClassificationSettings> GetDataClassificationSettingsAsync(long planId, CancellationToken cancellationToken = default);

        /// <summary>
        /// <para>Gets the data classification settings for the plan of a sheet, report, or dashboard.
        /// Use this overload when your request relates to a specific sheet, report, or dashboard.
        /// Supported asset types: <see cref="AssetType.SHEET"/>, <see cref="AssetType.REPORT"/>, <see cref="AssetType.SIGHT"/> (dashboard).</para>
        ///
        /// <para>Mirrors to the following Smartsheet REST API method: GET /governance/data-classification/settings?assetType={assetType}&amp;assetId={assetId}</para>
        /// </summary>
        /// <param name="assetType"> the type of the asset (<see cref="AssetType.SHEET"/>, <see cref="AssetType.REPORT"/> or <see cref="AssetType.SIGHT"/>) </param>
        /// <param name="assetId"> the ID of the sheet, report, or dashboard </param>
        /// <returns> the data classification settings </returns>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        DataClassificationSettings GetDataClassificationSettings(AssetType assetType, long assetId);

        /// <summary>
        /// <para>Gets the data classification settings for the plan of a sheet, report, or dashboard asynchronously.
        /// Use this overload when your request relates to a specific sheet, report, or dashboard.
        /// Supported asset types: <see cref="AssetType.SHEET"/>, <see cref="AssetType.REPORT"/>, <see cref="AssetType.SIGHT"/> (dashboard).</para>
        ///
        /// <para>Mirrors to the following Smartsheet REST API method: GET /governance/data-classification/settings?assetType={assetType}&amp;assetId={assetId}</para>
        /// </summary>
        /// <param name="assetType"> the type of the asset (<see cref="AssetType.SHEET"/>, <see cref="AssetType.REPORT"/> or <see cref="AssetType.SIGHT"/>) </param>
        /// <param name="assetId"> the ID of the sheet, report, or dashboard </param>
        /// <param name="cancellationToken"> the cancellation token </param>
        /// <returns> the data classification settings </returns>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        Task<DataClassificationSettings> GetDataClassificationSettingsAsync(AssetType assetType, long assetId, CancellationToken cancellationToken = default);
    }
}
