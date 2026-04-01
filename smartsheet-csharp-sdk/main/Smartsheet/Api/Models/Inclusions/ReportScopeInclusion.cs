//    #[license]
//    SmartsheetClient SDK for C#
//    %%
//    Copyright (C) 2016 SmartsheetClient
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
    /// An object representing a single asset (sheet or workspace) to be included in the scope of a report.
    /// </summary>
    public class ReportScopeInclusion
    {
        /// <summary>
        /// The asset type to be included in the scope of the report.
        /// 
        /// SHEET and WORKSPACE are the only supported asset types.
        /// </summary>
        public ReportAssetType AssetType { get; set; }

        /// <summary>
        /// The id of the asset according to its assetType.
        /// </summary>
        public long AssetId { get; set; }
    }
}
