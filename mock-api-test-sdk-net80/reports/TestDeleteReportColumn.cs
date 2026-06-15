using Smartsheet.Api;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestDeleteReportColumn
    {
        [TestMethod]
        public async Task TestDeleteReportColumnGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/delete-report-column/all-response-body-properties", requestId.ToString());

            smartsheet.ReportResources.DeleteReportColumn(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.IsNotNull(foundRequest);
            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual(string.Format("/2.0/reports/{0}/columns/{1}", CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID), uri.AbsolutePath);
            Assert.AreEqual("DELETE", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public void TestDeleteReportColumnError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.DeleteReportColumn(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID)
            );

            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestDeleteReportColumnError404Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/404-response", requestId.ToString());

            HelperFunctions.AssertRaisesException<SmartsheetException>(
                () => smartsheet.ReportResources.DeleteReportColumn(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID),
                "Not Found"
            );
        }

        [TestMethod]
        public async Task TestDeleteReportColumnAsyncGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/delete-report-column/all-response-body-properties", requestId.ToString());

            await smartsheet.ReportResources.DeleteReportColumnAsync(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.IsNotNull(foundRequest);
            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual(string.Format("/2.0/reports/{0}/columns/{1}", CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID), uri.AbsolutePath);
            Assert.AreEqual("DELETE", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestDeleteReportColumnAsyncError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.ReportResources.DeleteReportColumnAsync(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID),
                "Internal Server Error"
            );
        }

        [TestMethod]
        public async Task TestDeleteReportColumnAsyncError404Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/404-response", requestId.ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.ReportResources.DeleteReportColumnAsync(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID),
                "Not Found"
            );
        }
    }
}
