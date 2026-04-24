using Newtonsoft.Json;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestCreateReport
    {
        [TestMethod]
        public async Task TestCreateReportRequiredGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/create-report/required-response-body-properties", requestId.ToString());

            var request = new CreateReportRequest
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

            smartsheet.ReportResources.CreateReport(request);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual("/2.0/reports", path);
            Assert.AreEqual("POST", foundRequest.Method);
        }

        [TestMethod]
        public async Task TestCreateReportRequiredResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/create-report/required-response-body-properties", requestId.ToString());

            var request = new CreateReportRequest
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

            CreateReportResult result = smartsheet.ReportResources.CreateReport(request);

            Assert.IsNotNull(result);
            Assert.AreEqual(4583614634583940L, result.Id);
            Assert.AreEqual("Q2 Earnings", result.Name);
            Assert.AreEqual(AccessLevel.OWNER, result.AccessLevel);
            Assert.AreEqual("https://app.smartsheet.com/reports/c8gJxw87cXpRCvCC5PPw6jFhFRrf5r8PxCrxvW21", result.Permalink);
        }

        [TestMethod]
        public async Task TestCreateReportAllGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/create-report/all-response-body-properties", requestId.ToString());

            var request = new CreateReportRequest
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
                IsSummaryReport = false
            };

            smartsheet.ReportResources.CreateReport(request);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual("/2.0/reports", path);
            Assert.AreEqual("POST", foundRequest.Method);
        }

        [TestMethod]
        public async Task TestCreateReportAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/create-report/all-response-body-properties", requestId.ToString());

            var request = new CreateReportRequest
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
                IsSummaryReport = false
            };

            CreateReportResult result = smartsheet.ReportResources.CreateReport(request);

            Assert.IsNotNull(result);
            Assert.AreEqual(4583614634583940L, result.Id);
            Assert.AreEqual("Q2 Earnings", result.Name);
            Assert.AreEqual(AccessLevel.OWNER, result.AccessLevel);
            Assert.AreEqual("https://app.smartsheet.com/reports/c8gJxw87cXpRCvCC5PPw6jFhFRrf5r8PxCrxvW21", result.Permalink);
            Assert.AreEqual(false, result.IsSummaryReport);
        }

        [TestMethod]
        public async Task TestCreateReportRequiredRequestBody()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/create-report/required-response-body-properties", requestId.ToString());

            var request = new CreateReportRequest
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

            smartsheet.ReportResources.CreateReport(request);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            var expectedBody = JsonConvert.SerializeObject(new Dictionary<string, object>
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

            Assert.AreEqual(expectedBody, foundRequest.Body);
        }

        [TestMethod]
        public async Task TestCreateReportAllRequestBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/create-report/all-response-body-properties", requestId.ToString());

            var request = new CreateReportRequest
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

            smartsheet.ReportResources.CreateReport(request);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            var expectedBody = JsonConvert.SerializeObject(new Dictionary<string, object>
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

            Assert.AreEqual(expectedBody, foundRequest.Body);
        }

        [TestMethod]
        public void TestCreateReportError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            var request = new CreateReportRequest
            {
                Name = "Q2 Earnings",
                Destination = new ReportDestination
                {
                    DestinationId = 123456789,
                    DestinationType = ReportDestinationType.FOLDER
                },
                Columns = new List<ReportColumn>
                {
                    new ReportColumn { Title = "Primary", Type = ColumnType.TEXT_NUMBER, Index = 0 }
                },
                Scope = new List<ReportScopeInclusion>
                {
                    new ReportScopeInclusion { AssetType = ReportAssetType.SHEET, AssetId = 111111111 }
                }
            };

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.CreateReport(request)
            );

            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestCreateReportError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            var request = new CreateReportRequest
            {
                Name = "Q2 Earnings",
                Destination = new ReportDestination
                {
                    DestinationId = 123456789,
                    DestinationType = ReportDestinationType.FOLDER
                },
                Columns = new List<ReportColumn>
                {
                    new ReportColumn { Title = "Primary", Type = ColumnType.TEXT_NUMBER, Index = 0 }
                },
                Scope = new List<ReportScopeInclusion>
                {
                    new ReportScopeInclusion { AssetType = ReportAssetType.SHEET, AssetId = 111111111 }
                }
            };

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.CreateReport(request)
            );

            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }
    }
}
