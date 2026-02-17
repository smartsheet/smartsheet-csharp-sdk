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

using System;
using System.Collections.Generic;
using System.Text;
using Smartsheet.Api.Internal.Http;

namespace Smartsheet.Api.Internal
{
    using Models;
    using Util;
    using System.Net;
    using Utils = Internal.Utility.Utility;

    /// <summary>
    /// This is the implementation of the SharingResources.
    /// 
    /// Thread Safety: This class is thread safe because it is immutable and its base class is thread safe.
    /// </summary>
    public class AssetSharingResourcesImpl : AbstractResources, AssetSharingResources
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="smartsheet"> the Smartsheet </param>
        public AssetSharingResourcesImpl(SmartsheetImpl smartsheet)
            : base(smartsheet)
        {
        }

        /// <summary>
        /// <para>List shares of a given asset.</para>
        /// <para>It mirrors to the following Smartsheet REST API method:<br />
        /// GET /shares?assetType={assetType}&assetId={assetId}</para>
        /// </summary>
        /// <param name="assetType"> the asset type (sheet, report, sight, workspace, etc.) </param>
        /// <param name="assetId"> the asset Id </param>
        /// <param name="tokenPaginationParameters"> contains token pagination parameters </param>
        /// <param name="sharingInclude"> when specified with a value of <see cref="ShareScope.WORKSPACE"/>, the response will contain both item-level shares (scope='ITEM') and workspace-level shares (scope='WORKSPACE'). </param>
        /// <returns> the list of Share objects (note that an empty list will be returned if there is none). </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        public virtual ListAssetSharesResponse ListAssetShares(AssetType assetType, string assetId,
            TokenPaginationParameters? tokenPaginationParameters = null, ShareScope? sharingInclude = null)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("assetType", assetType.ToString().ToLower());
            parameters.Add("assetId", assetId);
            if (tokenPaginationParameters != null)
            {
                if (tokenPaginationParameters.LastKey != null)
                {
                    parameters.Add("lastKey", tokenPaginationParameters.LastKey);
                }

                if (tokenPaginationParameters.MaxItems != null)
                {
                    parameters.Add("maxItems", tokenPaginationParameters.MaxItems.ToString());
                }
            }

            if (sharingInclude != null)
            {
                parameters.Add("sharingInclude", sharingInclude.ToString());
            }

            String path = QueryUtil.GenerateUrl("/2.0/shares", parameters);

            return ListAssetSharesInternal(path);
        }

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
        public virtual AssetShare GetAssetShare(AssetType assetType, string assetId, string shareId)
        {
            StringBuilder url = new StringBuilder("/2.0/shares/");
            url.Append(shareId);

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("assetType", assetType.ToString().ToLower());
            parameters.Add("assetId", assetId);
            String path = QueryUtil.GenerateUrl(url.ToString(), parameters);

            return GetResource<AssetShare>(path, typeof(AssetShare));
        }

        /// <summary>
        /// <para>Shares an asset with the specified user or group.</para>
        ///
        /// <para>It mirrors to the following Smartsheet REST API method:<br />
        /// POST /shares?assetType={assetType}&assetId={assetId}</para>
        /// </summary>
        /// <param name="assetType"> the asset type (sheet, report, sight, workspace, etc.) </param>
        /// <param name="assetId"> the Id of the asset </param>
        /// <param name="shareRequest"> the share request object </param>
        /// <param name="sendEmail">(optional): Either true or false to indicate whether or not
        ///     to notify the user by email. Default is false.</param>
        /// <returns> the created share </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        public virtual AssetShare ShareAsset(AssetType assetType, string assetId, ShareAssetRequest shareRequest,
            bool? sendEmail = null)
        {
            StringBuilder url = new StringBuilder("/2.0/shares");

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("assetType", assetType.ToString().ToLower());
            parameters.Add("assetId", assetId);

            if (sendEmail.HasValue)
            {
                parameters.Add("sendEmail", sendEmail.ToString().ToLower());
            }

            String path = QueryUtil.GenerateUrl(url.ToString(), parameters);

            HttpRequest request;
            try
            {
                request = CreateHttpRequest(new Uri(Smartsheet.BaseURI, path), HttpMethod.POST);
            }
            catch (Exception e)
            {
                throw new SmartsheetException(e);
            }

            request.Entity = serializeToEntity<ShareAssetRequest>(shareRequest);
            HttpResponse response = Smartsheet.HttpClient.Request(request);

            AssetShare result = null;
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    result = Smartsheet.JsonSerializer.deserialize<AssetShare>(response.Entity.GetContent());
                    break;
                default:
                    HandleError(response);
                    break;
            }

            Smartsheet.HttpClient.ReleaseConnection();

            return result;
        }

        /// <summary>
        /// <para>Updates the access level of a User or Group for the specified asset.</para>
        /// <para>It mirrors to the following Smartsheet REST API method:<br />
        /// PATCH /shares/{shareId}?assetType={assetType}&assetId={assetId}</para>
        /// </summary>
        /// <param name="assetType"> the asset type (sheet, report, sight, workspace, etc.) </param>
        /// <param name="assetId"> the ID of the asset </param>
        /// <param name="shareId"> the ID of the share </param>
        /// <param name="updateShareRequest"> the update request </param>
        /// <returns> the updated share (note that if there is no such resource, this method will throw
        ///  ResourceNotFoundException rather than returning null). </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        public virtual AssetShare UpdateAssetShare(AssetType assetType, string assetId, String shareId,
            UpdateShareRequest updateShareRequest)
        {
            StringBuilder url = new StringBuilder("/2.0/shares/");
            url.Append(shareId);

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("assetType", assetType.ToString().ToLower());
            parameters.Add("assetId", assetId);
            String path = QueryUtil.GenerateUrl(url.ToString(), parameters);

            return PatchResource<AssetShare, UpdateShareRequest>(path, updateShareRequest);
        }

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
        public virtual void DeleteAssetShare(AssetType assetType, string assetId, string shareId)
        {
            StringBuilder url = new StringBuilder("/2.0/shares/");
            url.Append(shareId);

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("assetType", assetType.ToString().ToLower());
            parameters.Add("assetId", assetId);

            String path = QueryUtil.GenerateUrl(url.ToString(), parameters);

            DeleteResource<AssetShare>(path);
        }

        /// <summary>
        /// List resources using SmartsheetClient REST API.
        /// 
        /// Exceptions:
        ///   IllegalArgumentException : if any argument is null, or path is an empty string
        ///   InvalidRequestException : if there is any problem with the REST API request
        ///   AuthorizationException : if there is any problem with the REST API authorization (access token)
        ///   ServiceUnavailableException : if the REST API service is not available (possibly due to rate limiting)
        ///   SmartsheetRestException : if any other REST API related error occurred during the operation
        ///   SmartsheetException : if any other error occurred during the operation
        /// </summary>
        /// <param name="path"> the relative path of the resource collections </param>
        /// <returns> the resources </returns>
        /// <exception cref="SmartsheetException"> if an error occurred during the operation </exception>
        private ListAssetSharesResponse ListAssetSharesInternal(string path)
        {
            Utils.ThrowIfNull(path);
            Utils.ThrowIfEmpty(path);

            HttpRequest request;
            try
            {
                request = CreateHttpRequest(new Uri(smartsheet.BaseURI, path), HttpMethod.GET);
            }
            catch (Exception e)
            {
                throw new SmartsheetException(e);
            }

            HttpResponse response = smartsheet.HttpClient.Request(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    smartsheet.HttpClient.ReleaseConnection();
                    return smartsheet.JsonSerializer.deserialize<ListAssetSharesResponse>(response.Entity.GetContent());
                default:
                    smartsheet.HttpClient.ReleaseConnection();
                    HandleError(response);
                    return null;
            }
        }
    }
}