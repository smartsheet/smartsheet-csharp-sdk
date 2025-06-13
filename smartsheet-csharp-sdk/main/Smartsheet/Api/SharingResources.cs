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

namespace Smartsheet.Api
{
    using Smartsheet.Api.Models;
    using MultiShare = Api.Models.MultiShare;
    using Share = Api.Models.Share;

    /// <summary>
    /// <para>This interface provides methods to access asset-based Sharing resources.</para>
    /// 
    /// <para>Thread Safety: Implementation of this interface must be thread safe.</para>
    /// </summary>
    public interface SharingResources
    {
        /// <summary>
        /// <para>List shares of a given asset.</para>
        /// <para>It mirrors to the following Smartsheet REST API method:<br />
        /// GET /shares?assetType={assetType}&assetId={assetId}</para>
        /// </summary>
        /// <param name="assetType"> the asset type (sheet, report, sight, workspace, etc.) </param>
        /// <param name="assetId"> the asset Id </param>
        /// <param name="paging"> the pagination request </param>
        /// <param name="shareScope"> when specified with a value of <see cref="ShareScope.Workspace"/>, the response will contain both item-level shares (scope='ITEM') and workspace-level shares (scope='WORKSPACE'). </param>
        /// <returns> the list of Share objects (note that an empty list will be returned if there is none). </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        PaginatedResult<Share> ListAssetShares(AssetType assetType, long assetId, PaginationParameters? paging = null, ShareScope? shareScope = null);

        /// <summary>
        /// <para>Get a specific share for the specified asset.</para>
        /// 
        /// <para>It mirrors to the following Smartsheet REST API method:<br />
        /// GET /shares/{shareId}?assetType={assetType}&assetId={assetId}</para>
        /// </summary>
        /// <param name="assetType"> the asset type (sheet, report, sight, workspace, etc.) </param>
        /// <param name="assetId"> the ID of the asset </param>
        /// <param name="shareId"> the ID of the share instance </param>
        /// <returns> the share (note that if there is no such resource, this method will throw ResourceNotFoundException
        /// rather than returning null). </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        Share GetAssetShare(AssetType assetType, long assetId, string shareId);

        /// <summary>
        /// <para>Shares an asset with the specified Users and Groups.</para>
        /// 
        /// <para>It mirrors to the following Smartsheet REST API method:<br />
        /// POST /shares?assetType={assetType}&assetId={assetId}</para>
        /// </summary>
        /// <param name="assetType"> the asset type (sheet, report, sight, workspace, etc.) </param>
        /// <param name="assetId"> the Id of the asset </param>
        /// <param name="shares"> the share objects </param>
        /// <param name="sendEmail">(optional): Either true or false to indicate whether or not
        /// to notify the user by email. Default is false.</param>
        /// <returns> the created share </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        IList<Share> ShareAsset(AssetType assetType, long assetId, IEnumerable<Share> shares, bool? sendEmail = null);

        /// <summary>
        /// <para>Updates the access level of a User or Group for the specified asset.</para>
        /// <para>It mirrors to the following Smartsheet REST API method:<br />
        /// PATCH /shares/{shareId}?assetType={assetType}&assetId={assetId}</para>
        /// </summary>
        /// <param name="assetType"> the asset type (sheet, report, sight, workspace, etc.) </param>
        /// <param name="assetId"> the ID of the asset </param>
        /// <param name="share"> the share </param>
        /// <returns> the updated share (note that if there is no such resource, this method will throw
        ///  ResourceNotFoundException rather than returning null). </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        Share UpdateShare(AssetType assetType, long assetId, Share share);

        /// <summary>
        /// <para>Delete a share.</para>
        /// <para>It mirrors to the following Smartsheet REST API method:<br />
        /// DELETE /shares/{shareId}?assetType={assetType}&assetId={assetId}</para>
        /// </summary>
        /// <param name="assetType"> the asset type (sheet, report, sight, workspace, etc.) </param>
        /// <param name="assetId"> the ID of the asset </param>
        /// <param name="shareId"> the ID of the user to whom the object is shared </param>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        void DeleteShare(AssetType assetType, long assetId, string shareId);
    }
}