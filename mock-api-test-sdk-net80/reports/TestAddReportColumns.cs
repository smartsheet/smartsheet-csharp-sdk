using Newtonsoft.Json;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestAddReportColumns
    {
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
        public async Task TestAddReportColumnsRequiredGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/add-report-columns/required-response-body-properties", requestId.ToString());

            var columnsToAdd = new List<ReportColumn>
            {
                new ReportColumn
                {
                    Title = "Item selected",
                    Type = ColumnType.CHECKBOX,
                    Index = 4
                },
                new ReportColumn
                {
                    Title = "Sheet name",
                    Type = ColumnType.TEXT_NUMBER,
                    Index = 5
                }
            };

            smartsheet.ReportResources.AddReportColumns(CommonTestConstants.TEST_REPORT_ID, columnsToAdd);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual($"/2.0/reports/{CommonTestConstants.TEST_REPORT_ID}/columns", path);
            Assert.AreEqual("POST", foundRequest.Method);
        }

        [TestMethod]
        public async Task TestAddReportColumnsRequiredResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/add-report-columns/required-response-body-properties", requestId.ToString());

            var columnsToAdd = new List<ReportColumn>
            {
                new ReportColumn
                {
                    Title = "Item selected",
                    Type = ColumnType.CHECKBOX,
                    Index = 4
                },
                new ReportColumn
                {
                    Title = "Sheet name",
                    Type = ColumnType.TEXT_NUMBER,
                    Index = 5
                }
            };

            IList<ReportColumn> result = smartsheet.ReportResources.AddReportColumns(CommonTestConstants.TEST_REPORT_ID, columnsToAdd);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

            var expected0 = new Dictionary<string, object?>
            {
                { "VirtualId", 12345L },
                { "SheetNameColumn", null },
                { "SystemColumnType", null },
                { "Type", 4 },
                { "AutoNumberFormat", null },
                { "ContactOptions", null },
                { "Description", null },
                { "Format", null },
                { "Formula", null },
                { "Hidden", null },
                { "Index", 4 },
                { "Locked", null },
                { "LockedForUser", null },
                { "Options", null },
                { "Primary", null },
                { "Symbol", null },
                { "Tags", null },
                { "Title", "Item selected" },
                { "Validation", null },
                { "Version", null },
                { "Width", null },
                { "Id", null }
            };

            var expected1 = new Dictionary<string, object?>
            {
                { "VirtualId", 12346L },
                { "SheetNameColumn", null },
                { "SystemColumnType", null },
                { "Type", 0 },
                { "AutoNumberFormat", null },
                { "ContactOptions", null },
                { "Description", null },
                { "Format", null },
                { "Formula", null },
                { "Hidden", null },
                { "Index", 4 },
                { "Locked", null },
                { "LockedForUser", null },
                { "Options", null },
                { "Primary", null },
                { "Symbol", null },
                { "Tags", null },
                { "Title", "Sheet name" },
                { "Validation", null },
                { "Version", null },
                { "Width", null },
                { "Id", null }
            };

            var actualJson0 = JsonConvert.SerializeObject(result[0]);
            var actualJson1 = JsonConvert.SerializeObject(result[1]);
            var expectedJson0 = JsonConvert.SerializeObject(expected0);
            var expectedJson1 = JsonConvert.SerializeObject(expected1);

            Assert.AreEqual(expectedJson0, actualJson0);
            Assert.AreEqual(expectedJson1, actualJson1);
        }

        [TestMethod]
        public async Task TestAddReportColumnsAllGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/add-report-columns/all-response-body-properties", requestId.ToString());

            var columnsToAdd = new List<ReportColumn>
            {
                new ReportColumn
                {
                    Title = "Item selected",
                    Type = ColumnType.CHECKBOX,
                    Index = 4,
                    Hidden = false,
                    Width = 150
                },
                new ReportColumn
                {
                    Title = "Sheet name",
                    Type = ColumnType.TEXT_NUMBER,
                    Index = 5,
                    Hidden = false,
                    Width = 150,
                    SheetNameColumn = true
                }
            };

            smartsheet.ReportResources.AddReportColumns(CommonTestConstants.TEST_REPORT_ID, columnsToAdd);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual($"/2.0/reports/{CommonTestConstants.TEST_REPORT_ID}/columns", path);
            Assert.AreEqual("POST", foundRequest.Method);
        }

        [TestMethod]
        public async Task TestAddReportColumnsAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/add-report-columns/all-response-body-properties", requestId.ToString());

            var columnsToAdd = new List<ReportColumn>
            {
                new ReportColumn
                {
                    Title = "Item selected",
                    Type = ColumnType.CHECKBOX,
                    Index = 4,
                    Hidden = false,
                    Width = 150
                },
                new ReportColumn
                {
                    Title = "Sheet name",
                    Type = ColumnType.TEXT_NUMBER,
                    Index = 5,
                    Hidden = false,
                    Width = 150,
                    SheetNameColumn = true
                }
            };

            IList<ReportColumn> result = smartsheet.ReportResources.AddReportColumns(CommonTestConstants.TEST_REPORT_ID, columnsToAdd);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

            var expected0 = new Dictionary<string, object?>
            {
                { "VirtualId", 12345L },
                { "SheetNameColumn", null },
                { "SystemColumnType", null },
                { "Type", 4 },
                { "AutoNumberFormat", null },
                { "ContactOptions", null },
                { "Description", null },
                { "Format", null },
                { "Formula", null },
                { "Hidden", false },
                { "Index", 4 },
                { "Locked", null },
                { "LockedForUser", null },
                { "Options", null },
                { "Primary", null },
                { "Symbol", null },
                { "Tags", null },
                { "Title", "Item selected" },
                { "Validation", null },
                { "Version", 0 },
                { "Width", 150 },
                { "Id", null }
            };

            var expected1 = new Dictionary<string, object?>
            {
                { "VirtualId", 12346L },
                { "SheetNameColumn", true },
                { "SystemColumnType", null },
                { "Type", 0 },
                { "AutoNumberFormat", null },
                { "ContactOptions", null },
                { "Description", null },
                { "Format", null },
                { "Formula", null },
                { "Hidden", false },
                { "Index", 4 },
                { "Locked", null },
                { "LockedForUser", null },
                { "Options", null },
                { "Primary", null },
                { "Symbol", null },
                { "Tags", null },
                { "Title", "Sheet name" },
                { "Validation", null },
                { "Version", 0 },
                { "Width", 150 },
                { "Id", null }
            };

            var actualJson0 = JsonConvert.SerializeObject(result[0]);
            var actualJson1 = JsonConvert.SerializeObject(result[1]);
            var expectedJson0 = JsonConvert.SerializeObject(expected0);
            var expectedJson1 = JsonConvert.SerializeObject(expected1);

            Assert.AreEqual(expectedJson0, actualJson0);
            Assert.AreEqual(expectedJson1, actualJson1);
        }

        [TestMethod]
        public async Task TestAddReportColumnsRequiredRequestBody()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/add-report-columns/required-response-body-properties", requestId.ToString());

            var columnsToAdd = new List<ReportColumn>
            {
                new ReportColumn
                {
                    Title = "Item selected",
                    Type = ColumnType.CHECKBOX,
                    Index = 4
                },
                new ReportColumn
                {
                    Title = "Sheet name",
                    Type = ColumnType.TEXT_NUMBER,
                    Index = 5
                }
            };

            smartsheet.ReportResources.AddReportColumns(CommonTestConstants.TEST_REPORT_ID, columnsToAdd);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            var expectedBody = JsonConvert.SerializeObject(new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "type", "CHECKBOX" },
                    { "index", 4 },
                    { "title", "Item selected" }
                },
                new Dictionary<string, object>
                {
                    { "type", "TEXT_NUMBER" },
                    { "index", 5 },
                    { "title", "Sheet name" }
                }
            });

            Assert.AreEqual(expectedBody, foundRequest.Body);
        }

        [TestMethod]
        public async Task TestAddReportColumnsAllRequestBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/add-report-columns/all-response-body-properties", requestId.ToString());

            var columnsToAdd = new List<ReportColumn>
            {
                new ReportColumn
                {
                    Title = "Item selected",
                    Type = ColumnType.CHECKBOX,
                    Index = 4,
                    Hidden = false,
                    Width = 150
                },
                new ReportColumn
                {
                    Title = "Sheet name",
                    Type = ColumnType.TEXT_NUMBER,
                    Index = 5,
                    Hidden = false,
                    Width = 150,
                    SheetNameColumn = true
                }
            };

            smartsheet.ReportResources.AddReportColumns(CommonTestConstants.TEST_REPORT_ID, columnsToAdd);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            var expectedBody = JsonConvert.SerializeObject(new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "type", "CHECKBOX" },
                    { "hidden", false },
                    { "index", 4 },
                    { "title", "Item selected" },
                    { "width", 150 }
                },
                new Dictionary<string, object>
                {
                    { "sheetNameColumn", true },
                    { "type", "TEXT_NUMBER" },
                    { "hidden", false },
                    { "index", 5 },
                    { "title", "Sheet name" },
                    { "width", 150 }
                }
            });

            Assert.AreEqual(expectedBody, foundRequest.Body);
        }

        [TestMethod]
        public void TestAddReportColumnsError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            var columnsToAdd = new List<ReportColumn>
            {
                new ReportColumn
                {
                    Title = "Item selected",
                    Type = ColumnType.CHECKBOX,
                    Index = 4
                }
            };

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.AddReportColumns(CommonTestConstants.TEST_REPORT_ID, columnsToAdd)
            );

            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestAddReportColumnsError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            var columnsToAdd = new List<ReportColumn>
            {
                new ReportColumn
                {
                    Title = "Item selected",
                    Type = ColumnType.CHECKBOX,
                    Index = 4
                }
            };

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.AddReportColumns(CommonTestConstants.TEST_REPORT_ID, columnsToAdd)
            );

            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }
    }
}
