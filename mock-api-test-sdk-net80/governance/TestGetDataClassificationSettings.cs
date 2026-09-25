using System.Web;
using Newtonsoft.Json;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestGetDataClassificationSettings
    {
        private const long TEST_PLAN_ID = 1148023251199876L;
        private const long TEST_ORG_ID = 1556806293055364L;
        private const long TEST_ASSET_ID = 112398785741L;
        private const string TEST_PATH = "/2.0/governance/data-classification/settings";

        private static readonly ClassificationLabel CONFIDENTIAL_LABEL_REQUIRED = new ClassificationLabel
        {
            Id = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            Name = "Confidential",
            Color = "#ffe0e3",
            SensitivityOrder = 1,
            IsDefault = false
        };

        private static readonly DataClassificationSettings EXPECTED_ALL = new DataClassificationSettings
        {
            OrgId = TEST_ORG_ID,
            PlanId = TEST_PLAN_ID,
            IsDisabled = false,
            GuidelinesUrl = "https://wiki.example.com/classification-guide",
            AllowManualChange = true,
            Labels = new List<ClassificationLabel>
            {
                new ClassificationLabel
                {
                    Id = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                    Name = "Confidential",
                    Description = "Highly sensitive information",
                    Color = "#ffe0e3",
                    SensitivityOrder = 1,
                    IsDefault = false
                },
                new ClassificationLabel
                {
                    Id = "4aa85f64-5717-4562-b3fc-2c963f66afa7",
                    Name = "Internal",
                    Description = "For internal use only",
                    Color = "#b9f4c3",
                    SensitivityOrder = 2,
                    IsDefault = true
                }
            },
            DowngradeApprovalSettings = new DowngradeApprovalSettings
            {
                Mode = DowngradeApprovalMode.CUSTOM,
                LabelApprovers = new List<LabelApproverEntry>
                {
                    new LabelApproverEntry
                    {
                        LabelId = "4aa85f64-5717-4562-b3fc-2c963f66afa7",
                        Approvers = new List<ApproverEntry>
                        {
                            new ApproverEntry { Type = ApproverType.USERS, Ids = new List<long> { 5448085317937028L } },
                            new ApproverEntry { Type = ApproverType.WORKSPACE_ADMINS, Ids = new List<long>() }
                        }
                    }
                }
            }
        };

        private static readonly DataClassificationSettings EXPECTED_REQUIRED = new DataClassificationSettings
        {
            OrgId = TEST_ORG_ID,
            PlanId = TEST_PLAN_ID,
            IsDisabled = false,
            Labels = new List<ClassificationLabel> { CONFIDENTIAL_LABEL_REQUIRED },
            DowngradeApprovalSettings = new DowngradeApprovalSettings { Mode = DowngradeApprovalMode.NONE }
        };

        private static readonly DataClassificationSettings EXPECTED_DISABLED_PLAN = new DataClassificationSettings
        {
            OrgId = TEST_ORG_ID,
            PlanId = TEST_PLAN_ID,
            IsDisabled = true,
            Labels = new List<ClassificationLabel>(),
            DowngradeApprovalSettings = new DowngradeApprovalSettings { Mode = DowngradeApprovalMode.NONE }
        };

        private static readonly DataClassificationSettings EXPECTED_APPROVAL_NEEDED = new DataClassificationSettings
        {
            OrgId = TEST_ORG_ID,
            PlanId = TEST_PLAN_ID,
            IsDisabled = false,
            AllowManualChange = true,
            Labels = new List<ClassificationLabel> { CONFIDENTIAL_LABEL_REQUIRED },
            DowngradeApprovalSettings = new DowngradeApprovalSettings
            {
                Mode = DowngradeApprovalMode.APPROVAL_NEEDED,
                Approvers = new List<ApproverEntry>
                {
                    new ApproverEntry { Type = ApproverType.GROUPS, Ids = new List<long> { 5129226945881988L, 2877427132196740L } },
                    new ApproverEntry { Type = ApproverType.USERS, Ids = new List<long> { 5448085317937028L } }
                }
            }
        };

        [TestMethod]
        public async Task TestGetDataClassificationSettingsGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/governance/get-data-classification-settings/all-response-body-properties", requestId.ToString());

            smartsheet.GovernanceResources.GetDataClassificationSettings(TEST_PLAN_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual(TEST_PATH, uri.AbsolutePath);
            Assert.AreEqual("GET", foundRequest.Method);

            var queryParams = HttpUtility.ParseQueryString(uri.Query);
            CollectionAssert.AreEquivalent(
                new Dictionary<string, string> { { "planId", TEST_PLAN_ID.ToString() } },
                queryParams.AllKeys.ToDictionary(k => k!, k => queryParams[k])
            );
        }

        [TestMethod]
        public async Task TestGetDataClassificationSettingsAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/governance/get-data-classification-settings/all-response-body-properties", requestId.ToString());

            DataClassificationSettings result = smartsheet.GovernanceResources.GetDataClassificationSettings(TEST_PLAN_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsTrue(string.IsNullOrEmpty(foundRequest.Body));
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ALL), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetDataClassificationSettingsRequiredResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/governance/get-data-classification-settings/required-response-body-properties", requestId.ToString());

            DataClassificationSettings result = smartsheet.GovernanceResources.GetDataClassificationSettings(TEST_PLAN_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsTrue(string.IsNullOrEmpty(foundRequest.Body));
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_REQUIRED), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public void TestGetDataClassificationSettingsError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            HelperFunctions.AssertRaisesException<SmartsheetException>(
                () => smartsheet.GovernanceResources.GetDataClassificationSettings(TEST_PLAN_ID),
                "Malformed Request");
        }

        [TestMethod]
        public void TestGetDataClassificationSettingsError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            HelperFunctions.AssertRaisesException<SmartsheetException>(
                () => smartsheet.GovernanceResources.GetDataClassificationSettings(TEST_PLAN_ID),
                "Internal Server Error");
        }

        // ── Endpoint-specific tests ──────────────────────────────────────────

        [TestMethod]
        public void TestGetDataClassificationSettingsDisabledPlan()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/governance/get-data-classification-settings/disabled-plan", requestId.ToString());

            DataClassificationSettings result = smartsheet.GovernanceResources.GetDataClassificationSettings(TEST_PLAN_ID);

            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_DISABLED_PLAN), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public void TestGetDataClassificationSettingsDowngradeApprovalModeApprovalNeeded()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/governance/get-data-classification-settings/downgrade-approval-mode-approval-needed", requestId.ToString());

            DataClassificationSettings result = smartsheet.GovernanceResources.GetDataClassificationSettings(TEST_PLAN_ID);

            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_APPROVAL_NEEDED), JsonConvert.SerializeObject(result));
        }

        [DataTestMethod]
        [DataRow(AssetType.SHEET, "sheet")]
        [DataRow(AssetType.REPORT, "report")]
        [DataRow(AssetType.SIGHT, "sight")]
        public async Task TestGetDataClassificationSettingsByAssetGeneratedUrlIsCorrect(AssetType assetType, string expectedAssetType)
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/governance/get-data-classification-settings/all-response-body-properties", requestId.ToString());

            smartsheet.GovernanceResources.GetDataClassificationSettings(assetType, TEST_ASSET_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual(TEST_PATH, uri.AbsolutePath);
            Assert.AreEqual("GET", foundRequest.Method);

            var queryParams = HttpUtility.ParseQueryString(uri.Query);
            CollectionAssert.AreEquivalent(
                new Dictionary<string, string>
                {
                    { "assetType", expectedAssetType },
                    { "assetId", TEST_ASSET_ID.ToString() }
                },
                queryParams.AllKeys.ToDictionary(k => k!, k => queryParams[k])
            );
        }

        [TestMethod]
        public void TestGetDataClassificationSettingsByAssetAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/governance/get-data-classification-settings/all-response-body-properties", requestId.ToString());

            DataClassificationSettings result = smartsheet.GovernanceResources.GetDataClassificationSettings(AssetType.SHEET, TEST_ASSET_ID);

            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ALL), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public void TestGetDataClassificationSettingsError404Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/404-response", requestId.ToString());

            HelperFunctions.AssertRaisesException<ResourceNotFoundException>(
                () => smartsheet.GovernanceResources.GetDataClassificationSettings(TEST_PLAN_ID),
                "Not Found");
        }
    }
}
