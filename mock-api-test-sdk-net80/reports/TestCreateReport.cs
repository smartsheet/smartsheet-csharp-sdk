using Newtonsoft.Json;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestCreateReport
    {
        private static readonly CreateReportRequest REQUEST_REQUIRED = new CreateReportRequest
        {
            Name = "Q2 Earnings",
            Destination = new ReportDestination
            {
                DestinationId = 123456789,
                DestinationType = ReportDestinationType.FOLDER
            },
            Columns = new List<ReportColumn>
            {
                new ReportColumn { Title = "Primary", Type = ColumnType.TEXT_NUMBER, Index = 0, Primary = true }
            },
            Scope = new List<ReportScopeInclusion>
            {
                new ReportScopeInclusion { AssetType = ReportAssetType.SHEET, AssetId = 111111111 }
            }
        };

        private static readonly CreateReportRequest REQUEST_ALL = new CreateReportRequest
        {
            Name = "Q2 Earnings",
            Destination = new ReportDestination
            {
                DestinationId = 123456789,
                DestinationType = ReportDestinationType.FOLDER
            },
            Columns = new List<ReportColumn>
            {
                new ReportColumn { Title = "Primary", Type = ColumnType.TEXT_NUMBER, Index = 0, Primary = true }
            },
            Scope = new List<ReportScopeInclusion>
            {
                new ReportScopeInclusion { AssetType = ReportAssetType.SHEET, AssetId = 111111111 }
            },
            IsSummaryReport = false,
            ReportDefinition = new ReportDefinition
            {
                Filters = new ReportFilterExpression
                {
                    Operator = ReportFilterOperator.AND,
                    Criteria = new List<ReportFilterCriterion>
                    {
                        new ReportFilterCriterion
                        {
                            Column = new ReportColumnIdentifier
                            {
                                Type = ColumnType.TEXT_NUMBER,
                                Title = "Status"
                            },
                            Operator = ReportFilterCriteriaOperator.EQUAL,
                            Values = new List<ReportFilterValue> { new StringReportFilterValue("Complete") }
                        }
                    }
                }
            }
        };

        private static readonly string EXPECTED_REQUIRED_REQUEST_BODY = JsonConvert.SerializeObject(new Dictionary<string, object>
        {
            { "name", "Q2 Earnings" },
            { "columns", new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "type", "TEXT_NUMBER" },
                        { "index", 0 },
                        { "primary", true },
                        { "title", "Primary" }
                    }
                }
            },
            { "scope", new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "assetType", "sheet" },
                        { "assetId", 111111111L }
                    }
                }
            },
            { "destination", new Dictionary<string, object>
                {
                    { "destinationId", 123456789L },
                    { "destinationType", "folder" }
                }
            }
        });

        private static readonly string EXPECTED_ALL_REQUEST_BODY = JsonConvert.SerializeObject(new Dictionary<string, object>
        {
            { "name", "Q2 Earnings" },
            { "columns", new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "type", "TEXT_NUMBER" },
                        { "index", 0 },
                        { "primary", true },
                        { "title", "Primary" }
                    }
                }
            },
            { "scope", new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "assetType", "sheet" },
                        { "assetId", 111111111L }
                    }
                }
            },
            { "reportDefinition", new Dictionary<string, object>
                {
                    { "filters", new Dictionary<string, object>
                        {
                            { "operator", "AND" },
                            { "criteria", new List<Dictionary<string, object>>
                                {
                                    new Dictionary<string, object>
                                    {
                                        { "column", new Dictionary<string, object>
                                            {
                                                { "title", "Status" },
                                                { "type", "TEXT_NUMBER" }
                                            }
                                        },
                                        { "operator", "EQUAL" },
                                        { "values", new List<string> { "Complete" } }
                                    }
                                }
                            }
                        }
                    }
                }
            },
            { "isSummaryReport", false },
            { "destination", new Dictionary<string, object>
                {
                    { "destinationId", 123456789L },
                    { "destinationType", "folder" }
                }
            }
        });

        private static readonly CreateReportResult EXPECTED_REQUIRED_RESPONSE = new CreateReportResult
        {
            Id = 987654321L,
            Name = "Q2 Earnings Report",
            AccessLevel = AccessLevel.OWNER,
            Permalink = "https://app.smartsheet.com/reports/c8gJxw87cXpRCvCC5PPw6jFhFRrf5r8PxCrxvW21"
        };

        private static readonly CreateReportResult EXPECTED_ALL_RESPONSE = new CreateReportResult
        {
            Id = 987654321L,
            Name = "Q2 Earnings Report",
            AccessLevel = AccessLevel.OWNER,
            Permalink = "https://app.smartsheet.com/reports/c8gJxw87cXpRCvCC5PPw6jFhFRrf5r8PxCrxvW21",
            IsSummaryReport = false,
            Columns = new List<ReportColumn>
            {
                new ReportColumn { VirtualId = 1234567890123456, Index = 0, Title = "Primary column", Type = ColumnType.TEXT_NUMBER, Primary = true, Hidden = false, Version = 0, Width = 200, Validation = false },
                new ReportColumn { VirtualId = 2345678901234567, Index = 1, Title = "Sheet name", Type = ColumnType.TEXT_NUMBER, SheetNameColumn = true, Hidden = false, Version = 0, Width = 150, Validation = false },
                new ReportColumn { VirtualId = 3456789012345678, Index = 2, Title = "Created at", Type = ColumnType.DATETIME, SystemColumnType = SystemColumnType.CREATED_DATE, Hidden = false, Version = 0, Width = 150, Validation = false },
                new ReportColumn { VirtualId = 4567890123456789, Index = 3, Title = "Selected item", Type = ColumnType.PICKLIST, Hidden = false, Version = 0, Width = 150, Validation = false }
            }
        };

        [TestMethod]
        public async Task TestCreateReportGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/create-report/required-response-body-properties", requestId.ToString());

            smartsheet.ReportResources.CreateReport(REQUEST_REQUIRED);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual("/2.0/reports", uri.AbsolutePath);
            Assert.AreEqual("POST", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestCreateReportRequiredResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/create-report/required-response-body-properties", requestId.ToString());

            CreateReportResult result = smartsheet.ReportResources.CreateReport(REQUEST_REQUIRED);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.AreEqual(EXPECTED_REQUIRED_REQUEST_BODY, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_REQUIRED_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestCreateReportAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/create-report/all-response-body-properties", requestId.ToString());

            CreateReportResult result = smartsheet.ReportResources.CreateReport(REQUEST_ALL);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.AreEqual(EXPECTED_ALL_REQUEST_BODY, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ALL_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public void TestCreateReportError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.CreateReport(REQUEST_REQUIRED)
            );

            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestCreateReportError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.CreateReport(REQUEST_REQUIRED)
            );

            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }
    }
}
