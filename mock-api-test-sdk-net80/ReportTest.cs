
using Smartsheet.Api;
using Smartsheet.Api.Models;
using Smartsheet.Api.Models.Inclusions;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class ReportTests
    {
        [TestMethod]
        public void AddReportScope_EmptyScopes_ThrowsArgumentException()
        {
            SmartsheetClient client = HelperFunctions.SetupClient("Add Report Scope - Empty Scopes");

            Assert.ThrowsException<ArgumentException>(
                () => client.ReportResources.AddReportScope(123456789, new List<ReportScopeInclusion>())
            );
        }

        [TestMethod]
        public async Task AddReportScope_SingleScopeAsync()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient client = HelperFunctions.SetupClient("/reports/add-report-scope/all-response-body-properties", requestId.ToString());

            var scopesToAdd = new List<ReportScopeInclusion>
            {
                new ReportScopeInclusion
                {
                    AssetType = AssetType.SHEET,
                    AssetId = 987654321
                }
            };

            RequestResult<Report> result = client.ReportResources.AddReportScope(123456789, scopesToAdd);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual("/2.0/reports/123456789/scope", path);
            Assert.AreEqual("POST", foundRequest.Method);

            Assert.IsNotNull(result);
            Assert.AreEqual("SUCCESS", result.Message);
            Assert.AreEqual(0, result.ResultCode);
        }

        [TestMethod]
        public void RemoveReportScope_EmptyScopes_ThrowsArgumentException()
        {
            SmartsheetClient client = HelperFunctions.SetupClient("Remove Report Scope - Empty Scopes");

            Assert.ThrowsException<ArgumentException>(
                () => client.ReportResources.RemoveReportScope(123456789, new List<ReportScopeInclusion>())
            );
        }

        [TestMethod]
        public async Task RemoveReportScope_SingleScopeAsync()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient client = HelperFunctions.SetupClient("/reports/remove-report-scope/all-response-body-properties", requestId.ToString());

            var scopesToAdd = new List<ReportScopeInclusion>
            {
                new ReportScopeInclusion
                {
                    AssetType = AssetType.SHEET,
                    AssetId = 987654321
                }
            };

            RequestResult<Report> result = client.ReportResources.RemoveReportScope(123456789, scopesToAdd);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual("/2.0/reports/123456789/scope", path);
            Assert.AreEqual("DELETE", foundRequest.Method);

            Assert.IsNotNull(result);
            Assert.AreEqual("SUCCESS", result.Message);
            Assert.AreEqual(0, result.ResultCode);
        }
    }
}
