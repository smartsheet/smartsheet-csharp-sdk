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

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Represents the request object for sharing an asset with a user or group.
    /// One (and only one) of email or groupId is required (alongside accessLevel).
    /// </summary>
    public class CreateShareRequest
    {
        /// <summary>
        /// Gets or sets the primary email address of a user to share to.
        /// Must be provided if GroupId is not provided.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the ID of the group to share to.
        /// Must be provided if Email is not provided.
        /// </summary>
        public long? GroupId { get; set; }

        /// <summary>
        /// Gets or sets the access level for this specific share. Required.
        /// </summary>
        public AccessLevel AccessLevel { get; set; }

        /// <summary>
        /// Gets or sets the subject of the email that is optionally sent to notify the recipient.
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>
        /// Gets or sets the message included in the body of the email that is optionally sent to the recipient.
        /// </summary>
        public string? Message { get; set; }
        
        /// <summary>
        /// Gets or Sets whether to CC yourself in emails.
        /// </summary>
        public bool? CcMe { get; set; }
    }
}
