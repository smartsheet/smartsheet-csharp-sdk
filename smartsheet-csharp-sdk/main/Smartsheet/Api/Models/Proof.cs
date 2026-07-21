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

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Represents the Proof object. A proof is a container that holds attachments and comments for review,
    /// editing, or approval.
    /// </summary>
    public class Proof : NamedModel
    {
        /// <summary>
        /// Represents the ID of the original proof version.
        /// </summary>
        private long? originalId;

        /// <summary>
        /// Represents the Type of the proof.
        /// </summary>
        private ProofType? type;

        /// <summary>
        /// Represents the document Type of the proof.
        /// </summary>
        private string documentType;

        /// <summary>
        /// Represents the URL used to request the proof.
        /// </summary>
        private string proofRequestUrl;

        /// <summary>
        /// Represents the version number of the proof.
        /// </summary>
        private int? version;

        /// <summary>
        /// Represents the date and time the proof was last updated.
        /// </summary>
        private DateTime? lastUpdatedAt;

        /// <summary>
        /// User object containing name and email of the user who last updated the proof.
        /// </summary>
        private User lastUpdatedBy;

        /// <summary>
        /// Indicates whether the proof is completed.
        /// </summary>
        private bool? isCompleted;

        /// <summary>
        /// Represents the Attachments associated with the proof.
        /// </summary>
        private IList<Attachment> attachments;

        /// <summary>
        /// Represents the Discussions associated with the proof.
        /// </summary>
        private IList<Discussion> discussions;

        /// <summary>
        /// Gets the ID of the original proof version.
        /// </summary>
        /// <returns> the original Id </returns>
        public long? OriginalId
        {
            get { return originalId; }
            set { originalId = value; }
        }

        /// <summary>
        /// Gets the Type of the proof.
        /// </summary>
        /// <returns> the proof Type </returns>
        public ProofType? Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Gets the document Type of the proof.
        /// </summary>
        /// <returns> the document Type </returns>
        public string DocumentType
        {
            get { return documentType; }
            set { documentType = value; }
        }

        /// <summary>
        /// Gets the URL used to request the proof.
        /// </summary>
        /// <returns> the proof request URL </returns>
        public string ProofRequestUrl
        {
            get { return proofRequestUrl; }
            set { proofRequestUrl = value; }
        }

        /// <summary>
        /// Gets the version number of the proof.
        /// </summary>
        /// <returns> the version </returns>
        public int? Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// Gets the date and time the proof was last updated.
        /// </summary>
        /// <returns> the last updated at timestamp </returns>
        public DateTime? LastUpdatedAt
        {
            get { return lastUpdatedAt; }
            set { lastUpdatedAt = value; }
        }

        /// <summary>
        /// Gets the user who last updated the proof.
        /// </summary>
        /// <returns> the last updated by user </returns>
        public User LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }

        /// <summary>
        /// Gets whether the proof is completed.
        /// </summary>
        /// <returns> the completed status </returns>
        public bool? IsCompleted
        {
            get { return isCompleted; }
            set { isCompleted = value; }
        }

        /// <summary>
        /// Gets the Attachments associated with the proof.
        /// </summary>
        /// <returns> the Attachments </returns>
        public IList<Attachment> Attachments
        {
            get { return attachments; }
            set { attachments = value; }
        }

        /// <summary>
        /// Gets the Discussions associated with the proof.
        /// </summary>
        /// <returns> the Discussions </returns>
        public IList<Discussion> Discussions
        {
            get { return discussions; }
            set { discussions = value; }
        }
    }
}
