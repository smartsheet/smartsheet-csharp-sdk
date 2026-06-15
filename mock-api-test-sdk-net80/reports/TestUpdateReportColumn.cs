using Newtonsoft.Json;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestUpdateReportColumn
    {
        private static readonly UpdateReportColumnRequest REQUEST_REQUIRED = new UpdateReportColumnRequest
        {
            Title = "Updated Column",
            Index = 1
        };

        private static readonly UpdateReportColumnRequest REQUEST_ALL = new UpdateReportColumnRequest
        {
            Title = "Updated Task Name",
            Index = 2,
            Hidden = false,
            Width = 200
        };

        private static readonly string EXPECTED_REQUIRED_REQUEST_BODY = JsonConvert.SerializeObject(
            new Dictionary<string, object> { { "title", "Updated Column" }, { "index", 1 } }
        );

        private static readonly string EXPECTED_ALL_REQUEST_BODY = JsonConvert.SerializeObject(
            new Dictionary<string, object> { { "title", "Updated Task Name" }, { "index", 2 }, { "hidden", false }, { "width", 200 } }
        );

        private static readonly ReportColumn EXPECTED_REQUIRED_RESPONSE = new ReportColumn
        {
            Index = 1,
            Title = "Updated Column",
            Type = ColumnType.TEXT_NUMBER,
            Primary = true
        };

        private static readonly ReportColumn EXPECTED_ALL_RESPONSE = new ReportColumn
        {
            VirtualId = 7001,
            Index = 2,
            Title = "Updated Task Name",
            Type = ColumnType.TEXT_NUMBER,
            Primary = true,
            Width = 200,
            Hidden = false,
            Validation = true,
            Version = 0,
            AutoNumberFormat = new AutoNumberFormat { Fill = "0001", Prefix = "TASK-", StartingNumber = 1, Suffix = "" }
        };

        [TestMethod]
        public async Task TestUpdateReportColumnGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/update-report-column/required-response-body-properties", requestId.ToString());

            smartsheet.ReportResources.UpdateReportColumn(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID, REQUEST_REQUIRED);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual($"/2.0/reports/{CommonTestConstants.TEST_REPORT_ID}/columns/{CommonTestConstants.TEST_COLUMN_VIRTUAL_ID}", uri.AbsolutePath);
            Assert.AreEqual("PUT", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestUpdateReportColumnRequiredResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/update-report-column/required-response-body-properties", requestId.ToString());

            ReportColumn result = smartsheet.ReportResources.UpdateReportColumn(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID, REQUEST_REQUIRED);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.AreEqual(EXPECTED_REQUIRED_REQUEST_BODY, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_REQUIRED_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestUpdateReportColumnAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/update-report-column/all-response-body-properties", requestId.ToString());

            ReportColumn result = smartsheet.ReportResources.UpdateReportColumn(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID, REQUEST_ALL);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.AreEqual(EXPECTED_ALL_REQUEST_BODY, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ALL_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public void TestUpdateReportColumnError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.UpdateReportColumn(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID, REQUEST_REQUIRED)
            );

            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestUpdateReportColumnError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.UpdateReportColumn(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID, REQUEST_REQUIRED)
            );

            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }

        [TestMethod]
        public async Task TestUpdateReportColumnAsyncGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/update-report-column/required-response-body-properties", requestId.ToString());

            await smartsheet.ReportResources.UpdateReportColumnAsync(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID, REQUEST_REQUIRED);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual($"/2.0/reports/{CommonTestConstants.TEST_REPORT_ID}/columns/{CommonTestConstants.TEST_COLUMN_VIRTUAL_ID}", uri.AbsolutePath);
            Assert.AreEqual("PUT", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestUpdateReportColumnAsyncAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/update-report-column/all-response-body-properties", requestId.ToString());

            ReportColumn result = await smartsheet.ReportResources.UpdateReportColumnAsync(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID, REQUEST_ALL);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.AreEqual(EXPECTED_ALL_REQUEST_BODY, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ALL_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestUpdateReportColumnAsyncError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.ReportResources.UpdateReportColumnAsync(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID, REQUEST_REQUIRED),
                "Internal Server Error"
            );
        }

        [TestMethod]
        public async Task TestUpdateReportColumnAsyncError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.ReportResources.UpdateReportColumnAsync(CommonTestConstants.TEST_REPORT_ID, CommonTestConstants.TEST_COLUMN_VIRTUAL_ID, REQUEST_REQUIRED),
                "Malformed Request"
            );
        }
    }
}
