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
            Assert.AreEqual("PUT", foundRequest.Method);
        }

        [TestMethod]
        public async Task TestUpdateReportDefinitionAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/update-report-definition/all-response-body-properties", requestId.ToString());

            ReportDefinition definition = new ReportDefinition
            {
                SummarizingCriteria = new List<ReportSummarizingCriterion>
                {
                    new ReportSummarizingCriterion
                    {
                        AggregationType = ReportAggregationType.COUNT,
                        Column = new ReportColumnIdentifier
                        {
                            Primary = true,
                            Type = ColumnType.TEXT_NUMBER,
                            SystemColumnType = ReportSystemColumnType.AUTO_NUMBER,
                            Title = "Primary Column",
                        }
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
                                SystemColumnType = ReportSystemColumnType.AUTO_NUMBER,
                                Title = "Primary Column",
                            },
                            Operator = ReportFilterCriteriaOperator.EQUAL,
                            Values = new List<FilterValue> { new StringFilterValue("Test") },
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
                            SystemColumnType = ReportSystemColumnType.AUTO_NUMBER,
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
                            SystemColumnType = ReportSystemColumnType.AUTO_NUMBER,
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
                                                { "systemColumnType", "AUTO_NUMBER" },
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
                                    { "systemColumnType", "AUTO_NUMBER" },
                                    { "primary", true },
                                }
                            },
                            { "sortingDirection", "ASCENDING" },
                            { "isExpanded", true }
                        }
                    }
                },
                { "summarizingCriteria", new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            { "column", new Dictionary<string, object>
                                {
                                    { "title", "Primary Column" },
                                    { "type", "TEXT_NUMBER" },
                                    { "systemColumnType", "AUTO_NUMBER" },
                                    { "primary", true },
                                }
                            },
                            { "aggregationType", "COUNT" }
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
                                    { "systemColumnType", "AUTO_NUMBER" },
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
        public async Task TestUpdateReportDefinitionEmptyBody()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/update-report-definition/all-response-body-properties", requestId.ToString());

            smartsheet.ReportResources.UpdateReportDefinition(CommonTestConstants.TEST_REPORT_ID, new ReportDefinition());

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            var expectedBody = JsonConvert.SerializeObject(new Dictionary<string, object>());

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

        [TestMethod]
        public async Task TestUpdateReportDefinitionWithAllFilterValueTypes()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/update-report-definition/all-response-body-properties", requestId.ToString());

            ReportDefinition definition = new ReportDefinition
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
                                Primary = true,
                                Type = ColumnType.TEXT_NUMBER,
                                Title = "Primary Column",
                            },
                            Operator = ReportFilterCriteriaOperator.EQUAL,
                            Values = new List<FilterValue>
                            {
                                new StringFilterValue("Test String"),
                                new NumberFilterValue(42.5),
                                new NullFilterValue(),
                                new DateFilterValue("2024-01-15"),
                                new CurrentUserFilterValue()
                            },
                        }
                    }
                }
            };

            smartsheet.ReportResources.UpdateReportDefinition(CommonTestConstants.TEST_REPORT_ID, definition);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            // Verify the request body contains all value types
            Assert.IsTrue(foundRequest.Body.Contains("\"Test String\""));
            Assert.IsTrue(foundRequest.Body.Contains("42.5"));
            Assert.IsTrue(foundRequest.Body.Contains("null"));
            Assert.IsTrue(foundRequest.Body.Contains("\"objectType\":\"DATE\""));
            Assert.IsTrue(foundRequest.Body.Contains("\"value\":\"2024-01-15\""));
            Assert.IsTrue(foundRequest.Body.Contains("\"objectType\":\"CURRENT_USER\""));
        }
    }
}