using Newtonsoft.Json;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestAddReportScope
    {
        private static readonly List<ReportScopeInclusion> SCOPES_TO_ADD = new List<ReportScopeInclusion>
        {
            new ReportScopeInclusion
            {
                AssetType = ReportAssetType.SHEET,
                AssetId = CommonTestConstants.TEST_SHEET_ID
            }
        };

        private static readonly string EXPECTED_REQUEST_BODY = JsonConvert.SerializeObject(new List<Dictionary<string, object>>
        {
            new Dictionary<string, object>
            {
                { "assetType", "sheet" },
                { "assetId", CommonTestConstants.TEST_SHEET_ID }
            }
        });

        [TestMethod]
        public void TestAddReportScopeEmptyScopesError()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/add-report-scope/all-response-body-properties", requestId.ToString());

            Assert.ThrowsException<ArgumentException>(
                () => smartsheet.ReportResources.AddReportScope(CommonTestConstants.TEST_REPORT_ID, new List<ReportScopeInclusion>())
            );
        }

        [TestMethod]
        public async Task TestAddReportScopeGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/add-report-scope/all-response-body-properties", requestId.ToString());

            smartsheet.ReportResources.AddReportScope(CommonTestConstants.TEST_REPORT_ID, SCOPES_TO_ADD);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual($"/2.0/reports/{CommonTestConstants.TEST_REPORT_ID}/scope", path);
            Assert.AreEqual("POST", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestAddReportScopeAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/add-report-scope/all-response-body-properties", requestId.ToString());

            smartsheet.ReportResources.AddReportScope(CommonTestConstants.TEST_REPORT_ID, SCOPES_TO_ADD);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.AreEqual(EXPECTED_REQUEST_BODY, foundRequest.Body);
        }

        [TestMethod]
        public void TestAddReportScopeError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.AddReportScope(CommonTestConstants.TEST_REPORT_ID, SCOPES_TO_ADD)
            );

            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestAddReportScopeError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.AddReportScope(CommonTestConstants.TEST_REPORT_ID, SCOPES_TO_ADD)
            );

            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }
    }
}
