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

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Represents object types.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ObjectType
    {
        // ObjectType must also be in all lower case when building the path.
        // Below, the EnumMembers turn the enums into lowercase only during serialization into JSON object
        /// <summary>
        /// Workspace object type
        /// </summary>
        [EnumMember(Value = "workspace")]
        WORKSPACE,
        /// <summary>
        /// Folder object type
        /// </summary>
        [EnumMember(Value = "folder")]
        FOLDER,
        /// <summary>
        /// Sheet object type
        /// </summary>
        [EnumMember(Value = "sheet")]
        SHEET,
        /// <summary>
        /// Report object type
        /// </summary>
        [EnumMember(Value = "report")]
        REPORT,
        /// <summary>
        /// Template object type
        /// </summary>
        [EnumMember(Value = "template")]
        TEMPLATE,
        /// <summary>
        /// Sight/dashboard object type
        /// </summary>
        [EnumMember(Value = "sight")]
        SIGHT
    }

}