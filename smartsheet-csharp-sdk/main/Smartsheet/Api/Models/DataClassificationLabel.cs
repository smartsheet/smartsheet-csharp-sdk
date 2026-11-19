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
    /// Represents a single data classification label.
    /// </summary>
    public class DataClassificationLabel
    {
        /// <summary>
        /// Gets or sets the label ID (UUID string).
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the display name of the label.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the label.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the hex color code for the label (e.g. "#ffe0e3").
        /// </summary>
        public string? Color { get; set; }

        /// <summary>
        /// Gets or sets the sensitivity order (lower = more sensitive).
        /// </summary>
        public int? SensitivityOrder { get; set; }

        /// <summary>
        /// Gets or sets whether this label is the default classification.
        /// </summary>
        public bool? IsDefault { get; set; }
    }
}
