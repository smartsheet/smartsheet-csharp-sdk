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
namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Represents an AssetShare Object. </summary>
    /// <seealso href="http://help.Smartsheet.com/customer/portal/articles/520104-sharing-Sheets">Sharing Sheets</seealso>
    public class AssetShare : NamedModel
    {
        //Since ID is alphanumeric.
        // We can either overide the ID of the NamedModel interface. 
        //Or
        // Update IdentifiableModel to IdentifiableModel<T> to accept either a long or string type.
        //For now, we are overiding ID.
        private string id;

        /// <summary>
        /// Represents the groupId if the share is of type GROUP
        /// </summary>
        private string groupId;

        /// <summary>
        /// Represents the userId if the share is of type USER
        /// </summary>
        private string userId;

        /// <summary>
        /// Indicates what type of share this is
        /// </summary>
        private ShareType? type;

        /// <summary>
        /// Represents the access level for this specific share.
        /// </summary>
        private AccessLevel? accessLevel;

        /// <summary>
        /// Represents the Email for this specific share.
        /// </summary>
        private string email;

        /// <summary>
        /// The scope of this share. One of the following values:
        ///        ITEM: an item-level share (i.e., the specific object to which the Share applies is shared with the user or group)
        ///        WORKSPACE: a workspace-level share (i.e., the workspace that contains the object to which the Share applies is shared with the user or group)
        /// </summary>
        private string scope;

        /// <summary>
        /// Share ID, unlike other Smartsheet object ids, this id is an alphanumeric string.
        /// </summary>
        public new string Id
        {
            // This should hide inherited member "Id".
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Group ID if the share is a group share, else null.
        /// </summary>
        public string GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        /// <summary>
        /// User ID if the share is a user share, else null.
        /// </summary>
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        /// <summary>
        /// The type of this share. One of USER or GROUP.
        /// </summary>
        public ShareType? Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Gets the access level for this specific share.
        /// </summary>
        /// <returns> the access level </returns>
        public AccessLevel? AccessLevel
        {
            get { return accessLevel; }
            set { accessLevel = value; }
        }

        /// <summary>
        /// Gets the Email for this specific share.
        /// </summary>
        /// <returns> the Email </returns>
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        /// <summary>
        /// The scope of this share. One of ITEM or WORKSPACE.
        /// </summary>
        public string Scope
        {
            get { return scope; }
            set { scope = value; }
        }

        /// <summary>
        /// A convenience class for creating a <seealso cref="AssetShare"/> with the necessary fields for sharing the sheet to one user.
        /// </summary>
        public class CreateAssetShareBuilder
        {
            private AccessLevel? accessLevel;
            private string email;
            private string groupId;

            /// <summary>
            /// Sets the required properties for sharing.
            /// Note: Since both email and groupId are strings in AssetShare, use SetEmail() or SetGroupId()
            /// to specify the recipient after construction.
            /// </summary>
            /// <param name="accessLevel">the Access Level</param>
            public CreateAssetShareBuilder(AccessLevel? accessLevel)
            {
                this.accessLevel = accessLevel;
            }

            /// <summary>
            /// (required) Access level for this specific share.
            /// </summary>
            /// <param name="accessLevel"> the access level </param>
            /// <returns> the share to one builder </returns>
            public CreateAssetShareBuilder SetAccessLevel(AccessLevel? accessLevel)
            {
                this.accessLevel = accessLevel;
                return this;
            }

            /// <summary>
            ///  (optional) Email address for this specific share.
            ///  NOTE: One of email or groupId must be specified, but not both.
            /// </summary>
            /// <param name="email"> the Email </param>
            /// <returns> the share to one builder </returns>
            public CreateAssetShareBuilder SetEmail(string email)
            {
                this.email = email;
                return this;
            }

            /// <summary>
            /// the group share recipient's group ID.
            /// NOTE: One of email or groupId must be specified, but not both.
            /// </summary>
            /// <param name="groupId"> the groupId </param>
            /// <returns> the share to one builder </returns>
            public CreateAssetShareBuilder SetGroupId(string groupId)
            {
                this.groupId = groupId;
                return this;
            }

            /// <summary>
            /// Gets the access level.
            /// </summary>
            /// <returns> the access level </returns>
            public AccessLevel? GetAccessLevel()
            {
                return accessLevel;
            }

            /// <summary>
            /// Gets the Email.
            /// </summary>
            /// <returns> the Email </returns>
            public string GetEmail()
            {
                return email;
            }

            /// <summary>
            /// Gets the GroupId.
            /// </summary>
            /// <returns> the GroupId </returns>
            public string GetGroupId()
            {
                return groupId;
            }

            /// <summary>
            /// Builds the <seealso cref="AssetShare"/> object.
            /// </summary>
            /// <returns> the share </returns>
            public AssetShare Build()
            {
                return new AssetShare()
                {
                    AccessLevel = accessLevel,
                    Email = email,
                    GroupId = groupId
                };
            }
        }

        /// <summary>
        /// A convenience class for creating a <seealso cref="AssetShare"/> with the necessary fields to update a specific share.
        /// </summary>
        public class UpdateAssetShareBuilder
        {
            private string shareId;
            private AccessLevel? accessLevel;

            /// <summary>
            /// Sets the required properties for updating a share object.
            /// </summary>
            /// <param name="shareId">the share Id</param>
            /// <param name="accessLevel">the Access Level</param>
            public UpdateAssetShareBuilder(string shareId, AccessLevel? accessLevel)
            {
                this.shareId = shareId;
                this.accessLevel = accessLevel;
            }

            /// <summary>
            /// Access level for the share.
            /// </summary>
            /// <param name="accessLevel"> the access level </param>
            /// <returns> the update share builder </returns>
            public UpdateAssetShareBuilder SetAccessLevel(AccessLevel? accessLevel)
            {
                this.accessLevel = accessLevel;
                return this;
            }

            /// <summary>
            /// Gets the access level.
            /// </summary>
            /// <returns> the access level </returns>
            public AccessLevel? GetAccessLevel()
            {
                return accessLevel;
            }

            /// <summary>
            /// Builds the <seealso cref="AssetShare"/> object.
            /// </summary>
            /// <returns> the share </returns>
            public AssetShare Build()
            {
                AssetShare share = new AssetShare();
                share.id = shareId;
                share.accessLevel = accessLevel;
                return share;
            }
        }
    }
}