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
    using Api.Models;

    /// <summary>
    /// <para>This interface provides methods to access User resources.</para>
    /// 
    /// <para>Thread Safety: Implementation of this interface must be thread safe.</para>
    /// </summary>
    public interface UserResources
    {
        /// <summary>
        /// <para>Gets the list of Users in the organization. To filter by email, use the optional email query string
        /// parameter to specify a list of users’ email addresses.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: GET /Users</para>
        /// </summary>
        /// <param name="emails">list of email addresses on which to filter the results</param>
        /// <param name="includes">elements to include in response</param>
        /// <param name="paging"> the pagination</param>
        /// <param name="displayContributorSeatType">if true, VIEWER seat types are returned as CONTRIBUTOR</param>
        /// <returns> the list of all Users </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        PaginatedResult<User> ListUsers(IEnumerable<string> emails, IEnumerable<ListUserInclusion>? includes = null, PaginationParameters? paging = null, bool? displayContributorSeatType = null);

        /// <summary>
        /// <para>Gets the list of Users in the organization with plan and seat type filtering.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: GET /2.0/users</para>
        /// </summary>
        /// <param name="emails">list of email addresses on which to filter the results</param>
        /// <param name="planId">plan ID to filter users</param>
        /// <param name="seatType">seat type to filter users</param>
        /// <param name="paging">the pagination</param>
        /// <param name="displayContributorSeatType">if true, VIEWER seat types are returned as CONTRIBUTOR</param>
        /// <returns>the list of filtered Users</returns>
        /// <exception cref="System.InvalidOperationException">if any argument is null or empty string</exception>
        /// <exception cref="InvalidRequestException">if there is any problem with the REST API request</exception>
        /// <exception cref="AuthorizationException">if there is any problem with the REST API authorization (access token)</exception>
        /// <exception cref="ResourceNotFoundException">if the resource cannot be found</exception>
        /// <exception cref="ServiceUnavailableException">if the REST API service is not available (possibly due to rate limiting)</exception>
        /// <exception cref="SmartsheetException">if there is any other error during the operation</exception>
        PaginatedResult<User> ListUsers(IEnumerable<string> emails, long? planId, SeatType? seatType, PaginationParameters? paging = null, bool? displayContributorSeatType = null);

        /// <summary>
        /// <para>Add a user to the organization</para>
        /// <para>It mirrors to the following Smartsheet REST API method: POST /Users</para>
        /// </summary>
        /// <param name="user"> the user </param>
        /// <param name="sendEmail"> flag indicating whether or not to send a welcome email. Defaults to false. </param>
        /// <param name="allowInviteAccountAdmin">if user is an admin in another organization, setting to true will invite their entire organization.</param>
        /// <returns> the created user </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        User AddUser(User user, bool? sendEmail = null, bool? allowInviteAccountAdmin = null);

        /// <summary>
        /// <para>Get the current user.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: GET /users/me</para>
        /// </summary>
        /// <returns> the current user </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        UserProfile GetCurrentUser();

        /// <summary>
        /// <para>Get the current user.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: GET /users/me</para>
        /// </summary>
        /// <param name="includes">used to specify the optional objects to include.</param>
        /// <returns> the current user </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        UserProfile GetCurrentUser(IEnumerable<UserInclusion>? includes = null);

        /// <summary>
        /// <para>Gets the user.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: GET /users/{userId}</para>
        /// </summary>
        /// <param name="userId"> the user Id </param>
        /// <returns> the user </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        UserProfile GetUser(long userId);

        /// <summary>
        /// <para>Update a user.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: PUT /users/{userId}</para>
        /// </summary>
        /// <param name="user"> the user to update </param>
        /// <returns> the updated user </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        User UpdateUser(User user);

        /// <summary>
        /// <para>Fetch all user's plans.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: GET /2.0/users/{userId}/plans</para>
        /// </summary>
        /// <param name="userId">The ID of the user to fetch plans for.</param>
        /// <param name="lastKey">The last key for pagination.</param>
        /// <param name="maxItems">The maximum number of items to return.</param>
        /// <param name="displayContributorSeatType">if true, VIEWER seat types are returned as CONTRIBUTOR</param>
        /// <param name="include">
        /// <para>used to specify the optional objects to include, currently PLAN_NAME is supported.</para>
        /// <para>
        /// When PLAN_NAME is included, each returned plan carries the name of its owning
        /// organization in <see cref="UserPlan.PlanName"/>. Organization names are cached
        /// server-side for several hours, so a recently renamed organization may briefly
        /// return its previous name.
        /// </para>
        /// </param>
        /// <returns><see cref="TokenPaginatedResult{T}"/> object containing <see cref="UserPlan"/>.</returns>
        /// <exception cref="System.InvalidOperationException">If any argument is null or empty string.</exception>
        /// <exception cref="InvalidRequestException">If there is any problem with the REST API request.</exception>
        /// <exception cref="AuthorizationException">If there is any problem with the REST API authorization.</exception>
        /// <exception cref="ResourceNotFoundException">If the user cannot be found (404 Not Found).</exception>
        /// <exception cref="ServiceUnavailableException">If the REST API service is not available (possibly due to rate limiting or 500 Internal Server Error).</exception>
        /// <exception cref="SmartsheetException">If there is any other error during the operation.</exception>
        TokenPaginatedResult<UserPlan> ListUserPlans(long userId, string? lastKey, long? maxItems, bool? displayContributorSeatType = null, IEnumerable<UserPlanInclusion>? include = null);

        /// <summary>
        /// <para>Removes a user from a plan.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: DELETE /2.0/users/{userId}/plans/{planId}</para>
        /// </summary>
        /// <param name="userId">The ID of the user to remove from the plan.</param>
        /// <param name="planId">The ID of the plan to remove the user from.</param>
        /// <returns>void</returns>
        /// <exception cref="System.InvalidOperationException">If any argument is null or empty string.</exception>
        /// <exception cref="InvalidRequestException">If there is any problem with the REST API request.</exception>
        /// <exception cref="AuthorizationException">If there is any problem with the REST API authorization.</exception>
        /// <exception cref="ResourceNotFoundException">If the user cannot be found (404 Not Found).</exception>
        /// <exception cref="ServiceUnavailableException">If the REST API service is not available (possibly due to rate limiting or 500 Internal Server Error).</exception>
        /// <exception cref="SmartsheetException">If there is any other error during the operation.</exception>
        void RemoveUserFromPlan(long userId, long planId);

        /// <summary>
        /// <para>Upgrades a user's seat type within your Smartsheet organization or plan to a licensed Member or Guest.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: POST /users/{userId}/plans/{planId}/upgrade</para>
        /// </summary>
        /// <param name="userId">The ID of the user to upgrade.</param>
        /// <param name="planId">The ID of the plan.</param>
        /// <param name="seatType">The seat type to upgrade to ("MEMBER" or "GUEST").</param>
        /// <returns>void</returns>
        /// <exception cref="System.InvalidOperationException">If any argument is null or empty string.</exception>
        /// <exception cref="InvalidRequestException">If there is any problem with the REST API request.</exception>
        /// <exception cref="AuthorizationException">If there is any problem with the REST API authorization.</exception>
        /// <exception cref="ResourceNotFoundException">If the user cannot be found (404 Not Found).</exception>
        /// <exception cref="ServiceUnavailableException">If the REST API service is not available (possibly due to rate limiting or 500 Internal Server Error).</exception>
        /// <exception cref="SmartsheetException">If there is any other error during the operation.</exception>
        void UpgradeUser(long userId, long planId, UpgradeSeatType? seatType);

        /// <summary>
        /// <para>Downgrades a user's seat type within your Smartsheet organization or plan from a licensed Member, Provisional Member or Guest to a non-licensed Viewer or Guest.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: POST /users/{userId}/plans/{planId}/downgrade</para>
        /// </summary>
        /// <param name="userId">The ID of the user to downgrade.</param>
        /// <param name="planId">The ID of the plan.</param>
        /// <param name="seatType">The seat type to downgrade to ("VIEWER" or "GUEST").</param>
        /// <returns>void</returns>
        /// <exception cref="System.InvalidOperationException">If any argument is null or empty string.</exception>
        /// <exception cref="InvalidRequestException"> User is not eligible for downgrade (e.g., not Active, not Member/Provisional/Guest, or is an admin and removeAdminStatus is false).</exception>
        /// <exception cref="AuthorizationException"> Authentication failed or token missing.</exception>
        /// <exception cref="ResourceNotFoundException">User not found.</exception>
        /// <exception cref="ServiceUnavailableException">Unexpected error on the server.</exception>
        /// <exception cref="SmartsheetException">If there is any other error during the operation.</exception>
        void DowngradeUser(long userId, long planId, DowngradeSeatType seatType);

        /// <summary>
        /// <para>Reactivates the user associated with the current Smartsheet plan, restoring the user's access to Smartsheet, owned items, and shared items.</para>
        /// <para>Optionally, with Enterprise Plan Manager (EPM) enabled, you can specify the ID of a user within your managed plan hierarchy.</para>
        /// <para>Important: You can reactivate the user only if that user has been deactivated for less than thirty (30) days.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: POST /users/{userId}/reactivate</para>
        /// </summary>
        /// <param name="userId">The ID of the user to reactivate.</param>
        /// <returns>void</returns>
        /// <exception cref="System.InvalidOperationException">If any argument is null or empty string.</exception>
        /// <exception cref="InvalidRequestException">User is not eligible for reactivation (e.g., email belongs to ISP domain, email unassociated with plan domain, user not in organization, or deactivated for more than 30 days).</exception>
        /// <exception cref="AuthorizationException">Authentication failed or token missing. Requires System Admin permissions.</exception>
        /// <exception cref="ResourceNotFoundException">User not found.</exception>
        /// <exception cref="ServiceUnavailableException">Unexpected error on the server.</exception>
        /// <exception cref="SmartsheetException">If there is any other error during the operation.</exception>
        void ReactivateUser(long userId);

        /// <summary>
        /// <para>Deactivates the user associated with the current Smartsheet plan, blocking the user from using Smartsheet in any way. Deactivating a user does not affect their existing permissions on owned or shared items.</para>
        /// <para>Optionally, with Enterprise Plan Manager (EPM) enabled, you can deactivate a user from child organizations.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: POST /users/{userId}/deactivate</para>
        /// </summary>
        /// <param name="userId">The ID of the user to deactivate.</param>
        /// <returns>void</returns>
        /// <exception cref="System.InvalidOperationException">If any argument is null or empty string.</exception>
        /// <exception cref="InvalidRequestException">User is not eligible for deactivation (e.g., email belongs to ISP domain, email unassociated with plan domain, or user is managed by external source like IdP or directory integration).</exception>
        /// <exception cref="AuthorizationException">Authentication failed or token missing. Requires System Admin permissions.</exception>
        /// <exception cref="ResourceNotFoundException">User not found.</exception>
        /// <exception cref="ServiceUnavailableException">Unexpected error on the server.</exception>
        /// <exception cref="SmartsheetException">If there is any other error during the operation.</exception>
        void DeactivateUser(long userId);

        /// <summary>
        /// <para>Removes a User from an organization. User is transitioned to a free collaborator with read-only access to owned sheets (unless those are optionally transferred to another user).</para>
        /// <remarks>This operation is only available to system administrators.</remarks>
        /// <para>It mirrors to the following Smartsheet REST API method: DELETE /user{Id}</para>
        /// </summary>
        /// <param name="userId"> the Id of the user </param>
        /// <param name="transferTo">(required if user owns groups): The ID of the user to transfer ownership to. 
        /// If the user being deleted owns groups, they will be transferred to this user. 
        /// If the user owns sheets, and transferSheets is true, then the deleted user’s sheets will be transferred to this user.</param>
        /// <param name="transferSheets">If true, and transferTo is specified, the deleted user’s sheets will be transferred. Else, sheets will not be transferred. Defaults to false.</param>
        /// <param name="removeFromSharing">Set to true to remove the user from sharing for all sheets/workspaces in the organization. If not specified, User will not be removed from sharing.</param>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        void RemoveUser(long userId, long? transferTo = null, bool? transferSheets = null, bool? removeFromSharing = null);

        /// <summary>
        /// <para>List all user alternate email(s).</para>
        /// <para>It mirrors to the following Smartsheet REST API method: GET /users/{userId}/alternateemails</para>
        /// </summary>
        /// <param name="userId"> the Id of the user </param>
        /// <param name="pagination"> the pagination</param>
        /// <returns> the list of all AlternateEmails </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        PaginatedResult<AlternateEmail> ListAlternateEmails(long userId, PaginationParameters? pagination = null);

        /// <summary>
        /// <para>Get alternate email.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: GET /users/{userId}/alternateemails/{alternateEmailId}</para>
        /// </summary>
        /// <param name="userId"> the Id of the user </param>
        /// <param name="altEmailId"> the alternate email Id</param>
        /// <returns>
        /// Return the AlternateEmail (note that if there is no such resource, this method will throw 
        /// ResourceNotFoundException rather than returning null) 
        /// </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        AlternateEmail GetAlternateEmail(long userId, long altEmailId);

        /// <summary>
        /// <para>Add alternate email(s).</para>
        /// <para>It mirrors to the following Smartsheet REST API method: POST /users/{userId}/alternateemails</para>
        /// </summary>
        /// <param name="userId"> the Id of the user </param>
        /// <param name="altEmails"> list of AlternateEmail(s)</param>
        /// <returns>
        /// Return the list of AlternateEmails (note that if there is no such resource, this method will throw 
        /// ResourceNotFoundException rather than returning null) 
        /// </returns>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        IList<AlternateEmail> AddAlternateEmail(long userId, IEnumerable<AlternateEmail> altEmails);

        /// <summary>
        /// <para>Delete alternate email.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: DELETE /users/{userId}/alternateemails/{alternateEmailId}</para>
        /// </summary>
        /// <param name="userId"> the Id of the user </param>
        /// <param name="altEmailId"> the alternate email Id</param>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        void DeleteAlternateEmail(long userId, long altEmailId);

        /// <summary>
        /// <para>Promote an alternate email to primary.</para>
        /// <para>It mirrors to the following Smartsheet REST API method: POST /users/{userId}/alternateemails/{alternateEmailId}/makeprimary</para>
        /// </summary>
        /// <param name="userId"> the Id of the user </param>
        /// <param name="altEmailId"> the alternate email Id</param>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        AlternateEmail PromoteAlternateEmail(long userId, long altEmailId);

        /// <summary>
        /// <para>Uploads a profile image for the specified user.</para>
        /// </summary>
        /// <param name="userId"> the Id of the user </param>
        /// <param name="file"> path to the image file</param>
        /// <param name="fileType">fileType content type of the image file</param>
        /// <exception cref="System.InvalidOperationException"> if any argument is null or empty string </exception>
        /// <exception cref="InvalidRequestException"> if there is any problem with the REST API request </exception>
        /// <exception cref="AuthorizationException"> if there is any problem with  the REST API authorization (access token) </exception>
        /// <exception cref="ResourceNotFoundException"> if the resource cannot be found </exception>
        /// <exception cref="ServiceUnavailableException"> if the REST API service is not available (possibly due to rate limiting) </exception>
        /// <exception cref="SmartsheetException"> if there is any other error during the operation </exception>
        User AddProfileImage(long userId, string file, string? fileType = null);

        /// <summary>
        /// <para>Return the UserSheetResources object that provides access to sheets resources associated with
        /// User resources.</para>
        /// </summary>
        /// <returns> the associated discussion resources </returns>
        UserSheetResources SheetResources { get; }
    }
}
