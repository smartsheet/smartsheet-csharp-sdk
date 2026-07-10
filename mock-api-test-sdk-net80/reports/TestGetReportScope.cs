using System.Web;
using Newtonsoft.Json;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestGetReportScope
    {
        private static readonly List<ReportScopeInclusion> EXPECTED_REQUIRED_RESPONSE = new List<ReportScopeInclusion>
        {
            new ReportScopeInclusion { AssetType = ReportAssetType.SHEET, AssetId = 2331373580117892 }
        };

        private static readonly List<ReportScopeInclusion> EXPECTED_ALL_RESPONSE = new List<ReportScopeInclusion>
        {
            new ReportScopeInclusion { AssetType = ReportAssetType.SHEET, AssetId = 2331373580117892 },
            new ReportScopeInclusion { AssetType = ReportAssetType.WORKSPACE, AssetId = 7879278542455688 },
            new ReportScopeInclusion { AssetType = ReportAssetType.SHEET, AssetId = 1234567890123456 }
        };

        [TestMethod]
        public async Task TestGetReportScopeGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/list-report-scope/all-response-body-properties", requestId.ToString());

            smartsheet.ReportResources.GetReportScope(CommonTestConstants.TEST_REPORT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual($"/2.0/reports/{CommonTestConstants.TEST_REPORT_ID}/scope", uri.AbsolutePath);
            Assert.AreEqual("GET", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestGetReportScopeWithPaginationParams()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/list-report-scope/all-response-body-properties", requestId.ToString());

            smartsheet.ReportResources.GetReportScope(CommonTestConstants.TEST_REPORT_ID, new TokenPaginationParameters("abc123", 10));

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);
            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual("abc123", queryParams["lastKey"]);
            Assert.AreEqual("10", queryParams["maxItems"]);
        }

        [TestMethod]
        public void TestGetReportScopeRequiredResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/list-report-scope/required-response-body-properties", requestId.ToString());

            TokenPaginatedResult<ReportScopeInclusion> result = smartsheet.ReportResources.GetReportScope(CommonTestConstants.TEST_REPORT_ID);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Data.Count);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_REQUIRED_RESPONSE), JsonConvert.SerializeObject(result.Data));
        }

        [TestMethod]
        public void TestGetReportScopeAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/list-report-scope/all-response-body-properties", requestId.ToString());

            TokenPaginatedResult<ReportScopeInclusion> result = smartsheet.ReportResources.GetReportScope(CommonTestConstants.TEST_REPORT_ID);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Data.Count);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ALL_RESPONSE), JsonConvert.SerializeObject(result.Data));
        }

        [TestMethod]
        public void TestGetReportScopeError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.GetReportScope(CommonTestConstants.TEST_REPORT_ID)
            );

            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestGetReportScopeError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.GetReportScope(CommonTestConstants.TEST_REPORT_ID)
            );

            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }

        [TestMethod]
        public async Task TestGetReportScopeAsyncGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/list-report-scope/all-response-body-properties", requestId.ToString());

            await smartsheet.ReportResources.GetReportScopeAsync(CommonTestConstants.TEST_REPORT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual($"/2.0/reports/{CommonTestConstants.TEST_REPORT_ID}/scope", uri.AbsolutePath);
            Assert.AreEqual("GET", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestGetReportScopeAsyncAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/list-report-scope/all-response-body-properties", requestId.ToString());

            TokenPaginatedResult<ReportScopeInclusion> result = await smartsheet.ReportResources.GetReportScopeAsync(CommonTestConstants.TEST_REPORT_ID);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Data.Count);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ALL_RESPONSE), JsonConvert.SerializeObject(result.Data));
        }

        [TestMethod]
        public async Task TestGetReportScopeAsyncError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.ReportResources.GetReportScopeAsync(CommonTestConstants.TEST_REPORT_ID),
                "Internal Server Error");
        }

        [TestMethod]
        public async Task TestGetReportScopeAsyncError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.ReportResources.GetReportScopeAsync(CommonTestConstants.TEST_REPORT_ID),
                "Malformed Request");
        }
    }
}
