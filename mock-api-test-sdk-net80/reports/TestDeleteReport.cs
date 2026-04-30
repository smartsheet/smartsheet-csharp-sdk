using Smartsheet.Api;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestDeleteReport
    {
        private const long TEST_REPORT_ID = 314159265359;

        [TestMethod]
        public async void TestDeleteReportGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/reports/delete-report/all-response-body-properties", requestId.ToString());

            ss.ReportResources.DeleteReport(TEST_REPORT_ID);

            WiremockHelper wiremockHelper = new();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.IsNotNull(foundRequest);
            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual(string.Format("/2.0/reports/{0}", TEST_REPORT_ID), uri.AbsolutePath);
        }

        [TestMethod]
        public void TestDeleteReportResponse()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/reports/delete-report/all-response-body-properties", requestId.ToString());

            ss.ReportResources.DeleteReport(TEST_REPORT_ID);
        }

        [TestMethod]
        public void TestDeleteReportError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.ReportResources.DeleteReport(TEST_REPORT_ID));
            Assert.AreEqual("Malformed Request", exception.Message);
        }

        [TestMethod]
        public void TestDeleteReportError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.ReportResources.DeleteReport(TEST_REPORT_ID));
            Assert.AreEqual("Internal Server Error", exception.Message);
        }
    }
}
