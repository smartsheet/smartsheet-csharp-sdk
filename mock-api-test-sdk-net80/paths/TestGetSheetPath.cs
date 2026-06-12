using Newtonsoft.Json;
using Smartsheet.Api;
using Smartsheet.Api.Models;
using System.Globalization;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestGetSheetPath
    {
        private static readonly SheetPathNode EXPECTED_NESTED_RESPONSE = new SheetPathNode
        {
            Id = CommonTestConstants.TEST_PATH_WORKSPACE_ID,
            Name = CommonTestConstants.TEST_PATH_WORKSPACE_NAME,
            Permalink = CommonTestConstants.TEST_PATH_WORKSPACE_PERMALINK,
            AccessLevel = AccessLevel.OWNER,
            Folders = new List<SheetPathNode>
            {
                new SheetPathNode
                {
                    Id = CommonTestConstants.TEST_PATH_FOLDER_1_ID,
                    Name = CommonTestConstants.TEST_PATH_FOLDER_1_NAME,
                    Permalink = CommonTestConstants.TEST_PATH_FOLDER_1_PERMALINK,
                    Folders = new List<SheetPathNode>
                    {
                        new SheetPathNode
                        {
                            Id = CommonTestConstants.TEST_PATH_FOLDER_2_ID,
                            Name = CommonTestConstants.TEST_PATH_FOLDER_2_NAME,
                            Permalink = CommonTestConstants.TEST_PATH_FOLDER_2_PERMALINK,
                            Sheets = new List<PathLeaf>
                            {
                                new PathLeaf
                                {
                                    Id = CommonTestConstants.TEST_PATH_NESTED_LEAF_ID,
                                    Name = "Project Plan",
                                    Permalink = "https://app.smartsheet.com/sheets/3456789012345678",
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

        private static readonly SheetPathNode EXPECTED_ROOT_RESPONSE = new SheetPathNode
        {
            Id = CommonTestConstants.TEST_PATH_WORKSPACE_ID,
            Name = CommonTestConstants.TEST_PATH_WORKSPACE_NAME,
            Permalink = CommonTestConstants.TEST_PATH_WORKSPACE_PERMALINK,
            AccessLevel = AccessLevel.OWNER,
            Sheets = new List<PathLeaf>
            {
                new PathLeaf
                {
                    Id = CommonTestConstants.TEST_PATH_ROOT_LEAF_ID,
                    Name = "Root Level Sheet",
                    Permalink = "https://app.smartsheet.com/sheets/rootlevel",
                    AccessLevel = AccessLevel.ADMIN,
                    CreatedAt = DateTime.Parse("2024-01-01T00:00:00Z", null, DateTimeStyles.RoundtripKind),
                    ModifiedAt = DateTime.Parse("2024-06-01T00:00:00Z", null, DateTimeStyles.RoundtripKind)
                }
            }
        };

        [TestMethod]
        public async Task TestGetSheetPathGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sheets/get-nested-sheet-path/all-response-body-properties", requestId.ToString());

            smartsheet.SheetResources.GetSheetPath(CommonTestConstants.TEST_SHEET_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.IsNotNull(foundRequest);
            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual(string.Format("/2.0/sheets/{0}/path", CommonTestConstants.TEST_SHEET_ID), uri.AbsolutePath);
            Assert.AreEqual("GET", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestGetSheetPathNestedAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sheets/get-nested-sheet-path/all-response-body-properties", requestId.ToString());

            SheetPathNode result = smartsheet.SheetResources.GetSheetPath(CommonTestConstants.TEST_SHEET_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.AreEqual(string.Empty, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_NESTED_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetSheetPathRootAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sheets/get-root-sheet-path/all-response-body-properties", requestId.ToString());

            SheetPathNode result = smartsheet.SheetResources.GetSheetPath(CommonTestConstants.TEST_SHEET_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.AreEqual(string.Empty, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ROOT_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetSheetPathNestedLeafHelpers()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sheets/get-nested-sheet-path/all-response-body-properties", requestId.ToString());

            SheetPathNode result = smartsheet.SheetResources.GetSheetPath(CommonTestConstants.TEST_SHEET_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            PathLeaf? leaf = result.GetLeafSheet();
            Assert.IsNotNull(leaf);
            Assert.AreEqual(CommonTestConstants.TEST_PATH_NESTED_LEAF_ID, leaf.Id);
            Assert.AreEqual("Project Plan", leaf.Name);
            Assert.AreEqual("/Sample Workspace/Project Plans/Project Plans Subfolder/Project Plan", result.GetLeafSheetPath());
        }

        [TestMethod]
        public async Task TestGetSheetPathRootLeafHelpers()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sheets/get-root-sheet-path/all-response-body-properties", requestId.ToString());

            SheetPathNode result = smartsheet.SheetResources.GetSheetPath(CommonTestConstants.TEST_SHEET_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            PathLeaf? leaf = result.GetLeafSheet();
            Assert.IsNotNull(leaf);
            Assert.AreEqual(CommonTestConstants.TEST_PATH_ROOT_LEAF_ID, leaf.Id);
            Assert.AreEqual("Root Level Sheet", leaf.Name);
            Assert.AreEqual("/Sample Workspace/Root Level Sheet", result.GetLeafSheetPath());
        }

        [TestMethod]
        public void TestGetSheetPathError400Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", Guid.NewGuid().ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.SheetResources.GetSheetPath(CommonTestConstants.TEST_SHEET_ID));
            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }

        [TestMethod]
        public void TestGetSheetPathError500Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", Guid.NewGuid().ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.SheetResources.GetSheetPath(CommonTestConstants.TEST_SHEET_ID));
            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public async Task TestGetSheetPathAsyncGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sheets/get-nested-sheet-path/all-response-body-properties", requestId.ToString());

            await smartsheet.SheetResources.GetSheetPathAsync(CommonTestConstants.TEST_SHEET_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.IsNotNull(foundRequest);
            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual(string.Format("/2.0/sheets/{0}/path", CommonTestConstants.TEST_SHEET_ID), uri.AbsolutePath);
            Assert.AreEqual("GET", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestGetSheetPathAsyncNestedAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sheets/get-nested-sheet-path/all-response-body-properties", requestId.ToString());

            SheetPathNode result = await smartsheet.SheetResources.GetSheetPathAsync(CommonTestConstants.TEST_SHEET_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.AreEqual(string.Empty, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_NESTED_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetSheetPathAsyncRootAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sheets/get-root-sheet-path/all-response-body-properties", requestId.ToString());

            SheetPathNode result = await smartsheet.SheetResources.GetSheetPathAsync(CommonTestConstants.TEST_SHEET_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.AreEqual(string.Empty, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ROOT_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetSheetPathAsyncError400Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", Guid.NewGuid().ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.SheetResources.GetSheetPathAsync(CommonTestConstants.TEST_SHEET_ID),
                "Malformed Request");
        }

        [TestMethod]
        public async Task TestGetSheetPathAsyncError500Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", Guid.NewGuid().ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.SheetResources.GetSheetPathAsync(CommonTestConstants.TEST_SHEET_ID),
                "Internal Server Error");
        }
    }
}
