using System.Web;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class AssetSharingResourcesContractTest
    {
        [TestMethod]
        public async Task TestListAssetSharesGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/shares/list-asset-shares/all-response-body-properties" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;
            int maxItems = 100;
            string lastKey = "abcDefGhIjKlMnOpQrStUvWxYz";
            ShareScope sharingInclude = ShareScope.Workspace;

            TokenPaginationParameters pagination = new TokenPaginationParameters(lastKey, maxItems);

            ss.AssetSharingResources.ListAssetShares(assetType, assetId, pagination, sharingInclude);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual("/2.0/shares", path);
            Assert.AreEqual(assetType.ToString(), queryParams["assetType"]);
            Assert.AreEqual(assetId.ToString(), queryParams["assetId"]);
            Assert.AreEqual(maxItems.ToString(), queryParams["maxItems"]);
            Assert.AreEqual(lastKey, queryParams["lastKey"]);
            Assert.AreEqual(sharingInclude.ToString(), queryParams["include"]);
        }

        [TestMethod]
        public void TestListAssetSharesAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/shares/list-asset-shares/all-response-body-properties" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;

            ListAssetSharesResponse response = ss.AssetSharingResources.ListAssetShares(assetType, assetId, null, null);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Items);
            Assert.IsTrue(response.Items.Count > 0);
            Assert.AreEqual("AAABbbbCCCdddd", response.Items[0].Id);
            Assert.AreEqual("user@example.com", response.Items[0].Email);
            Assert.AreEqual(AccessLevel.VIEWER, response.Items[0].AccessLevel);
            Assert.AreEqual(ShareScope.Item, response.Items[0].Scope);
            Assert.AreEqual(ShareType.USER, response.Items[0].Type);
            Assert.AreEqual("1234567890", response.Items[0].UserId);
        }

        [TestMethod]
        public void TestListAssetSharesRequiredResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/shares/list-asset-shares/required-response-body-properties" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;

            ListAssetSharesResponse response = ss.AssetSharingResources.ListAssetShares(assetType, assetId, null, null);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Items);
            Assert.IsTrue(response.Items.Count > 0);
            Assert.AreEqual("AAABbbbCCCdddd", response.Items[0].Id);
            Assert.AreEqual(AccessLevel.VIEWER, response.Items[0].AccessLevel);
        }

        [TestMethod]
        public void TestListAssetSharesError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/errors/500-response" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.ListAssetShares(assetType, assetId, null, null));
            Assert.AreEqual("Internal Server Error", exception.Message);
        }

        [TestMethod]
        public void TestListAssetSharesError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/errors/400-response" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.ListAssetShares(assetType, assetId, null, null));
            Assert.AreEqual("Malformed Request", exception.Message);
        }

        [TestMethod]
        public async Task TestGetAssetShareGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/shares/get-asset-share/all-response-body-properties" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;
            string shareId = "AAABbbbCCCdddd";

            ss.AssetSharingResources.GetAssetShare(assetType, assetId, shareId);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual($"/2.0/shares/{shareId}", path);
            Assert.AreEqual(assetType.ToString(), queryParams["assetType"]);
            Assert.AreEqual(assetId.ToString(), queryParams["assetId"]);
        }

        [TestMethod]
        public void TestGetAssetShareAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/shares/get-asset-share/all-response-body-properties" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;
            string shareId = "AAABbbbCCCdddd";

            AssetShare response = ss.AssetSharingResources.GetAssetShare(assetType, assetId, shareId);

            Assert.IsNotNull(response);
            Assert.AreEqual("AAABbbbCCCdddd", response.Id);
            Assert.AreEqual("user@example.com", response.Email);
            Assert.AreEqual(AccessLevel.VIEWER, response.AccessLevel);
            Assert.AreEqual(ShareScope.Item, response.Scope);
            Assert.AreEqual(ShareType.USER, response.Type);
            Assert.AreEqual("1234567890", response.UserId);
            Assert.AreEqual("Test User", response.Name);
        }

        [TestMethod]
        public void TestGetAssetShareRequiredResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/shares/get-asset-share/required-response-body-properties" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;
            string shareId = "AAABbbbCCCdddd";

            AssetShare response = ss.AssetSharingResources.GetAssetShare(assetType, assetId, shareId);

            Assert.IsNotNull(response);
            Assert.AreEqual("AAABbbbCCCdddd", response.Id);
            Assert.AreEqual(AccessLevel.VIEWER, response.AccessLevel);
        }

        [TestMethod]
        public void TestGetAssetShareError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/errors/500-response" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;
            string shareId = "AAABbbbCCCdddd";

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.GetAssetShare(assetType, assetId, shareId));
            Assert.AreEqual("Internal Server Error", exception.Message);
        }

        [TestMethod]
        public void TestGetAssetShareError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/errors/400-response" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;
            string shareId = "AAABbbbCCCdddd";

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.GetAssetShare(assetType, assetId, shareId));
            Assert.AreEqual("Malformed Request", exception.Message);
        }

        [TestMethod]
        public async Task TestShareAssetGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/shares/share-asset/all-response-body-properties" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;
            bool sendEmail = true;

            AssetShare newShare = new AssetShare.CreateAssetShareBuilder(AccessLevel.VIEWER)
                .SetEmail("user@example.com")
                .SetSubject("Check out this sheet")
                .SetMessage("I thought you might find this interesting")
                .SetCcMe(true)
                .Build();

            ss.AssetSharingResources.ShareAsset(assetType, assetId, new List<AssetShare> { newShare }, sendEmail);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual("/2.0/shares", path);
            Assert.AreEqual(assetType.ToString(), queryParams["assetType"]);
            Assert.AreEqual(assetId.ToString(), queryParams["assetId"]);
            Assert.AreEqual(sendEmail.ToString().ToLower(), queryParams["sendEmail"]);
        }

        [TestMethod]
        public void TestShareAssetAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/shares/share-asset/all-response-body-properties" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;

            AssetShare newShare = new AssetShare.CreateAssetShareBuilder(AccessLevel.VIEWER)
                .SetEmail("user@example.com")
                .SetSubject("Check out this sheet")
                .SetMessage("I thought you might find this interesting")
                .SetCcMe(true)
                .Build();

            BulkItemResult<AssetShare> response = ss.AssetSharingResources.ShareAsset(assetType, assetId, new List<AssetShare> { newShare }, true);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, response.Result.Count);
            Assert.AreEqual("AAABbbbCCCdddd", response.Result[0].Id);
            Assert.AreEqual("user@example.com", response.Result[0].Email);
            Assert.AreEqual(AccessLevel.VIEWER, response.Result[0].AccessLevel);
            Assert.AreEqual(ShareScope.Item, response.Result[0].Scope);
            Assert.AreEqual(ShareType.USER, response.Result[0].Type);
        }

        [TestMethod]
        public void TestShareAssetRequiredResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/shares/share-asset/required-response-body-properties" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;

            AssetShare newShare = new AssetShare.CreateAssetShareBuilder(AccessLevel.VIEWER)
                .SetEmail("user@example.com")
                .Build();

            BulkItemResult<AssetShare> response = ss.AssetSharingResources.ShareAsset(assetType, assetId, new List<AssetShare> { newShare }, null);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, response.Result.Count);
            Assert.AreEqual("AAABbbbCCCdddd", response.Result[0].Id);
            Assert.AreEqual(AccessLevel.VIEWER, response.Result[0].AccessLevel);
        }

        [TestMethod]
        public void TestShareAssetError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/errors/500-response" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;

            AssetShare newShare = new AssetShare.CreateAssetShareBuilder(AccessLevel.VIEWER)
                .SetEmail("user@example.com")
                .Build();

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.ShareAsset(assetType, assetId, new List<AssetShare> { newShare }, null));
            Assert.AreEqual("Internal Server Error", exception.Message);
        }

        [TestMethod]
        public void TestShareAssetError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/errors/400-response" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;

            AssetShare newShare = new AssetShare.CreateAssetShareBuilder(AccessLevel.VIEWER)
                .SetEmail("invalid-email")
                .Build();

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.ShareAsset(assetType, assetId, new List<AssetShare> { newShare }, null));
            Assert.AreEqual("Malformed Request", exception.Message);
        }

        [TestMethod]
        public async Task TestUpdateAssetShareGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/shares/update-asset-share/all-response-body-properties" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;
            string shareId = "AAABbbbCCCdddd";

            UpdateShareRequest updateRequest = new UpdateShareRequest
            {
                AccessLevel = AccessLevel.EDITOR
            };

            ss.AssetSharingResources.UpdateAssetShare(assetType, assetId, shareId, updateRequest);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual($"/2.0/shares/{shareId}", path);
            Assert.AreEqual(assetType.ToString(), queryParams["assetType"]);
            Assert.AreEqual(assetId.ToString(), queryParams["assetId"]);
        }

        [TestMethod]
        public void TestUpdateAssetShareAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/shares/update-asset-share/all-response-body-properties" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;
            string shareId = "AAABbbbCCCdddd";

            UpdateShareRequest updateRequest = new UpdateShareRequest
            {
                AccessLevel = AccessLevel.EDITOR
            };

            AssetShare response = ss.AssetSharingResources.UpdateAssetShare(assetType, assetId, shareId, updateRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual("AAABbbbCCCdddd", response.Id);
            Assert.AreEqual(AccessLevel.EDITOR, response.AccessLevel);
            Assert.AreEqual("user@example.com", response.Email);
            Assert.AreEqual(ShareScope.Item, response.Scope);
        }

        [TestMethod]
        public void TestUpdateAssetShareRequiredResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/shares/update-asset-share/required-response-body-properties" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;
            string shareId = "AAABbbbCCCdddd";

            UpdateShareRequest updateRequest = new UpdateShareRequest
            {
                AccessLevel = AccessLevel.EDITOR
            };

            AssetShare response = ss.AssetSharingResources.UpdateAssetShare(assetType, assetId, shareId, updateRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual("AAABbbbCCCdddd", response.Id);
            Assert.AreEqual(AccessLevel.EDITOR, response.AccessLevel);
        }

        [TestMethod]
        public void TestUpdateAssetShareError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/errors/500-response" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;
            string shareId = "AAABbbbCCCdddd";

            UpdateShareRequest updateRequest = new UpdateShareRequest
            {
                AccessLevel = AccessLevel.EDITOR
            };

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.UpdateAssetShare(assetType, assetId, shareId, updateRequest));
            Assert.AreEqual("Internal Server Error", exception.Message);
        }

        [TestMethod]
        public void TestUpdateAssetShareError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/errors/400-response" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;
            string shareId = "AAABbbbCCCdddd";

            UpdateShareRequest updateRequest = new UpdateShareRequest
            {
                AccessLevel = AccessLevel.EDITOR
            };

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.UpdateAssetShare(assetType, assetId, shareId, updateRequest));
            Assert.AreEqual("Malformed Request", exception.Message);
        }

        [TestMethod]
        public async Task TestDeleteAssetShareGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/shares/delete-asset-share/success" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;
            string shareId = "AAABbbbCCCdddd";

            ss.AssetSharingResources.DeleteAssetShare(assetType, assetId, shareId);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual($"/2.0/shares/{shareId}", path);
            Assert.AreEqual(assetType.ToString(), queryParams["assetType"]);
            Assert.AreEqual(assetId.ToString(), queryParams["assetId"]);
        }

        [TestMethod]
        public void TestDeleteAssetShareSuccess()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/shares/delete-asset-share/success" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;
            string shareId = "AAABbbbCCCdddd";

            // Should not throw any exception
            ss.AssetSharingResources.DeleteAssetShare(assetType, assetId, shareId);
        }

        [TestMethod]
        public void TestDeleteAssetShareError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/errors/500-response" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;
            string shareId = "AAABbbbCCCdddd";

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.DeleteAssetShare(assetType, assetId, shareId));
            Assert.AreEqual("Internal Server Error", exception.Message);
        }

        [TestMethod]
        public void TestDeleteAssetShareError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> { { "x-test-name", "/errors/400-response" }, { "x-request-id", requestId.ToString() } });

            AssetType assetType = AssetType.SHEET;
            long assetId = 1234567890123456L;
            string shareId = "AAABbbbCCCdddd";

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.DeleteAssetShare(assetType, assetId, shareId));
            Assert.AreEqual("Malformed Request", exception.Message);
        }
    }
}