using Newtonsoft.Json;
using Smartsheet.Api;
using Smartsheet.Api.Models;
using System.Globalization;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestGetSightPath
    {
        private static readonly SightPathNode EXPECTED_NESTED_RESPONSE = new SightPathNode
        {
            Id = CommonTestConstants.TEST_PATH_WORKSPACE_ID,
            Name = CommonTestConstants.TEST_PATH_WORKSPACE_NAME,
            Permalink = CommonTestConstants.TEST_PATH_WORKSPACE_PERMALINK,
            AccessLevel = AccessLevel.OWNER,
            Folders = new List<SightPathNode>
            {
                new SightPathNode
                {
                    Id = CommonTestConstants.TEST_PATH_FOLDER_1_ID,
                    Name = CommonTestConstants.TEST_PATH_FOLDER_1_NAME,
                    Permalink = CommonTestConstants.TEST_PATH_FOLDER_1_PERMALINK,
                    Folders = new List<SightPathNode>
                    {
                        new SightPathNode
                        {
                            Id = CommonTestConstants.TEST_PATH_FOLDER_2_ID,
                            Name = CommonTestConstants.TEST_PATH_FOLDER_2_NAME,
                            Permalink = CommonTestConstants.TEST_PATH_FOLDER_2_PERMALINK,
                            Sights = new List<PathLeaf>
                            {
                                new PathLeaf
                                {
                                    Id = CommonTestConstants.TEST_PATH_NESTED_LEAF_ID,
                                    Name = "Project Dashboard",
                                    Permalink = "https://app.smartsheet.com/dashboards/3456789012345678",
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

        private static readonly SightPathNode EXPECTED_ROOT_RESPONSE = new SightPathNode
        {
            Id = CommonTestConstants.TEST_PATH_WORKSPACE_ID,
            Name = CommonTestConstants.TEST_PATH_WORKSPACE_NAME,
            Permalink = CommonTestConstants.TEST_PATH_WORKSPACE_PERMALINK,
            AccessLevel = AccessLevel.OWNER,
            Sights = new List<PathLeaf>
            {
                new PathLeaf
                {
                    Id = CommonTestConstants.TEST_PATH_ROOT_LEAF_ID,
                    Name = "Root Level Dashboard",
                    Permalink = "https://app.smartsheet.com/dashboards/rootlevel",
                    AccessLevel = AccessLevel.ADMIN,
                    CreatedAt = DateTime.Parse("2024-01-01T00:00:00Z", null, DateTimeStyles.RoundtripKind),
                    ModifiedAt = DateTime.Parse("2024-06-01T00:00:00Z", null, DateTimeStyles.RoundtripKind)
                }
            }
        };

        [TestMethod]
        public async Task TestGetSightPathGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sights/get-nested-sight-path/all-response-body-properties", requestId.ToString());

            smartsheet.SightResources.GetSightPath(CommonTestConstants.TEST_SIGHT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.IsNotNull(foundRequest);
            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual(string.Format("/2.0/sights/{0}/path", CommonTestConstants.TEST_SIGHT_ID), uri.AbsolutePath);
            Assert.AreEqual("GET", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestGetSightPathNestedAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sights/get-nested-sight-path/all-response-body-properties", requestId.ToString());

            SightPathNode result = smartsheet.SightResources.GetSightPath(CommonTestConstants.TEST_SIGHT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.AreEqual(string.Empty, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_NESTED_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetSightPathRootAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sights/get-root-sight-path/all-response-body-properties", requestId.ToString());

            SightPathNode result = smartsheet.SightResources.GetSightPath(CommonTestConstants.TEST_SIGHT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.AreEqual(string.Empty, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ROOT_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetSightPathNestedLeafHelpers()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sights/get-nested-sight-path/all-response-body-properties", requestId.ToString());

            SightPathNode result = smartsheet.SightResources.GetSightPath(CommonTestConstants.TEST_SIGHT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            PathLeaf? leaf = result.GetLeafSight();
            Assert.IsNotNull(leaf);
            Assert.AreEqual(CommonTestConstants.TEST_PATH_NESTED_LEAF_ID, leaf.Id);
            Assert.AreEqual("Project Dashboard", leaf.Name);
            Assert.AreEqual("/Sample Workspace/Project Plans/Project Plans Subfolder/Project Dashboard", result.GetLeafSightPath());
        }

        [TestMethod]
        public async Task TestGetSightPathRootLeafHelpers()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sights/get-root-sight-path/all-response-body-properties", requestId.ToString());

            SightPathNode result = smartsheet.SightResources.GetSightPath(CommonTestConstants.TEST_SIGHT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            PathLeaf? leaf = result.GetLeafSight();
            Assert.IsNotNull(leaf);
            Assert.AreEqual(CommonTestConstants.TEST_PATH_ROOT_LEAF_ID, leaf.Id);
            Assert.AreEqual("Root Level Dashboard", leaf.Name);
            Assert.AreEqual("/Sample Workspace/Root Level Dashboard", result.GetLeafSightPath());
        }

        [TestMethod]
        public void TestGetSightPathError400Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", Guid.NewGuid().ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.SightResources.GetSightPath(CommonTestConstants.TEST_SIGHT_ID));
            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }

        [TestMethod]
        public void TestGetSightPathError404Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/404-response", Guid.NewGuid().ToString());

            ResourceNotFoundException exception = Assert.ThrowsException<ResourceNotFoundException>(() =>
                smartsheet.SightResources.GetSightPath(CommonTestConstants.TEST_SIGHT_ID));
            Assert.IsTrue(exception.Message.Contains("Not Found"));
        }

        [TestMethod]
        public void TestGetSightPathError500Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", Guid.NewGuid().ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.SightResources.GetSightPath(CommonTestConstants.TEST_SIGHT_ID));
            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public async Task TestGetSightPathAsyncGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sights/get-nested-sight-path/all-response-body-properties", requestId.ToString());

            await smartsheet.SightResources.GetSightPathAsync(CommonTestConstants.TEST_SIGHT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.IsNotNull(foundRequest);
            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual(string.Format("/2.0/sights/{0}/path", CommonTestConstants.TEST_SIGHT_ID), uri.AbsolutePath);
            Assert.AreEqual("GET", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestGetSightPathAsyncNestedAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sights/get-nested-sight-path/all-response-body-properties", requestId.ToString());

            SightPathNode result = await smartsheet.SightResources.GetSightPathAsync(CommonTestConstants.TEST_SIGHT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.AreEqual(string.Empty, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_NESTED_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetSightPathAsyncRootAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sights/get-root-sight-path/all-response-body-properties", requestId.ToString());

            SightPathNode result = await smartsheet.SightResources.GetSightPathAsync(CommonTestConstants.TEST_SIGHT_ID);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.AreEqual(string.Empty, foundRequest.Body);
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ROOT_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestGetSightPathAsyncError400Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", Guid.NewGuid().ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.SightResources.GetSightPathAsync(CommonTestConstants.TEST_SIGHT_ID),
                "Malformed Request");
        }

        [TestMethod]
        public async Task TestGetSightPathAsyncError404Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/404-response", Guid.NewGuid().ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.SightResources.GetSightPathAsync(CommonTestConstants.TEST_SIGHT_ID),
                "Not Found");
        }

        [TestMethod]
        public async Task TestGetSightPathAsyncError500Response()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", Guid.NewGuid().ToString());

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.SightResources.GetSightPathAsync(CommonTestConstants.TEST_SIGHT_ID),
                "Internal Server Error");
        }
    }
}
