using Newtonsoft.Json;
using NLog;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestRemoveReportScope
    {
        [TestMethod]
        public void TestRemoveReportScopeEmptyScopesError()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/remove-report-scope/all-response-body-properties", requestId.ToString());

            Assert.ThrowsException<ArgumentException>(
                () => smartsheet.ReportResources.RemoveReportScope(123456789, new List<ReportScopeInclusion>())
            );
        }

        [TestMethod]
        public async Task TestRemoveReportScopeAllGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/remove-report-scope/all-response-body-properties", requestId.ToString());

            var scopesToRemove = new List<ReportScopeInclusion>
            {
                new ReportScopeInclusion
                {
                    AssetType = ReportAssetType.SHEET,
                    AssetId = 987654321
                }
            };

            smartsheet.ReportResources.RemoveReportScope(123456789, scopesToRemove);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            
            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual("/2.0/reports/123456789/scope", path);
            Assert.AreEqual("DELETE", foundRequest.Method);
        }

        [TestMethod]
        public async Task TestRemoveReportScopeAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/remove-report-scope/all-response-body-properties", requestId.ToString());

            var scopesToRemove = new List<ReportScopeInclusion>
            {
                new ReportScopeInclusion
                {
                    AssetType = ReportAssetType.SHEET,
                    AssetId = 987654321
                }
            };

            smartsheet.ReportResources.RemoveReportScope(123456789, scopesToRemove);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            
            var expectedBody = JsonConvert.SerializeObject(new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "assetType", "SHEET" },
                    { "assetId", 987654321 }
                }
            });
            
            Assert.AreEqual(expectedBody, foundRequest.Body);
        }

        [TestMethod]
        public void TestRemoveReportScopeError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            var scopesToRemove = new List<ReportScopeInclusion>
            {
                new ReportScopeInclusion
                {
                    AssetType = ReportAssetType.SHEET,
                    AssetId = 987654321
                }
            };

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.RemoveReportScope(123456789, scopesToRemove)
            );
            
            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestRemoveReportScopeError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            var scopesToRemove = new List<ReportScopeInclusion>
            {
                new ReportScopeInclusion
                {
                    AssetType = ReportAssetType.SHEET,
                    AssetId = 987654321
                }
            };

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.RemoveReportScope(123456789, scopesToRemove)
            );
            
            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }
    }
}