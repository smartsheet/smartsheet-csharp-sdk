using Newtonsoft.Json;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestUpdateReportDefinition
    {
        [TestMethod]
        public async Task TestUpdateReportDefinitionAllGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/update-report-definition/all-response-body-properties", requestId.ToString());


            smartsheet.ReportResources.UpdateReportDefinition(CommonTestConstants.TEST_REPORT_ID, new ReportDefinition());

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            
            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual($"/2.0/reports/{CommonTestConstants.TEST_REPORT_ID}/definition", path);
            Assert.AreEqual("PATCH", foundRequest.Method);
        }

        [TestMethod]
        public async Task TestUpdateReportDefinitionAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/update-report-definition/all-response-body-properties", requestId.ToString());

            ReportDefinition definition = new ReportDefinition
            {
                AggregationCriteria = new List<ReportAggregationCriterion>
                {
                    new ReportAggregationCriterion
                    {
                        AggregationType = ReportAggregationType.COUNT,
                        Column = new ReportColumnIdentifier
                        {
                            Primary = true,
                            Type = ColumnType.TEXT_NUMBER,
                            SystemColumnType = ReportSystemColumnType.SHEET_NAME,
                            Title = "Primary Column",
                        },
                    IsExpanded = true
                    }
                },
                Filters = new ReportFilterExpression
                {
                    Operator = ReportFilterOperator.AND,
                    Criteria = new List<ReportFilterCriterion>
                    {
                        new ReportFilterCriterion
                        {
                            Column = new ReportColumnIdentifier
                            {
                                Primary = true,
                                Type = ColumnType.TEXT_NUMBER,
                                SystemColumnType = ReportSystemColumnType.SHEET_NAME,
                                Title = "Primary Column",
                            },
                            Operator = ReportFilterCriteriaOperator.EQUAL,
                            Values = new List<string> { "Test" },
                        }
                    }
                },
                GroupingCriteria = new List<ReportGroupingCriterion>
                {
                    new ReportGroupingCriterion
                    {
                        Column = new ReportColumnIdentifier
                        {
                            Primary = true,
                            Type = ColumnType.TEXT_NUMBER,
                            SystemColumnType = ReportSystemColumnType.SHEET_NAME,
                            Title = "Primary Column",
                        },
                        SortingDirection = SortDirection.ASCENDING,
                        IsExpanded = true
                    }
                },
                SortingCriteria = new List<ReportSortingCriterion>
                {
                    new ReportSortingCriterion
                    {
                        Column = new ReportColumnIdentifier
                        {
                            Primary = true,
                            Type = ColumnType.TEXT_NUMBER,
                            SystemColumnType = ReportSystemColumnType.SHEET_NAME,
                            Title = "Primary Column",
                        },
                        SortingDirection = SortDirection.ASCENDING
                    }
                },
            };

            smartsheet.ReportResources.UpdateReportDefinition(CommonTestConstants.TEST_REPORT_ID, definition);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            
            var expectedBody = JsonConvert.SerializeObject(new Dictionary<string, object>
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
                                                { "title", "Primary Column" },
                                                { "type", "TEXT_NUMBER" },
                                                { "systemColumnType", "SHEET_NAME" },
                                                { "primary", true },
                                            }
                                        },
                                        { "operator", "EQUAL" },
                                        { "values", new List<string> { "Test" } }
                                    }
                                }
                            }
                        }
                },
                { "groupingCriteria", new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            { "column", new Dictionary<string, object>
                                {
                                    { "title", "Primary Column" },
                                    { "type", "TEXT_NUMBER" },
                                    { "systemColumnType", "SHEET_NAME" },
                                    { "primary", true },
                                }
                            },
                            { "sortingDirection", "ASCENDING" },
                            { "isExpanded", true }
                        }
                    }
                },
                { "aggregationCriteria", new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            { "column", new Dictionary<string, object>
                                {
                                    { "title", "Primary Column" },
                                    { "type", "TEXT_NUMBER" },
                                    { "systemColumnType", "SHEET_NAME" },
                                    { "primary", true },
                                }
                            },
                            { "aggregationType", "COUNT" },
                            { "isExpanded", true }
                        }
                    }
                },
                { "sortingCriteria", new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            { "column", new Dictionary<string, object>
                                {
                                    { "title", "Primary Column" },
                                    { "type", "TEXT_NUMBER" },
                                    { "systemColumnType", "SHEET_NAME" },
                                    { "primary", true },
                                }
                            },
                            { "sortingDirection", "ASCENDING" },
                        }
                    }
                },
            });
            
            Assert.AreEqual(expectedBody, foundRequest.Body);
        }

        [TestMethod]
        public void TestUpdateReportDefinitionError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.UpdateReportDefinition(CommonTestConstants.TEST_REPORT_ID, new ReportDefinition())
            );
            
            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestUpdateReportDefinitionError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.UpdateReportDefinition(CommonTestConstants.TEST_REPORT_ID, new ReportDefinition())
            );
            
            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }
    }
}