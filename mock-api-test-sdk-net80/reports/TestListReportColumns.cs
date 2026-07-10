using Newtonsoft.Json;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestListReportColumns
    {
        private static readonly List<ReportColumn> EXPECTED_REQUIRED_RESPONSE = new List<ReportColumn>
        {
            new ReportColumn { Index = 0, Title = "Task Name", Type = ColumnType.TEXT_NUMBER, Primary = true },
            new ReportColumn { Index = 1, Type = ColumnType.DATETIME, SystemColumnType = SystemColumnType.CREATED_DATE }
        };

        private static readonly List<ReportColumn> EXPECTED_ALL_RESPONSE = new List<ReportColumn>
        {
            new ReportColumn
            {
                VirtualId = 7001L, Index = 0, Title = "Task Name", Type = ColumnType.TEXT_NUMBER,
                Primary = true, Width = 150, Hidden = false, Validation = true, Version = 0,
                AutoNumberFormat = new AutoNumberFormat { Fill = "0001", Prefix = "TASK-", StartingNumber = 1, Suffix = "" }
            },
            new ReportColumn
            {
                VirtualId = 7002L, Index = 1, Title = "Status", Type = ColumnType.PICKLIST,
                Width = 120, Hidden = false, Validation = false, Version = 0
            },
            new ReportColumn
            {
                VirtualId = 7003L, Index = 2, Title = "Created By", Type = ColumnType.CONTACT_LIST,
                SystemColumnType = SystemColumnType.CREATED_BY, Width = 150, Hidden = false, Validation = false, Version = 1
            },
            new ReportColumn
            {
                VirtualId = 7004L, Index = 3, Title = "Sheet Name", Type = ColumnType.TEXT_NUMBER,
                SheetNameColumn = true, Width = 200, Hidden = false, Validation = false, Version = 0
            }
        };

        [TestMethod]
        public async Task TestListReportColumnsGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/list-report-columns/all-response-body-properties", requestId.ToString());

            smartsheet.ReportResources.ListReportColumns(CommonTestConstants.TEST_REPORT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual($"/2.0/reports/{CommonTestConstants.TEST_REPORT_ID}/columns", uri.AbsolutePath);
            Assert.AreEqual("GET", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestListReportColumnsWithLevelGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/list-report-columns/all-response-body-properties", requestId.ToString());

            smartsheet.ReportResources.ListReportColumns(CommonTestConstants.TEST_REPORT_ID, null, 3);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);
            var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual($"/2.0/reports/{CommonTestConstants.TEST_REPORT_ID}/columns", uri.AbsolutePath);
            Assert.AreEqual("GET", foundRequest.Method);

            Assert.AreEqual("3", queryParams["level"]);
        }

        [TestMethod]
        public async Task TestListReportColumnsWithPaginationParams()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/list-report-columns/all-response-body-properties", requestId.ToString());

            smartsheet.ReportResources.ListReportColumns(CommonTestConstants.TEST_REPORT_ID, new TokenPaginationParameters("tok1", 5));

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);
            var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual("tok1", queryParams["lastKey"]);
            Assert.AreEqual("5", queryParams["maxItems"]);
        }

        [TestMethod]
        public void TestListReportColumnsRequiredResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/list-report-columns/required-response-body-properties", requestId.ToString());

            TokenPaginatedResult<ReportColumn> result = smartsheet.ReportResources.ListReportColumns(CommonTestConstants.TEST_REPORT_ID);

            Assert.IsNotNull(result);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_REQUIRED_RESPONSE), JsonConvert.SerializeObject(result.Data));
        }

        [TestMethod]
        public void TestListReportColumnsAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/list-report-columns/all-response-body-properties", requestId.ToString());

            TokenPaginatedResult<ReportColumn> result = smartsheet.ReportResources.ListReportColumns(CommonTestConstants.TEST_REPORT_ID);

            Assert.IsNotNull(result);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ALL_RESPONSE), JsonConvert.SerializeObject(result.Data));
        }

        [TestMethod]
        public void TestListReportColumnsError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.ListReportColumns(CommonTestConstants.TEST_REPORT_ID)
            );

            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestListReportColumnsError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.ListReportColumns(CommonTestConstants.TEST_REPORT_ID)
            );

            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }

        [TestMethod]
        public async Task TestListReportColumnsAsyncGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/list-report-columns/all-response-body-properties", requestId.ToString());

            await smartsheet.ReportResources.ListReportColumnsAsync(CommonTestConstants.TEST_REPORT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual($"/2.0/reports/{CommonTestConstants.TEST_REPORT_ID}/columns", uri.AbsolutePath);
            Assert.AreEqual("GET", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestListReportColumnsAsyncAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/list-report-columns/all-response-body-properties", requestId.ToString());

            TokenPaginatedResult<ReportColumn> result = await smartsheet.ReportResources.ListReportColumnsAsync(CommonTestConstants.TEST_REPORT_ID);

            Assert.IsNotNull(result);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ALL_RESPONSE), JsonConvert.SerializeObject(result.Data));
        }

        [TestMethod]
        public async Task TestListReportColumnsAsyncError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.ReportResources.ListReportColumnsAsync(CommonTestConstants.TEST_REPORT_ID),
                "Internal Server Error"
            );
        }

        [TestMethod]
        public async Task TestListReportColumnsAsyncError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.ReportResources.ListReportColumnsAsync(CommonTestConstants.TEST_REPORT_ID),
                "Malformed Request"
            );
        }
    }
}
