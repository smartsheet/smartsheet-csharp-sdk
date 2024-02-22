//    #[license]
//    SmartsheetClient SDK for C#
//    %%
//    Copyright (C) 2019 SmartsheetClient
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

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Enum to hold what EventObject is being used in event reporting.
    /// See source https://smartsheet-platform.github.io/event-reporting-docs/
    /// </summary>
    public enum EventObjectType
    {
        /// <summary>
        /// Access token event object type: Authorize, refresh, revoke
        /// </summary>
        ACCESS_TOKEN,
        /// <summary>
        /// Account event object type: Bulk_update, downloads, import users, list sheets, rename, update main contact
        /// </summary>
        ACCOUNT,
        /// <summary>
        /// Attachment event object type: create, update, load, delete, send
        /// </summary>
        ATTACHMENT,
        /// <summary>
        /// Dashboard event object type: create, delete, load, add/remove publish, add share(s)
        /// Remove share(s), transfer ownership, move, purge, rename, restore, save as new, update
        /// </summary>
        DASHBOARD,
        /// <summary>
        /// Discussion event object type: create, delete, update, send, send comment
        /// </summary>
        DISCUSSION,
        /// <summary>
        /// Folder event object type: create, rename, save as new, delete, request backup, export
        /// </summary>
        FOLDER,
        /// <summary>
        /// Form event object type: create, update, deactivate, activate, delete
        /// </summary>
        FORM,
        /// <summary>
        /// Group event object type: create, download access report, rename, update, delete, transfer ownership
        /// add member, remove member
        /// </summary>
        GROUP,
        /// <summary>
        /// Sheet event object type: create, update, load, delete, rename, purge, restore, add share(s)
        /// Remove share(s), transfer ownership, save as new, save as new template, send/move/copy row
        /// create cell link, move, export, request backup.
        /// </summary>
        SHEET,
        /// <summary>
        /// Report event object type: create, update, load, delete, rename, purge, restore, add share(s)
        /// Remove share(s), transfer ownership, save as new, move, export, save as attachment.
        /// </summary>
        REPORT,
        /// <summary>
        /// Update request event object type: create
        /// </summary>
        UPDATE_REQUEST,
        /// <summary>
        /// User event object type: add to account, accept invite, decline invite, send invite, download access reports
        /// remove from groups, remove shares, remove from account, send password reset, transfer owned items, 
        /// transfer owned groups, update user.
        /// </summary>
        USER,
        /// <summary>
        /// Workspace event object type: create, rename, save as new, delete, add share, remove share, 
        /// transfer ownership, create/update/delete recurring backup, request backup, export.
        /// </summary>
        WORKSPACE
    }
}
