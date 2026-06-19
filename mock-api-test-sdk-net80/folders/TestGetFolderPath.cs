using Newtonsoft.Json;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestGetFolderPath
    {
        private static readonly FolderPathNode EXPECTED_NESTED_RESPONSE = new FolderPathNode
        {
            Id = CommonTestConstants.TEST_PATH_WORKSPACE_ID,
            Name = CommonTestConstants.TEST_PATH_WORKSPACE_NAME,
            Permalink = CommonTestConstants.TEST_PATH_WORKSPACE_PERMALINK,
            AccessLevel = AccessLevel.OWNER,
            Folders = new List<FolderPathNode>
            {
                new FolderPathNode
                {
                    Id = CommonTestConstants.TEST_PATH_FOLDER_1_ID,
                    Name = CommonTestConstants.TEST_PATH_FOLDER_1_NAME,
                    Permalink = CommonTestConstants.TEST_PATH_FOLDER_1_PERMALINK,
                    Folders = new List<FolderPathNode>
                    {
                        new FolderPathNode
                        {
                            Id = CommonTestConstants.TEST_PATH_FOLDER_2_ID,
                            Name = CommonTestConstants.TEST_PATH_FOLDER_2_NAME,
                            Permalink = CommonTestConstants.TEST_PATH_FOLDER_2_PERMALINK,
                            Folders = new List<FolderPathNode>
                            {
                                new FolderPathNode
                                {
                                    Id = CommonTestConstants.TEST_PATH_NESTED_LEAF_ID,
                                    Name = CommonTestConstants.TEST_PATH_NESTED_LEAF_NAME,
                                    Permalink = "https://app.smartsheet.com/folders/3456789012345678"
                                }
                            }
                        }
                    }
                }
            }
        };

        private static readonly FolderPathNode EXPECTED_ROOT_RESPONSE = new FolderPathNode
        {
            Id = CommonTestConstants.TEST_PATH_WORKSPACE_ID,
            Name = CommonTestConstants.TEST_PATH_WORKSPACE_NAME,
            Permalink = CommonTestConstants.TEST_PATH_WORKSPACE_PERMALINK,
            AccessLevel = AccessLevel.OWNER,
            Folders = new List<FolderPathNode>
            {
                new FolderPathNode
                {
                    Id = CommonTestConstants.TEST_PATH_ROOT_LEAF_ID,
                    Name = "Root Level Folder",
                    Permalink = "https://app.smartsheet.com/folders/rootlevel"
                }
            }
        };

        [TestMethod]
        public async Task TestGetFolderPathGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/folders/get-nested-folder-path/all-response-body-properties", requestId.ToString());

            smartsheet.FolderResources.GetFolderPath(CommonTestConstants.TEST_FOLDER_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.IsNotNull(foundRequest);
            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual(string.Format("/2.0/folders/{0}/path", CommonTestConstants.TEST_FOLDER_ID), uri.AbsolutePath);
            Assert.AreEqual("GET", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestGetFolderPathNestedAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/folders/get-nested-folder-path/all-response-body-properties", requestId.ToString());

            FolderPathNode result = smartsheet.FolderResources.GetFolderPath(CommonTestConstants.TEST_FOLDER_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.AreEqual(string.Empty, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_NESTED_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetFolderPathRootAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/folders/get-root-folder-path/all-response-body-properties", requestId.ToString());

            FolderPathNode result = smartsheet.FolderResources.GetFolderPath(CommonTestConstants.TEST_FOLDER_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.AreEqual(string.Empty, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ROOT_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetFolderPathNestedLeafHelpers()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/folders/get-nested-folder-path/all-response-body-properties", requestId.ToString());

            FolderPathNode result = smartsheet.FolderResources.GetFolderPath(CommonTestConstants.TEST_FOLDER_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            FolderPathNode leaf = result.GetLeafFolder();
            Assert.IsNotNull(leaf);
            Assert.AreEqual(CommonTestConstants.TEST_PATH_NESTED_LEAF_ID, leaf.Id);
            Assert.AreEqual(CommonTestConstants.TEST_PATH_NESTED_LEAF_NAME, leaf.Name);
            Assert.AreEqual("/Sample Workspace/Project Plans/Project Plans Subfolder/Project Plans Sub-Subfolder", result.GetLeafFolderPath());
        }

        [TestMethod]
        public async Task TestGetFolderPathRootLeafHelpers()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/folders/get-root-folder-path/all-response-body-properties", requestId.ToString());

            FolderPathNode result = smartsheet.FolderResources.GetFolderPath(CommonTestConstants.TEST_FOLDER_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            FolderPathNode leaf = result.GetLeafFolder();
            Assert.IsNotNull(leaf);
            Assert.AreEqual(CommonTestConstants.TEST_PATH_ROOT_LEAF_ID, leaf.Id);
            Assert.AreEqual("Root Level Folder", leaf.Name);
            Assert.AreEqual("/Sample Workspace/Root Level Folder", result.GetLeafFolderPath());
        }

        [TestMethod]
        public void TestGetFolderPathError400Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", Guid.NewGuid().ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.FolderResources.GetFolderPath(CommonTestConstants.TEST_FOLDER_ID));
            Assert.AreEqual("Malformed Request", exception.Message);
        }

        [TestMethod]
        public void TestGetFolderPathError404Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/404-response", Guid.NewGuid().ToString());

            ResourceNotFoundException exception = Assert.ThrowsException<ResourceNotFoundException>(() =>
                smartsheet.FolderResources.GetFolderPath(CommonTestConstants.TEST_FOLDER_ID));
            Assert.AreEqual("Not Found", exception.Message);
        }

        [TestMethod]
        public void TestGetFolderPathError500Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", Guid.NewGuid().ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.FolderResources.GetFolderPath(CommonTestConstants.TEST_FOLDER_ID));
            Assert.AreEqual("Internal Server Error", exception.Message);
        }

        [TestMethod]
        public async Task TestGetFolderPathAsyncGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/folders/get-nested-folder-path/all-response-body-properties", requestId.ToString());

            await smartsheet.FolderResources.GetFolderPathAsync(CommonTestConstants.TEST_FOLDER_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.IsNotNull(foundRequest);
            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual(string.Format("/2.0/folders/{0}/path", CommonTestConstants.TEST_FOLDER_ID), uri.AbsolutePath);
            Assert.AreEqual("GET", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestGetFolderPathAsyncNestedAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/folders/get-nested-folder-path/all-response-body-properties", requestId.ToString());

            FolderPathNode result = await smartsheet.FolderResources.GetFolderPathAsync(CommonTestConstants.TEST_FOLDER_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.AreEqual(string.Empty, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_NESTED_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetFolderPathAsyncRootAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/folders/get-root-folder-path/all-response-body-properties", requestId.ToString());

            FolderPathNode result = await smartsheet.FolderResources.GetFolderPathAsync(CommonTestConstants.TEST_FOLDER_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.AreEqual(string.Empty, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ROOT_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetFolderPathAsyncError400Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", Guid.NewGuid().ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.FolderResources.GetFolderPathAsync(CommonTestConstants.TEST_FOLDER_ID),
                "Malformed Request");
        }

        [TestMethod]
        public async Task TestGetFolderPathAsyncError404Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/404-response", Guid.NewGuid().ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<ResourceNotFoundException>(
                () => smartsheet.FolderResources.GetFolderPathAsync(CommonTestConstants.TEST_FOLDER_ID),
                "Not Found");
        }

        [TestMethod]
        public async Task TestGetFolderPathAsyncError500Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", Guid.NewGuid().ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.FolderResources.GetFolderPathAsync(CommonTestConstants.TEST_FOLDER_ID),
                "Internal Server Error");
        }
    }
}
