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

namespace Smartsheet.Api
{
    using Smartsheet.Api.Models;
    using Workspace = Api.Models.Workspace;

    /// <summary>
    /// <para>This interface provides methods to access Workspace resources.</para>
    ///
    /// <para>Thread Safety: Implementation of this interface must be thread safe.</para>
    /// </summary>
    public interface WorkspaceResources
    {
        /// <summary>
        /// <para>List all Workspaces.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: GET /Workspaces</para>
        /// <remarks>This operation supports token-based pagination of results.</remarks>
        /// </summary>
        /// <param name="tokenPaging">Token-based pagination parameters</param>
        /// <returns> the list of Workspaces with pagination token (note that an empty list will be returned if there are none) </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        TokenPaginatedResult<Workspace> ListWorkspaces(TokenPaginationParameters? tokenPaging = null);

        /// <summary>
        /// <para>Create a workspace.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: POST /Workspaces</para>
        /// </summary>
        /// <param name="workspace"> the workspace to create </param>
        /// <returns> the created workspace </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        Workspace CreateWorkspace(Workspace workspace);

        /// <summary>
        /// <para>Update a workspace.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: PUT /workspaces/{workspaceId}</para>
        /// </summary>
        /// <param name="workspace"> the workspace to update </param>
        /// <returns> the updated workspace (note that if there is no such resource, this method will throw
        /// ResourceNotFoundException rather than returning null) </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        Workspace UpdateWorkspace(Workspace workspace);

        /// <summary>
        /// <para>Creates a copy of the specified Workspace.</para>
        /// <para>It mirrors to the following Smartsheet REST API method:<br />
        /// POST /workspaces/{workspaceId}/copy</para>
        /// </summary>
        /// <param name="workspaceId"> the workspace Id </param>
        /// <param name="destination"> the destination to copy to </param>
        /// <param name="include"> the elements to copy. Note: Cell history will not be copied, regardless of which include parameter values are specified.</param>
        /// <param name="skipRemap"> the references to NOT re-map for the newly created folder
        /// <para>
        /// If "cellLinks" is specified in the skipRemap parameter value, the cell links within the newly created folder will continue to point to the original source sheets.
        /// If "reports" is specified in the skipRemap parameter value, the reports within the newly created folder will continue to point to the original source sheets.
        /// </para>
        /// </param>
        /// <returns> the created workspace </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        Workspace CopyWorkspace(long workspaceId, ContainerDestination destination, IEnumerable<WorkspaceCopyInclusion>? include = null, IEnumerable<WorkspaceRemapExclusion>? skipRemap = null);

        /// <summary>
        /// <para>Deletes the specified Workspace (and its contents).</para>
        /// <para>It mirrors to the following Smartsheet REST API method: DELETE /workspaces{workspaceId}</para>
        /// </summary>
        /// <param name="workspaceId"> the Id of the workspace </param>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        void DeleteWorkspace(long workspaceId);

        /// <summary>
        /// <para>Return the WorkspaceFolderResources object that provides access to Folder resources associated with Workspace
        /// resources.</para>
        /// </summary>
        /// <returns> the workspace folder resources </returns>
        WorkspaceFolderResources FolderResources { get; }

        /// <summary>
        /// <para>Return the WorkspaceFolderResources object that provides access to Folder resources associated with Workspace
        /// resources.</para>
        /// </summary>
        /// <returns> the workspace folder resources </returns>
        WorkspaceSheetResources SheetResources { get; }

        /// <summary>
        /// <para>Gets a page of a workspace's children of the specified type.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: GET /workspaces/{workspaceId}/children</para>
        /// </summary>
        /// <param name="workspaceId">the workspace id</param>
        /// <param name="childrenResourceTypes">A comma-separated list of the child types to include in the response</param>
        /// <param name="include">A comma-separated list of optional elements to include in the response</param>
        /// <param name="numericDates">If true, dates are accepted and returned in Unix epoch time. Default is false, which means ISO-8601 format</param>
        /// <param name="accessApiLevel">Allows COMMENTER access for inputs and return values. For backwards-compatibility, VIEWER is the default</param>
        /// <param name="lastKey">The lastKey token returned from the previous page of results</param>
        /// <param name="maxItems">The maximum number of items to return in the response (default: 100, min: 100, max: 1000)</param>
        /// <returns>An array of asset references with a pagination token if there are more results</returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        TokenPaginatedResult<object> GetWorkspaceChildren(long workspaceId, IEnumerable<ChildrenResourceType>? childrenResourceTypes = null, IEnumerable<ChildrenInclusion>? include = null, bool? numericDates = null, int? accessApiLevel = null, string? lastKey = null, int? maxItems = null);

        /// <summary>
        /// <para>Gets the metadata of a workspace.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: GET /workspaces/{workspaceId}/metadata</para>
        /// </summary>
        /// <param name="workspaceId">the workspace id</param>
        /// <param name="include">A comma-separated list of optional elements to include in the response</param>
        /// <param name="numericDates">If true, dates are accepted and returned in Unix epoch time. Default is false, which means ISO-8601 format</param>
        /// <param name="accessApiLevel">Allows COMMENTER access for inputs and return values. For backwards-compatibility, VIEWER is the default</param>
        /// <returns>The metadata of a workspace</returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        Workspace GetWorkspaceMetadata(long workspaceId, IEnumerable<WorkspaceInclusion>? include = null, bool? numericDates = null, int? accessApiLevel = null);
    }

}
