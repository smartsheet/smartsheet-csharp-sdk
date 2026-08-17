using System.Web;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class GovernanceResourcesTest
    {
        private const long TEST_PLAN_ID = 41878788L;
        private const long TEST_ORG_ID = 1244212L;
        private const string TEST_GUIDELINES_URL = "https://wiki.example.com/classification-guide";
        private const string TEST_LABEL_ID = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
        private const string TEST_LABEL_ID_2 = "4aa85f64-5717-4562-b3fc-2c963f66afa7";

        // ── URL construction ──────────────────────────────────────────────────

        [TestMethod]
        public async Task TestGetDataClassificationSettingsGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient(
                "/governance/get-data-classification-settings/all-response-body-properties",
                requestId.ToString());

            smartsheet.GovernanceResources.GetDataClassificationSettings(TEST_PLAN_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest);
            var uri = new Uri(foundRequest.AbsoluteUrl);
            Assert.AreEqual("/2.0/governance/data-classification/settings", uri.AbsolutePath);
            Assert.AreEqual(TEST_PLAN_ID.ToString(), HttpUtility.ParseQueryString(uri.Query)["planId"]);
        }

        // ── CUSTOM mode — all optional fields ────────────────────────────────

        [TestMethod]
        public void TestGetDataClassificationSettingsCustomMode()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient(
                "/governance/get-data-classification-settings/all-response-body-properties",
                requestId.ToString());

            DataClassificationSettings response =
                smartsheet.GovernanceResources.GetDataClassificationSettings(TEST_PLAN_ID);

            Assert.IsNotNull(response);
            Assert.AreEqual(TEST_ORG_ID, response.OrgId);
            Assert.AreEqual(TEST_PLAN_ID, response.PlanId);
            Assert.AreEqual(false, response.IsDisabled);
            Assert.AreEqual(TEST_GUIDELINES_URL, response.GuidelinesUrl);
            Assert.AreEqual(true, response.AllowManualChange);
            Assert.AreEqual(2, response.Labels!.Count);
            Assert.AreEqual(TEST_LABEL_ID, response.Labels[0].Id);
            Assert.AreEqual("Confidential", response.Labels[0].Name);
            Assert.AreEqual("Highly sensitive information", response.Labels[0].Description);
            Assert.AreEqual("#FFE0E3", response.Labels[0].Color);
            Assert.AreEqual(1, response.Labels[0].SensitivityOrder);
            Assert.AreEqual(false, response.Labels[0].IsDefault);

            // CUSTOM mode: only labelApprovers — no top-level approvers.
            Assert.AreEqual("CUSTOM", response.DowngradeApprovalSettings!.Mode);
            Assert.IsNull(response.DowngradeApprovalSettings.Approvers);
            Assert.IsNotNull(response.DowngradeApprovalSettings.LabelApprovers);
            Assert.AreEqual(1, response.DowngradeApprovalSettings.LabelApprovers!.Count);
            Assert.AreEqual(TEST_LABEL_ID_2, response.DowngradeApprovalSettings.LabelApprovers[0].LabelId);
            var labelApprovers = response.DowngradeApprovalSettings.LabelApprovers[0].Approvers!;
            Assert.AreEqual(2, labelApprovers.Count);
            Assert.AreEqual("USERS", labelApprovers[0].Type);
            Assert.AreEqual(1, labelApprovers[0].Ids!.Count);
            Assert.AreEqual("WORKSPACE_ADMINS", labelApprovers[1].Type);
            Assert.AreEqual(0, labelApprovers[1].Ids!.Count);
        }

        // ── APPROVAL_NEEDED mode ──────────────────────────────────────────────

        [TestMethod]
        public void TestGetDataClassificationSettingsApprovalNeededMode()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient(
                "/governance/get-data-classification-settings/downgrade-approval-mode-approval-needed",
                requestId.ToString());

            DataClassificationSettings response =
                smartsheet.GovernanceResources.GetDataClassificationSettings(TEST_PLAN_ID);

            Assert.AreEqual("APPROVAL_NEEDED", response.DowngradeApprovalSettings!.Mode);
            // APPROVAL_NEEDED: top-level approvers, no labelApprovers.
            Assert.IsNotNull(response.DowngradeApprovalSettings.Approvers);
            Assert.AreEqual(2, response.DowngradeApprovalSettings.Approvers!.Count);
            Assert.AreEqual("GROUPS", response.DowngradeApprovalSettings.Approvers[0].Type);
            Assert.AreEqual(2, response.DowngradeApprovalSettings.Approvers[0].Ids!.Count);
            Assert.AreEqual("USERS", response.DowngradeApprovalSettings.Approvers[1].Type);
            Assert.AreEqual(1, response.DowngradeApprovalSettings.Approvers[1].Ids!.Count);
            Assert.IsNull(response.DowngradeApprovalSettings.LabelApprovers);
        }

        // ── NONE mode (required-only fields) ──────────────────────────────────

        [TestMethod]
        public void TestGetDataClassificationSettingsRequiredFieldsNoneMode()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient(
                "/governance/get-data-classification-settings/required-response-body-properties",
                requestId.ToString());

            DataClassificationSettings response =
                smartsheet.GovernanceResources.GetDataClassificationSettings(TEST_PLAN_ID);

            Assert.AreEqual(TEST_ORG_ID, response.OrgId);
            Assert.AreEqual(TEST_PLAN_ID, response.PlanId);
            Assert.AreEqual(false, response.IsDisabled);
            Assert.IsNull(response.GuidelinesUrl);
            Assert.IsNull(response.AllowManualChange);
            Assert.AreEqual(1, response.Labels!.Count);
            Assert.IsNull(response.Labels[0].Description);
            Assert.AreEqual("NONE", response.DowngradeApprovalSettings!.Mode);
            Assert.IsNull(response.DowngradeApprovalSettings.Approvers);
            Assert.IsNull(response.DowngradeApprovalSettings.LabelApprovers);
        }

        // ── Disabled plan ──────────────────────────────────────────────────────

        [TestMethod]
        public void TestGetDataClassificationSettingsDisabledPlan()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient(
                "/governance/get-data-classification-settings/disabled-plan",
                requestId.ToString());

            DataClassificationSettings response =
                smartsheet.GovernanceResources.GetDataClassificationSettings(TEST_PLAN_ID);

            Assert.AreEqual(true, response.IsDisabled);
            Assert.AreEqual(0, response.Labels!.Count);
            Assert.IsNull(response.GuidelinesUrl);
            Assert.IsNull(response.AllowManualChange);
            Assert.AreEqual("NONE", response.DowngradeApprovalSettings!.Mode);
            Assert.IsNull(response.DowngradeApprovalSettings.Approvers);
            Assert.IsNull(response.DowngradeApprovalSettings.LabelApprovers);
        }

        // ── Error responses ────────────────────────────────────────────────────

        [TestMethod]
        public void TestGetDataClassificationSettingsError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient(
                "/errors/400-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(
                () => smartsheet.GovernanceResources.GetDataClassificationSettings(TEST_PLAN_ID));
            Assert.AreEqual("Malformed Request", exception.Message);
        }

        [TestMethod]
        public void TestGetDataClassificationSettingsError403Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient(
                "/errors/403-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(
                () => smartsheet.GovernanceResources.GetDataClassificationSettings(TEST_PLAN_ID));
            Assert.AreEqual("You are not authorized to perform this action.", exception.Message);
        }

        [TestMethod]
        public void TestGetDataClassificationSettingsError404Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient(
                "/errors/404-response", requestId.ToString());

            ResourceNotFoundException exception = Assert.ThrowsException<ResourceNotFoundException>(
                () => smartsheet.GovernanceResources.GetDataClassificationSettings(TEST_PLAN_ID));
            Assert.AreEqual("Not Found", exception.Message);
        }

        [TestMethod]
        public void TestGetDataClassificationSettingsError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient(
                "/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(
                () => smartsheet.GovernanceResources.GetDataClassificationSettings(TEST_PLAN_ID));
            Assert.AreEqual("Internal Server Error", exception.Message);
        }
    }
}
