using Newtonsoft.Json;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestGetReportDefinition
    {
        private static readonly ReportDefinition EXPECTED_ALL = new ReportDefinition
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
                            Title = "Primary Column",
                            Type = ColumnType.TEXT_NUMBER,
                            Primary = true,
                        },
                        Operator = ReportFilterCriteriaOperator.EQUAL,
                        Values = new List<ReportFilterValue> { new StringReportFilterValue("Test Value") },
                    },
                    new ReportFilterCriterion
                    {
                        Column = new ReportColumnIdentifier
                        {
                            Title = "Status",
                            Type = ColumnType.PICKLIST,
                        },
                        Operator = ReportFilterCriteriaOperator.NOT_EQUAL,
                        Values = new List<ReportFilterValue> { new StringReportFilterValue("Complete") },
                    },
                    new ReportFilterCriterion
                    {
                        Column = new ReportColumnIdentifier
                        {
                            Title = "Amount",
                            Type = ColumnType.TEXT_NUMBER,
                        },
                        Operator = ReportFilterCriteriaOperator.GREATER_THAN,
                        Values = new List<ReportFilterValue> { new NumberReportFilterValue(42) },
                    },
                    new ReportFilterCriterion
                    {
                        Column = new ReportColumnIdentifier
                        {
                            Type = ColumnType.DATETIME,
                            SystemColumnType = SystemColumnType.MODIFIED_DATE,
                        },
                        Operator = ReportFilterCriteriaOperator.LESS_THAN,
                        Values = new List<ReportFilterValue> { new DateReportFilterValue("2025-01-14") },
                    },
                    new ReportFilterCriterion
                    {
                        Column = new ReportColumnIdentifier
                        {
                            Title = "Assigned To",
                            Type = ColumnType.CONTACT_LIST,
                        },
                        Operator = ReportFilterCriteriaOperator.EQUAL,
                        Values = new List<ReportFilterValue> { new CurrentUserReportFilterValue() },
                    },
                    new ReportFilterCriterion
                    {
                        Column = new ReportColumnIdentifier
                        {
                            Title = "Notes",
                            Type = ColumnType.TEXT_NUMBER,
                        },
                        Operator = ReportFilterCriteriaOperator.EQUAL,
                        Values = new List<ReportFilterValue> { new NullReportFilterValue() },
                    }
                }
            },
            GroupingCriteria = new List<ReportGroupingCriterion>
            {
                new ReportGroupingCriterion
                {
                    Column = new ReportColumnIdentifier
                    {
                        Title = "Primary Column",
                        Type = ColumnType.TEXT_NUMBER,
                        Primary = true,
                    },
                    SortingDirection = SortDirection.ASCENDING,
                    IsExpanded = true
                },
                new ReportGroupingCriterion
                {
                    Column = new ReportColumnIdentifier
                    {
                        Title = "Category",
                        Type = ColumnType.TEXT_NUMBER,
                    },
                    SortingDirection = SortDirection.DESCENDING,
                    IsExpanded = false
                }
            },
            SummarizingCriteria = new List<ReportSummarizingCriterion>
            {
                new ReportSummarizingCriterion
                {
                    Column = new ReportColumnIdentifier
                    {
                        Title = "Primary Column",
                        Type = ColumnType.TEXT_NUMBER,
                        Primary = true,
                    },
                    AggregationType = ReportAggregationType.COUNT
                },
                new ReportSummarizingCriterion
                {
                    Column = new ReportColumnIdentifier
                    {
                        Title = "Amount",
                        Type = ColumnType.TEXT_NUMBER,
                    },
                    AggregationType = ReportAggregationType.SUM
                }
            },
            SortingCriteria = new List<ReportSortingCriterion>
            {
                new ReportSortingCriterion
                {
                    Column = new ReportColumnIdentifier
                    {
                        Title = "Primary Column",
                        Type = ColumnType.TEXT_NUMBER,
                        Primary = true,
                    },
                    SortingDirection = SortDirection.ASCENDING
                },
                new ReportSortingCriterion
                {
                    Column = new ReportColumnIdentifier
                    {
                        Type = ColumnType.DATETIME,
                        SystemColumnType = SystemColumnType.MODIFIED_DATE,
                    },
                    SortingDirection = SortDirection.DESCENDING
                }
            },
        };

        [TestMethod]
        public async Task TestGetReportDefinitionGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/get-report-definition/all-response-body-properties", requestId.ToString());

            smartsheet.ReportResources.GetReportDefinition(CommonTestConstants.TEST_REPORT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual($"/2.0/reports/{CommonTestConstants.TEST_REPORT_ID}/definition", path);
            Assert.AreEqual("GET", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public void TestGetReportDefinitionRequiredResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/get-report-definition/required-response-body-properties", requestId.ToString());

            ReportDefinition result = smartsheet.ReportResources.GetReportDefinition(CommonTestConstants.TEST_REPORT_ID);

            Assert.IsNotNull(result);
            Assert.IsNull(result.Filters);
            Assert.IsNull(result.GroupingCriteria);
            Assert.IsNull(result.SummarizingCriteria);
            Assert.IsNull(result.SortingCriteria);
        }

        [TestMethod]
        public void TestGetReportDefinitionAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/get-report-definition/all-response-body-properties", requestId.ToString());

            ReportDefinition result = smartsheet.ReportResources.GetReportDefinition(CommonTestConstants.TEST_REPORT_ID);

            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ALL), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public void TestGetReportDefinitionError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.GetReportDefinition(CommonTestConstants.TEST_REPORT_ID)
            );

            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestGetReportDefinitionError404Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/404-response", requestId.ToString());

            HelperFunctions.AssertRaisesException<SmartsheetException>(
                () => smartsheet.ReportResources.GetReportDefinition(CommonTestConstants.TEST_REPORT_ID),
                "Not Found"
            );
        }

        [TestMethod]
        public async Task TestGetReportDefinitionAsyncGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/get-report-definition/all-response-body-properties", requestId.ToString());

            await smartsheet.ReportResources.GetReportDefinitionAsync(CommonTestConstants.TEST_REPORT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual($"/2.0/reports/{CommonTestConstants.TEST_REPORT_ID}/definition", path);
            Assert.AreEqual("GET", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestGetReportDefinitionAsyncAllResponseProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/get-report-definition/all-response-body-properties", requestId.ToString());

            ReportDefinition result = await smartsheet.ReportResources.GetReportDefinitionAsync(CommonTestConstants.TEST_REPORT_ID);

            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ALL), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetReportDefinitionAsyncError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.ReportResources.GetReportDefinitionAsync(CommonTestConstants.TEST_REPORT_ID),
                "Internal Server Error");
        }

        [TestMethod]
        public async Task TestGetReportDefinitionAsyncError404Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/404-response", requestId.ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.ReportResources.GetReportDefinitionAsync(CommonTestConstants.TEST_REPORT_ID),
                "Not Found");
        }
    }
}
