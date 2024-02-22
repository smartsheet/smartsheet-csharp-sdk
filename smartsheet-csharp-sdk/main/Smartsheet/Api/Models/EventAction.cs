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
    /// Enum encapsulating event actions.
    /// </summary>
    public enum EventAction
    {
        /// <summary>
        /// Create action
        /// </summary>
        CREATE,
        /// <summary>
        /// Update action
        /// </summary>
        UPDATE,
        /// <summary>
        /// Load action
        /// </summary>
        LOAD,
        /// <summary>
        /// Delete action
        /// </summary>
        DELETE,
        /// <summary>
        /// Purge action
        /// </summary>
        PURGE,
        /// <summary>
        /// Restore action
        /// </summary>
        RESTORE,
        /// <summary>
        /// Rename action
        /// </summary>
        RENAME,
        /// <summary>
        /// Activate action
        /// </summary>
        ACTIVATE,
        /// <summary>
        /// Deactivate action
        /// </summary>
        DEACTIVATE,
        /// <summary>
        /// Export action
        /// </summary>
        EXPORT,
        /// <summary>
        /// Move action
        /// </summary>
        MOVE,
        /// <summary>
        /// Move row action
        /// </summary>
        MOVE_ROW,
        /// <summary>
        /// Copy row action
        /// </summary>
        COPY_ROW,
        /// <summary>
        /// Save as new action
        /// </summary>
        SAVE_AS_NEW,
        /// <summary>
        /// Save as new template action
        /// </summary>
        SAVE_AS_TEMPLATE,
        /// <summary>
        /// Add publish action
        /// </summary>
        ADD_PUBLISH,
        /// <summary>
        /// Remove publish action
        /// </summary>
        REMOVE_PUBLISH,
        /// <summary>
        /// Add share action
        /// </summary>
        ADD_SHARE,
        /// <summary>
        /// Remove share action
        /// </summary>
        REMOVE_SHARE,
        /// <summary>
        /// Add share member action
        /// </summary>
        ADD_SHARE_MEMBER,
        /// <summary>
        /// Remove share member action
        /// </summary>
        REMOVE_SHARE_MEMBER,
        /// <summary>
        /// Add workspace share action
        /// </summary>
        ADD_WORKSPACE_SHARE,
        /// <summary>
        /// Remove workspace share action
        /// </summary>
        REMOVE_WORKSPACE_SHARE,
        /// <summary>
        /// Add member action
        /// </summary>
        ADD_MEMBER,
        /// <summary>
        /// Remove member action
        /// </summary>
        REMOVE_MEMBER,
        /// <summary>
        /// Transfer ownership action
        /// </summary>
        TRANSFER_OWNERSHIP,
        /// <summary>
        /// Create cell link action
        /// </summary>
        CREATE_CELL_LINK,
        /// <summary>
        /// Remove shares action
        /// </summary>
        REMOVE_SHARES,
        /// <summary>
        /// Transfer owned groups action
        /// </summary>
        TRANSFER_OWNED_GROUPS,
        /// <summary>
        /// Transfer owned items action
        /// </summary>
        TRANSFER_OWNED_ITEMS,
        /// <summary>
        /// Download sheet access report action
        /// </summary>
        DOWNLOAD_SHEET_ACCESS_REPORT,
        /// <summary>
        /// Download user list action
        /// </summary>
        DOWNLOAD_USER_LIST,
        /// <summary>
        /// Download login history action
        /// </summary>
        DOWNLOAD_LOGIN_HISTORY,
        /// <summary>
        /// Download published items report action
        /// </summary>
        DOWNLOAD_PUBLISHED_ITEMS_REPORT,
        /// <summary>
        /// Update main contact action
        /// </summary>
        UPDATE_MAIN_CONTACT,
        /// <summary>
        /// Import users action
        /// </summary>
        IMPORT_USERS,
        /// <summary>
        /// Bulk update action
        /// </summary>
        BULK_UPDATE,
        /// <summary>
        /// List sheets action
        /// </summary>
        LIST_SHEETS,
        /// <summary>
        /// Requests backup action
        /// </summary>
        REQUEST_BACKUP,
        /// <summary>
        /// Create recurring backup action
        /// </summary>
        CREATE_RECURRING_BACKUP,
        /// <summary>
        /// Update recurring backup action
        /// </summary>
        UPDATE_RECURRING_BACKUP,
        /// <summary>
        /// Delete recurring backup action
        /// </summary>
        DELETE_RECURRING_BACKUP,
        /// <summary>
        /// Remove from groups action
        /// </summary>
        REMOVE_FROM_GROUPS,
        /// <summary>
        /// Send as attachment action
        /// </summary>
        SEND_AS_ATTACHMENT,
        /// <summary>
        /// Send row action
        /// </summary>
        SEND_ROW,
        /// <summary>
        /// Send action
        /// </summary>
        SEND,
        /// <summary>
        /// Send comment action
        /// </summary>
        SEND_COMMENT,
        /// <summary>
        /// Send invite action
        /// </summary>
        SEND_INVITE,
        /// <summary>
        /// Decline invite action
        /// </summary>
        DECLINE_INVITE,
        /// <summary>
        /// Accept invite action
        /// </summary>
        ACCEPT_INVITE,
        /// <summary>
        /// Send password reset action
        /// </summary>
        SEND_PASSWORD_RESET,
        /// <summary>
        /// Remove from account action
        /// </summary>
        REMOVE_FROM_ACCOUNT,
        /// <summary>
        /// Add to account action
        /// </summary>
        ADD_TO_ACCOUNT,
        /// <summary>
        /// Authorize action
        /// </summary>
        AUTHORIZE,
        /// <summary>
        /// Revoke action
        /// </summary>
        REVOKE
        
    }
}