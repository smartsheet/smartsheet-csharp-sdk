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

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Represents one specific RequestResult of a search.
    /// </summary>
    public class SearchResultItem
    {
        /// <summary>
        /// Represents the object Id for this specific search RequestResult.
        /// </summary>
        private long? objectId;

        /// <summary>
        /// Represents the parent object Id for this specific search RequestResult.
        /// </summary>
        private long? parentObjectId;

        /// <summary>
        /// Represents the object type (row, discussion, attach) for this specific search RequestResult.
        /// </summary>
        private SearchObjectType? objectType;

        /// <summary>
        /// Represents the parent object type for this specific search RequestResult.
        /// </summary>
        private ObjectType? parentObjectType;

        /// <summary>
        /// Represents the attachment type if the search result item is an attachment
        /// </summary>
        private AttachmentType? attachmentType;

        /// <summary>
        /// Represents the context data for this specific search RequestResult.
        /// </summary>
        private IList<string> contextData;

        /// <summary>
        /// If the search result item is a favorite
        /// </summary>
        private bool? favorite;

        /// <summary>
        /// Represents the MIME type
        /// </summary>
        private string mimeType;

        /// <summary>
        /// If the parent object of the search item is a favorite
        /// </summary>
        private bool? parentObjectFavorite;

        /// <summary>
        /// Represents the parent object name for this specific search RequestResult.
        /// </summary>
        private string parentObjectName;

        /// <summary>
        /// Represents the text for this specific search RequestResult.
        /// </summary>
        private string text;

        /// <summary>
        /// Gets the object Id for this specific search RequestResult.
        /// </summary>
        /// <returns> the object Id </returns>
        public long? ObjectId
        {
            get { return objectId; }
            set { objectId = value; }
        }

        /// <summary>
        /// Gets the parent object Id for this specific search RequestResult.
        /// </summary>
        /// <returns> the parent object Id </returns>
        public long? ParentObjectId
        {
            get { return parentObjectId; }
            set { parentObjectId = value; }
        }

        /// <summary>
        /// Gets the object type for this specific search RequestResult.
        /// </summary>
        /// <returns> the object type </returns>
        public SearchObjectType? ObjectType
        {
            get { return objectType; }
            set { objectType = value; }
        }
        
        /// <summary>
        /// Gets the parent object type for this specific search RequestResult.
        /// </summary>
        /// <returns> the parent object type </returns>
        public ObjectType? ParentObjectType
        {
            get { return parentObjectType; }
            set { parentObjectType = value; }
        }
        
        /// <summary>
        /// Gets the attachment type if the search result item is an attachment
        /// </summary>
        /// <returns> the attachment type </returns>
        public AttachmentType? AttachmentType
        {
            get { return attachmentType; }
            set { attachmentType = value; }
        }

        /// <summary>
        /// Gets the context data for this specific search RequestResult.
        /// </summary>
        /// <returns> the context data </returns>
        public IList<string> ContextData
        {
            get { return contextData; }
            set { contextData = value; }
        }

        /// <summary>
        /// Indicates whether the search result item is a favorite
        /// </summary>
        /// <returns> the favorite flag </returns>
        public bool? Favorite
        {
            get { return favorite; }
            set { favorite = value; }
        }

        /// <summary>
        /// Gets the MIME type
        /// </summary>
        /// <returns> the MIME type </returns>
        public string MimeType
        {
            get { return mimeType; }
            set { mimeType = value; }
        }

        /// <summary>
        /// Indicates whether the search result item parent is a favorite
        /// </summary>
        public bool? ParentObjectFavorite
        {
            get { return parentObjectFavorite; }
            set { parentObjectFavorite = value; }
        }

        /// <summary>
        /// Gets the parent object name for this specific search RequestResult.
        /// </summary>
        /// <returns> the parent object name </returns>
        public string ParentObjectName
        {
            get { return parentObjectName; }
            set { parentObjectName = value; }
        }

        /// <summary>
        /// Gets the text for this specific search RequestResult.
        /// </summary>
        /// <returns> the text </returns>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        // Fields added for the unified search-service response

        private string workspaceId;
        private string containerId;
        private long? modifyDateTime;
        private string primaryColumnCellText;
        private string attachmentSource;
        private string attachmentDescription;
        private bool? isTemplate;

        /// <summary>ID of the workspace containing this result.</summary>
        public string WorkspaceId { get { return workspaceId; } set { workspaceId = value; } }

        /// <summary>ID of the direct container (sheet or folder) for this result.</summary>
        public string ContainerId { get { return containerId; } set { containerId = value; } }

        /// <summary>Last-modified timestamp in milliseconds since the Unix epoch (UTC).</summary>
        public long? ModifyDateTime { get { return modifyDateTime; } set { modifyDateTime = value; } }

        /// <summary>Primary-column cell text. Only populated for GRID_ROW results.</summary>
        public string PrimaryColumnCellText { get { return primaryColumnCellText; } set { primaryColumnCellText = value; } }

        /// <summary>Object type the attachment belongs to. Only populated for ATTACHMENT results.</summary>
        public string AttachmentSource { get { return attachmentSource; } set { attachmentSource = value; } }

        /// <summary>User-provided attachment description. Only populated for ATTACHMENT results.</summary>
        public string AttachmentDescription { get { return attachmentDescription; } set { attachmentDescription = value; } }

        /// <summary>True if this sheet result is a template. Only populated for SHEET results.</summary>
        public bool? IsTemplate { get { return isTemplate; } set { isTemplate = value; } }
    }
}
