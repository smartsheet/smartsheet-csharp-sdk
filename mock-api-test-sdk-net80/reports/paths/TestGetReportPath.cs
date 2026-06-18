using Newtonsoft.Json;
using Smartsheet.Api;
using Smartsheet.Api.Models;
using System.Globalization;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestGetReportPath
    {
        private static readonly ReportPathNode EXPECTED_NESTED_RESPONSE = new ReportPathNode
        {
            Id = CommonTestConstants.TEST_PATH_WORKSPACE_ID,
            Name = CommonTestConstants.TEST_PATH_WORKSPACE_NAME,
            Permalink = CommonTestConstants.TEST_PATH_WORKSPACE_PERMALINK,
            AccessLevel = AccessLevel.OWNER,
            Folders = new List<ReportPathNode>
            {
                new ReportPathNode
                {
                    Id = CommonTestConstants.TEST_PATH_FOLDER_1_ID,
                    Name = CommonTestConstants.TEST_PATH_FOLDER_1_NAME,
                    Permalink = CommonTestConstants.TEST_PATH_FOLDER_1_PERMALINK,
                    Folders = new List<ReportPathNode>
                    {
                        new ReportPathNode
                        {
                            Id = CommonTestConstants.TEST_PATH_FOLDER_2_ID,
                            Name = CommonTestConstants.TEST_PATH_FOLDER_2_NAME,
                            Permalink = CommonTestConstants.TEST_PATH_FOLDER_2_PERMALINK,
                            Reports = new List<PathLeaf>
                            {
                                new PathLeaf
                                {
                                    Id = CommonTestConstants.TEST_PATH_NESTED_LEAF_ID,
                                    Name = "Project Report",
                                    Permalink = "https://app.smartsheet.com/reports/3456789012345678",
                                    AccessLevel = AccessLevel.ADMIN,
                                    CreatedAt = DateTime.Parse("2024-01-01T00:00:00Z", null, DateTimeStyles.RoundtripKind),
                                    ModifiedAt = DateTime.Parse("2024-06-01T00:00:00Z", null, DateTimeStyles.RoundtripKind)
                                }
                            }
                        }
                    }
                }
            }
        };

        private static readonly ReportPathNode EXPECTED_ROOT_RESPONSE = new ReportPathNode
        {
            Id = CommonTestConstants.TEST_PATH_WORKSPACE_ID,
            Name = CommonTestConstants.TEST_PATH_WORKSPACE_NAME,
            Permalink = CommonTestConstants.TEST_PATH_WORKSPACE_PERMALINK,
            AccessLevel = AccessLevel.OWNER,
            Reports = new List<PathLeaf>
            {
                new PathLeaf
                {
                    Id = CommonTestConstants.TEST_PATH_ROOT_LEAF_ID,
                    Name = "Root Level Report",
                    Permalink = "https://app.smartsheet.com/reports/rootlevel",
                    AccessLevel = AccessLevel.ADMIN,
                    CreatedAt = DateTime.Parse("2024-01-01T00:00:00Z", null, DateTimeStyles.RoundtripKind),
                    ModifiedAt = DateTime.Parse("2024-06-01T00:00:00Z", null, DateTimeStyles.RoundtripKind)
                }
            }
        };

        [TestMethod]
        public async Task TestGetReportPathGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/get-nested-report-path/all-response-body-properties", requestId.ToString());

            smartsheet.ReportResources.GetReportPath(CommonTestConstants.TEST_REPORT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.IsNotNull(foundRequest);
            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual(string.Format("/2.0/reports/{0}/path", CommonTestConstants.TEST_REPORT_ID), uri.AbsolutePath);
            Assert.AreEqual("GET", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestGetReportPathNestedAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/get-nested-report-path/all-response-body-properties", requestId.ToString());

            ReportPathNode result = smartsheet.ReportResources.GetReportPath(CommonTestConstants.TEST_REPORT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.AreEqual(string.Empty, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_NESTED_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetReportPathRootAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/get-root-report-path/all-response-body-properties", requestId.ToString());

            ReportPathNode result = smartsheet.ReportResources.GetReportPath(CommonTestConstants.TEST_REPORT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.AreEqual(string.Empty, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ROOT_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetReportPathNestedLeafHelpers()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/get-nested-report-path/all-response-body-properties", requestId.ToString());

            ReportPathNode result = smartsheet.ReportResources.GetReportPath(CommonTestConstants.TEST_REPORT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            PathLeaf? leaf = result.GetLeafReport();
            Assert.IsNotNull(leaf);
            Assert.AreEqual(CommonTestConstants.TEST_PATH_NESTED_LEAF_ID, leaf.Id);
            Assert.AreEqual("Project Report", leaf.Name);
            Assert.AreEqual("/Sample Workspace/Project Plans/Project Plans Subfolder/Project Report", result.GetLeafReportPath());
        }

        [TestMethod]
        public async Task TestGetReportPathRootLeafHelpers()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/get-root-report-path/all-response-body-properties", requestId.ToString());

            ReportPathNode result = smartsheet.ReportResources.GetReportPath(CommonTestConstants.TEST_REPORT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            PathLeaf? leaf = result.GetLeafReport();
            Assert.IsNotNull(leaf);
            Assert.AreEqual(CommonTestConstants.TEST_PATH_ROOT_LEAF_ID, leaf.Id);
            Assert.AreEqual("Root Level Report", leaf.Name);
            Assert.AreEqual("/Sample Workspace/Root Level Report", result.GetLeafReportPath());
        }

        [TestMethod]
        public void TestGetReportPathError400Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", Guid.NewGuid().ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.GetReportPath(CommonTestConstants.TEST_REPORT_ID));
            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }

        [TestMethod]
        public void TestGetReportPathError404Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/404-response", Guid.NewGuid().ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.GetReportPath(CommonTestConstants.TEST_REPORT_ID));
            Assert.IsTrue(exception.Message.Contains("Not Found"));
        }

        [TestMethod]
        public void TestGetReportPathError500Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", Guid.NewGuid().ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.ReportResources.GetReportPath(CommonTestConstants.TEST_REPORT_ID));
            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public async Task TestGetReportPathAsyncGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/get-nested-report-path/all-response-body-properties", requestId.ToString());

            await smartsheet.ReportResources.GetReportPathAsync(CommonTestConstants.TEST_REPORT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.IsNotNull(foundRequest);
            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual(string.Format("/2.0/reports/{0}/path", CommonTestConstants.TEST_REPORT_ID), uri.AbsolutePath);
            Assert.AreEqual("GET", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestGetReportPathAsyncNestedAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/get-nested-report-path/all-response-body-properties", requestId.ToString());

            ReportPathNode result = await smartsheet.ReportResources.GetReportPathAsync(CommonTestConstants.TEST_REPORT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.AreEqual(string.Empty, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_NESTED_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetReportPathAsyncRootAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/reports/get-root-report-path/all-response-body-properties", requestId.ToString());

            ReportPathNode result = await smartsheet.ReportResources.GetReportPathAsync(CommonTestConstants.TEST_REPORT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.AreEqual(string.Empty, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ROOT_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetReportPathAsyncError400Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", Guid.NewGuid().ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.ReportResources.GetReportPathAsync(CommonTestConstants.TEST_REPORT_ID),
                "Malformed Request");
        }

        [TestMethod]
        public async Task TestGetReportPathAsyncError404Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/404-response", Guid.NewGuid().ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.ReportResources.GetReportPathAsync(CommonTestConstants.TEST_REPORT_ID),
                "Not Found");
        }

        [TestMethod]
        public async Task TestGetReportPathAsyncError500Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", Guid.NewGuid().ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.ReportResources.GetReportPathAsync(CommonTestConstants.TEST_REPORT_ID),
                "Internal Server Error");
        }
    }
}
