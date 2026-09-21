//    #[license]
//    SmartsheetClient SDK for C#
//    %%
//    Copyright (C) 2026 SmartsheetClient
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
    /// Represents the data classification of a sheet.
    /// </summary>
    public class SheetDataClassification
    {
        /// <summary>
        /// The data classification label.
        /// </summary>
        private string dataClassification;

        /// <summary>
        /// Gets or sets the data classification, which is a label from the plan's
        /// published classification labels (configured by a plan admin in Admin Center).
        /// </summary>
        public string DataClassification
        {
            get { return dataClassification; }
            set { dataClassification = value; }
        }
    }
}
