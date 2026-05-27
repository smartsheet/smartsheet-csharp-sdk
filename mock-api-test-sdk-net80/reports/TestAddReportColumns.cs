using Newtonsoft.Json;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestAddReportColumns
    {
        private static readonly List<ReportColumn> COLUMNS_TO_ADD_REQUIRED = new List<ReportColumn>
        {
            new ReportColumn { Title = "Item selected", Type = ColumnType.CHECKBOX, Index = 4 },
            new ReportColumn { Title = "Sheet name", Type = ColumnType.TEXT_NUMBER, Index = 5 }
        };

        private static readonly List<ReportColumn> COLUMNS_TO_ADD_ALL = new List<ReportColumn>
        {
            new ReportColumn { Title = "Item selected", Type = ColumnType.CHECKBOX, Index = 4, Hidden = false, Width = 150 },
            new ReportColumn { Title = "Sheet name", Type = ColumnType.TEXT_NUMBER, Index = 5, Hidden = false, Width = 150, SheetNameColumn = true }
        };

        private static readonly string EXPECTED_REQUIRED_REQUEST_BODY = JsonConvert.SerializeObject(new List<Dictionary<string, object>>
        {
            new Dictionary<string, object> { { "type", "CHECKBOX" }, { "index", 4 }, { "title", "Item selected" } },
            new Dictionary<string, object> { { "type", "TEXT_NUMBER" }, { "index", 5 }, { "title", "Sheet name" } }
        });

        private static readonly string EXPECTED_ALL_REQUEST_BODY = JsonConvert.SerializeObject(new List<Dictionary<string, object>>
        {
            new Dictionary<string, object> { { "type", "CHECKBOX" }, { "hidden", false }, { "index", 4 }, { "title", "Item selected" }, { "width", 150 } },
            new Dictionary<string, object> { { "sheetNameColumn", true }, { "type", "TEXT_NUMBER" }, { "hidden", false }, { "index", 5 }, { "title", "Sheet name" }, { "width", 150 } }
        });

        private static readonly List<ReportColumn> EXPECTED_REQUIRED_RESPONSE = new List<ReportColumn>
        {
            new ReportColumn { VirtualId = 12345, Index = 4, Title = "Item selected", Type = ColumnType.CHECKBOX, Version = 0 },
            new ReportColumn { VirtualId = 12346, Index = 5, Title = "Sheet name", Type = ColumnType.TEXT_NUMBER, SheetNameColumn = true, Version = 0 },
            new ReportColumn { VirtualId = 12347, Index = 6, Title = "Created By", Type = ColumnType.CONTACT_LIST, SystemColumnType = SystemColumnType.CREATED_BY, Version = 0 },
            new ReportColumn { VirtualId = 12348, Index = 7, Title = "Primary", Type = ColumnType.TEXT_NUMBER, Primary = true, Version = 0 },
            new ReportColumn { VirtualId = 12349, Index = 8, Title = "Row Number", Type = ColumnType.TEXT_NUMBER, SystemColumnType = SystemColumnType.AUTO_NUMBER, Version = 0, AutoNumberFormat = new AutoNumberFormat { Fill = "000", Prefix = "TASK-", StartingNumber = 1, Suffix = "" } }
        };

        private static readonly List<ReportColumn> EXPECTED_ALL_RESPONSE = new List<ReportColumn>
        {
            new ReportColumn { VirtualId = 12345, Index = 4, Title = "Item selected", Type = ColumnType.CHECKBOX, Hidden = false, Validation = false, Version = 0, Width = 150 },
            new ReportColumn { VirtualId = 12346, Index = 5, Title = "Sheet name", Type = ColumnType.TEXT_NUMBER, SheetNameColumn = true, Hidden = false, Validation = false, Version = 0, Width = 150 },
            new ReportColumn { VirtualId = 12347, Index = 6, Title = "Created By", Type = ColumnType.CONTACT_LIST, SystemColumnType = SystemColumnType.CREATED_BY, Hidden = false, Validation = false, Version = 0, Width = 150 },
            new ReportColumn { VirtualId = 12348, Index = 7, Title = "Primary", Type = ColumnType.TEXT_NUMBER, Primary = true, Hidden = false, Validation = false, Version = 0, Width = 200 },
            new ReportColumn { VirtualId = 12349, Index = 8, Title = "Row Number", Type = ColumnType.TEXT_NUMBER, SystemColumnType = SystemColumnType.AUTO_NUMBER, Hidden = false, Validation = false, Version = 0, Width = 100, AutoNumberFormat = new AutoNumberFormat { Fill = "000", Prefix = "TASK-", StartingNumber = 1, Suffix = "" } }
        };

        [TestMethod]
        public void TestAddReportColumnsEmptyColumnsError()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/add-report-columns/required-response-body-properties", requestId.ToString());

            Assert.ThrowsException<ArgumentException>(
                () => smartsheet.ReportResources.AddReportColumns(CommonTestConstants.TEST_REPORT_ID, new List<ReportColumn>())
            );
        }

        [TestMethod]
        public async Task TestAddReportColumnsGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/add-report-columns/required-response-body-properties", requestId.ToString());

            smartsheet.ReportResources.AddReportColumns(CommonTestConstants.TEST_REPORT_ID, COLUMNS_TO_ADD_REQUIRED);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual($"/2.0/reports/{CommonTestConstants.TEST_REPORT_ID}/columns", uri.AbsolutePath);
            Assert.AreEqual("POST", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestAddReportColumnsRequiredResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/add-report-columns/required-response-body-properties", requestId.ToString());

            IList<ReportColumn> result = smartsheet.ReportResources.AddReportColumns(CommonTestConstants.TEST_REPORT_ID, COLUMNS_TO_ADD_REQUIRED);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.AreEqual(EXPECTED_REQUIRED_REQUEST_BODY, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_REQUIRED_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestAddReportColumnsAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/add-report-columns/all-response-body-properties", requestId.ToString());

            IList<ReportColumn> result = smartsheet.ReportResources.AddReportColumns(CommonTestConstants.TEST_REPORT_ID, COLUMNS_TO_ADD_ALL);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.AreEqual(EXPECTED_ALL_REQUEST_BODY, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ALL_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public void TestAddReportColumnsError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.AddReportColumns(CommonTestConstants.TEST_REPORT_ID, COLUMNS_TO_ADD_REQUIRED)
            );

            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestAddReportColumnsError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.AddReportColumns(CommonTestConstants.TEST_REPORT_ID, COLUMNS_TO_ADD_REQUIRED)
            );

            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }
    }
}
