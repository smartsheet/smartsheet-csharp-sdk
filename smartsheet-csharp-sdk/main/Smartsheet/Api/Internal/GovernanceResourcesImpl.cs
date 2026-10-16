using System.Collections.Generic;
using Smartsheet.Api.Models;
using Smartsheet.Api.Internal.Util;

namespace Smartsheet.Api.Internal
{
    /// <summary>
    /// Implementation of GovernanceResources.
    /// </summary>
    public class GovernanceResourcesImpl : AbstractResources, GovernanceResources
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="smartsheet"> the SmartsheetImpl </param>
        public GovernanceResourcesImpl(SmartsheetImpl smartsheet) : base(smartsheet) { }

        /// <summary>
        /// Gets the data classification settings for a plan.
        /// </summary>
        /// <param name="planId"> the plan ID </param>
        /// <returns> the data classification settings </returns>
        public virtual DataClassificationSettings GetDataClassificationSettings(long planId)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("planId", planId.ToString());
            return GetResource<DataClassificationSettings>(
                "governance/data-classification/settings" + QueryUtil.GenerateUrl(null, parameters),
                typeof(DataClassificationSettings));
        }

        /// <summary>
        /// Gets the data classification settings by resolving the plan from an asset.
        /// Supported asset types: <see cref="AssetType.SHEET"/>, <see cref="AssetType.REPORT"/>, <see cref="AssetType.SIGHT"/> (dashboard).
        /// </summary>
        /// <param name="assetType"> the type of the asset </param>
        /// <param name="assetId"> the ID of the asset </param>
        /// <returns> the data classification settings </returns>
        public virtual DataClassificationSettings GetDataClassificationSettings(AssetType assetType, long assetId)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("assetType", assetType.ToString().ToLower());
            parameters.Add("assetId", assetId.ToString());
            return GetResource<DataClassificationSettings>(
                "governance/data-classification/settings" + QueryUtil.GenerateUrl(null, parameters),
                typeof(DataClassificationSettings));
        }
    }
}
