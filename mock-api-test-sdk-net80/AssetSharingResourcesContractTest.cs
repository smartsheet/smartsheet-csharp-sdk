using System.Web;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class AssetSharingResourcesContractTest
    {
        // Test constants matching Python SDK
        private const string TEST_ASSET_ID = "AAAMCmYGFOeE";
        private const string TEST_SHARE_ID = "AAABbbbCccDdd";
        private const string TEST_EMAIL = "test.email@smartsheet.com";
        private const string TEST_USER_ID = "9876543210";
        private const string TEST_GROUP_ID = "1234567890";
        private const string TEST_NAME = "Example Name";
        private const int TEST_MAX_ITEMS = 100;
        private const string TEST_LAST_KEY = "test_last_key";
        private const string TEST_LAST_KEY_RESPONSE = "abcDefGhIjKlMnOpQrStUvWxYz";

        [TestMethod]
        public async Task TestListAssetSharesGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/sharing/list-asset-shares/all-response-body-properties", requestId.ToString());

            AssetType assetType = AssetType.SHEET;
            ShareScope sharingInclude = ShareScope.Item;

            TokenPaginationParameters pagination = new TokenPaginationParameters(TEST_LAST_KEY, TEST_MAX_ITEMS);

            ss.AssetSharingResources.ListAssetShares(assetType, TEST_ASSET_ID, pagination, sharingInclude);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual("/2.0/shares", path);
            Assert.AreEqual(assetType.ToString().ToLower(), queryParams["assetType"]);
            Assert.AreEqual(TEST_ASSET_ID, queryParams["assetId"]);
            Assert.AreEqual(TEST_MAX_ITEMS.ToString(), queryParams["maxItems"]);
            Assert.AreEqual(TEST_LAST_KEY, queryParams["lastKey"]);
            Assert.AreEqual(sharingInclude.ToString(), queryParams["sharingInclude"]);
        }

        [TestMethod]
        public void TestListAssetSharesAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/sharing/list-asset-shares/all-response-body-properties", requestId.ToString());

            AssetType assetType = AssetType.SHEET;

            ListAssetSharesResponse response = ss.AssetSharingResources.ListAssetShares(assetType, TEST_ASSET_ID, null, null);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.LastKey);
            Assert.AreEqual(TEST_LAST_KEY_RESPONSE, response.LastKey);

            Assert.IsNotNull(response.Items);
            Assert.IsTrue(response.Items.Count > 0);
            Assert.AreEqual(TEST_ASSET_ID, response.Items[0].Id);
            Assert.AreEqual(TEST_EMAIL, response.Items[0].Email);
            Assert.AreEqual(AccessLevel.ADMIN, response.Items[0].AccessLevel);
            Assert.AreEqual(AssetShareScope.ITEM, response.Items[0].Scope);
            Assert.AreEqual(ShareType.USER, response.Items[0].Type);
            Assert.AreEqual(TEST_USER_ID, response.Items[0].UserId);
            Assert.AreEqual(TEST_GROUP_ID, response.Items[0].GroupId);
            Assert.AreEqual(TEST_NAME, response.Items[0].Name);
        }

        [TestMethod]
        public void TestListAssetSharesRequiredResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/sharing/list-asset-shares/required-response-body-properties", requestId.ToString());

            AssetType assetType = AssetType.SHEET;

            ListAssetSharesResponse response = ss.AssetSharingResources.ListAssetShares(assetType, TEST_ASSET_ID, null, null);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Items);
            Assert.IsTrue(response.Items.Count > 0);
            Assert.AreEqual(TEST_ASSET_ID, response.Items[0].Id);
            Assert.AreEqual(AccessLevel.ADMIN, response.Items[0].AccessLevel);
            Assert.IsNull(response.Items[0].Name);
        }

        [TestMethod]
        public void TestListAssetSharesError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/errors/500-response",requestId.ToString());

            AssetType assetType = AssetType.SHEET;

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.ListAssetShares(assetType, TEST_ASSET_ID, null, null));
            Assert.AreEqual("Internal Server Error", exception.Message);
        }

        [TestMethod]
        public void TestListAssetSharesError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            AssetType assetType = AssetType.SHEET;

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.ListAssetShares(assetType, TEST_ASSET_ID, null, null));
            Assert.AreEqual("Malformed Request", exception.Message);
        }

        [TestMethod]
        public async Task TestGetAssetShareGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/sharing/get-asset-share/all-response-body-properties", requestId.ToString());

            AssetType assetType = AssetType.SHEET;
            string shareId = TEST_SHARE_ID;

            ss.AssetSharingResources.GetAssetShare(assetType, TEST_ASSET_ID, shareId);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual($"/2.0/shares/{shareId}", path);
            Assert.AreEqual(assetType.ToString().ToLower(), queryParams["assetType"]);
            Assert.AreEqual(TEST_ASSET_ID, queryParams["assetId"]);
        }

        [TestMethod]
        public void TestGetAssetShareAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/sharing/get-asset-share/all-response-body-properties", requestId.ToString());

            AssetType assetType = AssetType.SHEET;
            string shareId = TEST_SHARE_ID;

            AssetShare response = ss.AssetSharingResources.GetAssetShare(assetType, TEST_ASSET_ID, shareId);

            Assert.IsNotNull(response);
            Assert.AreEqual(TEST_ASSET_ID, response.Id);
            Assert.AreEqual(TEST_EMAIL, response.Email);
            Assert.AreEqual(AccessLevel.ADMIN, response.AccessLevel);
            Assert.AreEqual(AssetShareScope.ITEM, response.Scope);
            Assert.AreEqual(ShareType.USER, response.Type);
            Assert.AreEqual(TEST_USER_ID, response.UserId);
            Assert.AreEqual(TEST_GROUP_ID, response.GroupId);
            Assert.AreEqual(TEST_NAME, response.Name);
        }

        [TestMethod]
        public void TestGetAssetShareRequiredResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/sharing/get-asset-share/required-response-body-properties", requestId.ToString());

            AssetType assetType = AssetType.SHEET;
            string shareId = TEST_SHARE_ID;

            AssetShare response = ss.AssetSharingResources.GetAssetShare(assetType, TEST_ASSET_ID, shareId);

            Assert.IsNotNull(response);
            Assert.AreEqual(TEST_ASSET_ID, response.Id);
            Assert.AreEqual(AccessLevel.ADMIN, response.AccessLevel);
            Assert.IsNull(response.Name);
        }

        [TestMethod]
        public void TestGetAssetShareError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            AssetType assetType = AssetType.SHEET;
            string shareId = TEST_SHARE_ID;

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.GetAssetShare(assetType, TEST_ASSET_ID, shareId));
            Assert.AreEqual("Internal Server Error", exception.Message);
        }

        [TestMethod]
        public void TestGetAssetShareError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            AssetType assetType = AssetType.SHEET;
            string shareId = TEST_SHARE_ID;

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.GetAssetShare(assetType, TEST_ASSET_ID, shareId));
            Assert.AreEqual("Malformed Request", exception.Message);
        }

        [TestMethod]
        public async Task TestShareAssetGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/sharing/share-asset/all-response-body-properties", requestId.ToString());

            AssetType assetType = AssetType.SHEET;
            bool sendEmail = false;

            AssetShare newShare = new AssetShare.CreateAssetShareBuilder(AccessLevel.ADMIN)
                .SetEmail(TEST_EMAIL)
                .Build();

            ss.AssetSharingResources.ShareAsset(assetType, TEST_ASSET_ID, new List<AssetShare> { newShare }, sendEmail);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual("/2.0/shares", path);
            Assert.AreEqual(assetType.ToString().ToLower(), queryParams["assetType"]);
            Assert.AreEqual(TEST_ASSET_ID, queryParams["assetId"]);
            Assert.AreEqual(sendEmail.ToString().ToLower(), queryParams["sendEmail"]);
        }

        [TestMethod]
        public async void TestShareAssetAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/sharing/share-asset/all-response-body-properties", requestId.ToString());

            AssetType assetType = AssetType.SHEET;

            AssetShare newShare = new AssetShare.CreateAssetShareBuilder(AccessLevel.ADMIN)
                .SetEmail(TEST_EMAIL)
                .Build();

            BulkItemResult<AssetShare> response = ss.AssetSharingResources.ShareAsset(assetType, TEST_ASSET_ID, new List<AssetShare> { newShare }, false);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.IsNotNull(foundRequest.Body);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, response.Result.Count);
            Assert.AreEqual(TEST_ASSET_ID, response.Result[0].Id);
            Assert.AreEqual(TEST_EMAIL, response.Result[0].Email);
            Assert.AreEqual(AccessLevel.ADMIN, response.Result[0].AccessLevel);
            Assert.AreEqual(ShareScope.Item.ToString(), response.Result[0].Scope);
            Assert.AreEqual(ShareType.USER, response.Result[0].Type);
            Assert.AreEqual(TEST_NAME, response.Result[0].Name);
        }

        [TestMethod]
        public async void TestShareAssetRequiredResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/sharing/share-asset/required-response-body-properties", requestId.ToString());

            AssetType assetType = AssetType.SHEET;

            AssetShare newShare = new AssetShare.CreateAssetShareBuilder(AccessLevel.ADMIN)
                .SetEmail(TEST_EMAIL)
                .Build();

            BulkItemResult<AssetShare> response = ss.AssetSharingResources.ShareAsset(assetType, TEST_ASSET_ID, new List<AssetShare> { newShare }, false);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.IsNotNull(foundRequest.Body);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, response.Result.Count);
            Assert.AreEqual(TEST_ASSET_ID, response.Result[0].Id);
            Assert.AreEqual(AccessLevel.ADMIN, response.Result[0].AccessLevel);
            Assert.IsNull(response.Result[0].Name);
        }

        [TestMethod]
        public void TestShareAssetError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            AssetType assetType = AssetType.SHEET;

            AssetShare newShare = new AssetShare.CreateAssetShareBuilder(AccessLevel.ADMIN)
                .SetEmail(TEST_EMAIL)
                .Build();

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.ShareAsset(assetType, TEST_ASSET_ID, new List<AssetShare> { newShare }, false));
            Assert.AreEqual("Internal Server Error", exception.Message);
        }

        [TestMethod]
        public void TestShareAssetError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            AssetType assetType = AssetType.SHEET;

            AssetShare newShare = new AssetShare.CreateAssetShareBuilder(AccessLevel.ADMIN)
                .SetEmail("invalid-email")
                .Build();

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.ShareAsset(assetType, TEST_ASSET_ID, new List<AssetShare> { newShare }, false));
            Assert.AreEqual("Malformed Request", exception.Message);
        }

        [TestMethod]
        public async Task TestUpdateAssetShareGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/sharing/update-asset-share/all-response-body-properties", requestId.ToString());

            AssetType assetType = AssetType.SHEET;
            string shareId = TEST_SHARE_ID;

            UpdateShareRequest updateRequest = new UpdateShareRequest
            {
                AccessLevel = AccessLevel.ADMIN
            };

            ss.AssetSharingResources.UpdateAssetShare(assetType, TEST_ASSET_ID, shareId, updateRequest);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual($"/2.0/shares/{shareId}", path);
            Assert.AreEqual(assetType.ToString().ToLower(), queryParams["assetType"]);
            Assert.AreEqual(TEST_ASSET_ID, queryParams["assetId"]);
        }

        [TestMethod]
        public void TestUpdateAssetShareAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/sharing/update-asset-share/all-response-body-properties", requestId.ToString());

            AssetType assetType = AssetType.SHEET;
            string shareId = TEST_SHARE_ID;

            UpdateShareRequest updateRequest = new UpdateShareRequest
            {
                AccessLevel = AccessLevel.ADMIN
            };

            AssetShare response = ss.AssetSharingResources.UpdateAssetShare(assetType, TEST_ASSET_ID, shareId, updateRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(TEST_ASSET_ID, response.Id);
            Assert.AreEqual(AccessLevel.ADMIN, response.AccessLevel);
            Assert.AreEqual(TEST_EMAIL, response.Email);
            Assert.AreEqual(AssetShareScope.ITEM, response.Scope);
            Assert.AreEqual(TEST_NAME, response.Name);
        }

        [TestMethod]
        public void TestUpdateAssetShareRequiredResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/sharing/update-asset-share/required-response-body-properties", requestId.ToString());

            AssetType assetType = AssetType.SHEET;
            string shareId = TEST_SHARE_ID;

            UpdateShareRequest updateRequest = new UpdateShareRequest
            {
                AccessLevel = AccessLevel.ADMIN
            };

            AssetShare response = ss.AssetSharingResources.UpdateAssetShare(assetType, TEST_ASSET_ID, shareId, updateRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(TEST_ASSET_ID, response.Id);
            Assert.AreEqual(AccessLevel.ADMIN, response.AccessLevel);
            Assert.IsNull(response.Name);
        }

        [TestMethod]
        public void TestUpdateAssetShareError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            AssetType assetType = AssetType.SHEET;
            string shareId = TEST_SHARE_ID;

            UpdateShareRequest updateRequest = new UpdateShareRequest
            {
                AccessLevel = AccessLevel.ADMIN
            };

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.UpdateAssetShare(assetType, TEST_ASSET_ID, shareId, updateRequest));
            Assert.AreEqual("Internal Server Error", exception.Message);
        }

        [TestMethod]
        public void TestUpdateAssetShareError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            AssetType assetType = AssetType.SHEET;
            string shareId = TEST_SHARE_ID;

            UpdateShareRequest updateRequest = new UpdateShareRequest
            {
                AccessLevel = AccessLevel.ADMIN
            };

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.UpdateAssetShare(assetType, TEST_ASSET_ID, shareId, updateRequest));
            Assert.AreEqual("Malformed Request", exception.Message);
        }

        [TestMethod]
        public async Task TestDeleteAssetShareGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/sharing/delete-asset-share/all-response-body-properties", requestId.ToString());

            AssetType assetType = AssetType.SHEET;
            string shareId = TEST_SHARE_ID;

            ss.AssetSharingResources.DeleteAssetShare(assetType, TEST_ASSET_ID, shareId);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual($"/2.0/shares/{shareId}", path);
            Assert.AreEqual(assetType.ToString().ToLower(), queryParams["assetType"]);
            Assert.AreEqual(TEST_ASSET_ID, queryParams["assetId"]);
        }

        [TestMethod]
        public void TestDeleteAssetShareSuccess()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/sharing/delete-asset-share/all-response-body-properties", requestId.ToString());

            AssetType assetType = AssetType.SHEET;
            string shareId = TEST_SHARE_ID;

            // Should not throw any exception
            ss.AssetSharingResources.DeleteAssetShare(assetType, TEST_ASSET_ID, shareId);
        }

        [TestMethod]
        public void TestDeleteAssetShareError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            AssetType assetType = AssetType.SHEET;
            string shareId = TEST_SHARE_ID;

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.DeleteAssetShare(assetType, TEST_ASSET_ID, shareId));
            Assert.AreEqual("Internal Server Error", exception.Message);
        }

        [TestMethod]
        public void TestDeleteAssetShareError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            AssetType assetType = AssetType.SHEET;
            string shareId = TEST_SHARE_ID;

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.AssetSharingResources.DeleteAssetShare(assetType, TEST_ASSET_ID, shareId));
            Assert.AreEqual("Malformed Request", exception.Message);
        }
    }
}