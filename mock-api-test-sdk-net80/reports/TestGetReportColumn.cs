using Newtonsoft.Json;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestGetReportColumn
    {
        private static readonly ReportColumn EXPECTED_REQUIRED_RESPONSE = new ReportColumn
        {
            Index = 0,
            Title = "Task Name",
            Type = ColumnType.TEXT_NUMBER,
            Primary = true
        };

        private static readonly ReportColumn EXPECTED_ALL_RESPONSE = new ReportColumn
        {
            VirtualId = 7001,
            Index = 0,
            Title = "Task Name",
            Type = ColumnType.TEXT_NUMBER,
            Primary = true,
            Width = 150,
            Hidden = false,
            Validation = true,
            Version = 0,
            AutoNumberFormat = new AutoNumberFormat { Fill = "0001", Prefix = "TASK-", StartingNumber = 1, Suffix = "" }
        };

        [TestMethod]
        public async Task TestGetReportColumnGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/get-report-column/all-response-body-properties", requestId.ToString());

            smartsheet.ReportResources.GetReportColumn(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual($"/2.0/reports/{CommonTestConstants.TEST_REPORT_ID}/columns/{CommonTestConstants.TEST_COLUMN_VIRTUAL_ID}", uri.AbsolutePath);
            Assert.AreEqual("GET", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestGetReportColumnRequiredResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/get-report-column/required-response-body-properties", requestId.ToString());

            ReportColumn result = smartsheet.ReportResources.GetReportColumn(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_REQUIRED_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetReportColumnAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/get-report-column/all-response-body-properties", requestId.ToString());

            ReportColumn result = smartsheet.ReportResources.GetReportColumn(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ALL_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public void TestGetReportColumnError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.GetReportColumn(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID)
            );

            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestGetReportColumnError404Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/404-response", requestId.ToString());

            HelperFunctions.AssertRaisesException<SmartsheetException>(
                () => smartsheet.ReportResources.GetReportColumn(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID),
                "Not Found"
            );
        }

        [TestMethod]
        public async Task TestGetReportColumnAsyncGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/get-report-column/all-response-body-properties", requestId.ToString());

            await smartsheet.ReportResources.GetReportColumnAsync(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual($"/2.0/reports/{CommonTestConstants.TEST_REPORT_ID}/columns/{CommonTestConstants.TEST_COLUMN_VIRTUAL_ID}", uri.AbsolutePath);
            Assert.AreEqual("GET", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestGetReportColumnAsyncAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/get-report-column/all-response-body-properties", requestId.ToString());

            ReportColumn result = await smartsheet.ReportResources.GetReportColumnAsync(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ALL_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetReportColumnAsyncError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.ReportResources.GetReportColumnAsync(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID),
                "Internal Server Error"
            );
        }

        [TestMethod]
        public async Task TestGetReportColumnAsyncError404Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/404-response", requestId.ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.ReportResources.GetReportColumnAsync(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID),
                "Not Found"
            );
        }
    }
}
