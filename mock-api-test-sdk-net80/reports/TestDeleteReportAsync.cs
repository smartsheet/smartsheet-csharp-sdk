using Smartsheet.Api;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestDeleteReportAsync
    {
        [TestMethod]
        public async Task TestDeleteReportAsyncGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/reports/delete-report/all-response-body-properties", requestId.ToString());

            await ss.ReportResources.DeleteReportAsync(CommonTestConstants.TEST_REPORT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.IsNotNull(foundRequest);
            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual(string.Format("/2.0/reports/{0}", CommonTestConstants.TEST_REPORT_ID), uri.AbsolutePath);
            Assert.AreEqual("DELETE", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestDeleteReportAsyncAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/reports/delete-report/all-response-body-properties", requestId.ToString());

            await ss.ReportResources.DeleteReportAsync(CommonTestConstants.TEST_REPORT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.AreEqual(string.Empty, foundRequest.Body);
        }

        [TestMethod]
        public async Task TestDeleteReportAsyncError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            SmartsheetException exception = await Assert.ThrowsExceptionAsync<SmartsheetException>(() => ss.ReportResources.DeleteReportAsync(CommonTestConstants.TEST_REPORT_ID));
            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }

        [TestMethod]
        public async Task TestDeleteReportAsyncError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = await Assert.ThrowsExceptionAsync<SmartsheetException>(() => ss.ReportResources.DeleteReportAsync(CommonTestConstants.TEST_REPORT_ID));
            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }
    }
}
