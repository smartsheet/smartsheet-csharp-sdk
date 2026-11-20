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
using System.Threading;
using System.Threading.Tasks;
using Smartsheet.Api.Models;
using Smartsheet.Api.Internal.Util;

namespace Smartsheet.Api.Internal
{
    /// <summary>
    /// Implementation of GovernanceResources.
    /// </summary>
    public class GovernanceResourcesImpl : AbstractResources, GovernanceResources
    {
        private const string DataClassificationSettingsPath = "governance/data-classification/settings";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="smartsheet"> the SmartsheetImpl </param>
        public GovernanceResourcesImpl(SmartsheetImpl smartsheet) : base(smartsheet) { }

        /// <summary>
        /// <para>Gets the data classification settings for a plan.</para>
        ///
        /// <para>Mirrors to the following Smartsheet REST API method: GET /governance/data-classification/settings?planId={planId}</para>
        /// </summary>
        /// <param name="planId"> the ID of the plan whose classification settings to return </param>
        /// <returns> the data classification settings </returns>
        public virtual DataClassificationSettings GetDataClassificationSettings(long planId)
        {
            return this.GetDataClassificationSettingsAsync(planId).GetAwaiter().GetResult();
        }

        /// <summary>
        /// <para>Gets the data classification settings for a plan asynchronously.</para>
        ///
        /// <para>Mirrors to the following Smartsheet REST API method: GET /governance/data-classification/settings?planId={planId}</para>
        /// </summary>
        /// <param name="planId"> the ID of the plan whose classification settings to return </param>
        /// <param name="cancellationToken"> the cancellation token </param>
        /// <returns> the data classification settings </returns>
        public virtual async Task<DataClassificationSettings> GetDataClassificationSettingsAsync(long planId, CancellationToken cancellationToken = default)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("planId", planId.ToString());
            return await this.GetResourceAsync<DataClassificationSettings>(
                DataClassificationSettingsPath + QueryUtil.GenerateUrl(null, parameters),
                typeof(DataClassificationSettings),
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// <para>Gets the data classification settings for the plan of a sheet, report, or dashboard.</para>
        ///
        /// <para>Mirrors to the following Smartsheet REST API method: GET /governance/data-classification/settings?assetType={assetType}&amp;assetId={assetId}</para>
        /// </summary>
        /// <param name="assetType"> the type of the asset </param>
        /// <param name="assetId"> the ID of the sheet, report, or dashboard </param>
        /// <returns> the data classification settings </returns>
        public virtual DataClassificationSettings GetDataClassificationSettings(AssetType assetType, long assetId)
        {
            return this.GetDataClassificationSettingsAsync(assetType, assetId).GetAwaiter().GetResult();
        }

        /// <summary>
        /// <para>Gets the data classification settings for the plan of a sheet, report, or dashboard asynchronously.</para>
        ///
        /// <para>Mirrors to the following Smartsheet REST API method: GET /governance/data-classification/settings?assetType={assetType}&amp;assetId={assetId}</para>
        /// </summary>
        /// <param name="assetType"> the type of the asset </param>
        /// <param name="assetId"> the ID of the sheet, report, or dashboard </param>
        /// <param name="cancellationToken"> the cancellation token </param>
        /// <returns> the data classification settings </returns>
        public virtual async Task<DataClassificationSettings> GetDataClassificationSettingsAsync(AssetType assetType, long assetId, CancellationToken cancellationToken = default)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("assetType", assetType.ToString().ToLowerInvariant());
            parameters.Add("assetId", assetId.ToString());
            return await this.GetResourceAsync<DataClassificationSettings>(
                DataClassificationSettingsPath + QueryUtil.GenerateUrl(null, parameters),
                typeof(DataClassificationSettings),
                cancellationToken).ConfigureAwait(false);
        }
    }
}
