using Newtonsoft.Json;
using NLog;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestAddReportScope
    {
        [TestMethod]
        public void TestAddReportScopeEmptyScopesError()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/add-report-scope/all-response-body-properties", requestId.ToString());

            Assert.ThrowsException<ArgumentException>(
                () => smartsheet.ReportResources.AddReportScope(123456789, new List<ReportScopeInclusion>())
            );
        }

        [TestMethod]
        public async Task TestAddReportScopeAllGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/add-report-scope/all-response-body-properties", requestId.ToString());

            var scopesToAdd = new List<ReportScopeInclusion>
            {
                new ReportScopeInclusion
                {
                    AssetType = ReportAssetType.SHEET,
                    AssetId = 987654321
                }
            };

            smartsheet.ReportResources.AddReportScope(123456789, scopesToAdd);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            
            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual("/2.0/reports/123456789/scope", path);
            Assert.AreEqual("POST", foundRequest.Method);
        }

        [TestMethod]
        public async Task TestAddReportScopeAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/add-report-scope/all-response-body-properties", requestId.ToString());

            var scopesToAdd = new List<ReportScopeInclusion>
            {
                new ReportScopeInclusion
                {
                    AssetType = ReportAssetType.SHEET,
                    AssetId = 987654321
                }
            };

            smartsheet.ReportResources.AddReportScope(123456789, scopesToAdd);

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
        public void TestAddReportScopeError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            var scopesToAdd = new List<ReportScopeInclusion>
            {
                new ReportScopeInclusion
                {
                    AssetType = ReportAssetType.SHEET,
                    AssetId = 987654321
                }
            };

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.AddReportScope(123456789, scopesToAdd)
            );
            
            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestAddReportScopeError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            var scopesToAdd = new List<ReportScopeInclusion>
            {
                new ReportScopeInclusion
                {
                    AssetType = ReportAssetType.SHEET,
                    AssetId = 987654321
                }
            };

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.AddReportScope(123456789, scopesToAdd)
            );
            
            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }
    }
}