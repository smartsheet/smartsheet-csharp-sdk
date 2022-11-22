//    #[license]
//    SmartsheetClient SDK for C#
//    %%
//    Copyright (C) 2018 SmartsheetClient
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
using System.Linq;
using System.Text;

namespace Smartsheet.Api.Models
{
    public class ContactObjectValue : Contact, ObjectValue
    {
        /// <summary>
        /// Part of the ContactOverlay, if contactReferences is present in the sheet, refIndex will indicate
        /// the offset into the list containing this Contact
        /// </summary>
        private int? refIndex;

        /// <summary>
        /// ID of the contact image
        /// </summary>
        private string imageId;

        public ObjectValueType ObjectType
        {
            get { return ObjectValueType.CONTACT; }
        }

        /// <summary>
        /// Gets the offset in the contactReferences for this Contact
        /// </summary>
        public int? RefIndex
        {
            get { return refIndex; }
            set { refIndex = value; }
        }

        /// <summary>
        /// Gets the ID for the contact image
        /// </summary>
        public string ImageId
        {
            get { return imageId; }
            set { imageId = value; }
        }
    }
}
