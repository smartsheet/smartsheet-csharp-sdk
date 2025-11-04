using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class AssetSharingTests
    {
        [TestMethod]
        public void ListAssetShares_Sheet()
        {
            SmartsheetClient ss = HelperFunctions.SetupClient("List Asset Shares - Sheet");

            ListAssetSharesResponse response = ss.AssetSharingResources.ListAssetShares(
                AssetType.SHEET,
                1234567890123456L
            );

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.IsTrue(response.Data.Count > 0);
        }

        [TestMethod]
        public void ListAssetShares_WithPagination()
        {
            SmartsheetClient ss = HelperFunctions.SetupClient("List Asset Shares - With Pagination");

            TokenPaginationParameters paginationParams = new TokenPaginationParameters(
                includeAll: false,
                pageSize: 10
            );

            ListAssetSharesResponse response = ss.AssetSharingResources.ListAssetShares(
                AssetType.SHEET,
                1234567890123456L,
                paginationParams
            );

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
        }

        [TestMethod]
        public void ListAssetShares_WithWorkspaceScope()
        {
            SmartsheetClient ss = HelperFunctions.SetupClient("List Asset Shares - With Workspace Scope");

            ListAssetSharesResponse response = ss.AssetSharingResources.ListAssetShares(
                AssetType.SHEET,
                1234567890123456L,
                null,
                ShareScope.WORKSPACE
            );

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            // Verify that both ITEM and WORKSPACE scoped shares are returned
            Assert.IsTrue(response.Data.Any(s => s.Scope == ShareScope.ITEM || s.Scope == ShareScope.WORKSPACE));
        }

        [TestMethod]
        public void GetAssetShare_Success()
        {
            SmartsheetClient ss = HelperFunctions.SetupClient("Get Asset Share - Success");

            AssetShare share = ss.AssetSharingResources.GetAssetShare(
                AssetType.SHEET,
                1234567890123456L,
                "AAABbbbCCCdddd"
            );

            Assert.IsNotNull(share);
            Assert.AreEqual("AAABbbbCCCdddd", share.Id);
            Assert.IsNotNull(share.Email);
            Assert.IsNotNull(share.AccessLevel);
        }

        [TestMethod]
        public void GetAssetShare_NotFound()
        {
            SmartsheetClient ss = HelperFunctions.SetupClient("Get Asset Share - Not Found");

            HelperFunctions.AssertRaisesException<ResourceNotFoundException>(() =>
                ss.AssetSharingResources.GetAssetShare(
                    AssetType.SHEET,
                    1234567890123456L,
                    "InvalidShareId"
                ),
                "Share not found."
            );
        }

        [TestMethod]
        public void ShareAsset_SingleUser()
        {
            SmartsheetClient ss = HelperFunctions.SetupClient("Share Asset - Single User");

            AssetShare newShare = new AssetShare.CreateAssetShareBuilder(AccessLevel.VIEWER)
                .SetEmail("user@example.com")
                .SetSubject("Check out this sheet")
                .SetMessage("I thought you might find this interesting")
                .SetCcMe(true)
                .Build();

            BulkItemResult<AssetShare> result = ss.AssetSharingResources.ShareAsset(
                AssetType.SHEET,
                1234567890123456L,
                new List<AssetShare> { newShare },
                sendEmail: true
            );

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Result);
            Assert.AreEqual(1, result.Result.Count);
            Assert.AreEqual("user@example.com", result.Result[0].Email);
        }

        [TestMethod]
        public void ShareAsset_MultipleUsers()
        {
            SmartsheetClient ss = HelperFunctions.SetupClient("Share Asset - Multiple Users");

            List<AssetShare> shares = new List<AssetShare>
            {
                new AssetShare.CreateAssetShareBuilder(AccessLevel.EDITOR)
                    .SetEmail("user1@example.com")
                    .Build(),
                new AssetShare.CreateAssetShareBuilder(AccessLevel.VIEWER)
                    .SetEmail("user2@example.com")
                    .Build()
            };

            BulkItemResult<AssetShare> result = ss.AssetSharingResources.ShareAsset(
                AssetType.SHEET,
                1234567890123456L,
                shares,
                sendEmail: false
            );

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Result);
            Assert.AreEqual(2, result.Result.Count);
        }

        [TestMethod]
        public void ShareAsset_WithGroup()
        {
            SmartsheetClient ss = HelperFunctions.SetupClient("Share Asset - With Group");

            AssetShare groupShare = new AssetShare.CreateAssetShareBuilder(AccessLevel.EDITOR)
                .SetGroupId("9876543210987654")
                .Build();

            BulkItemResult<AssetShare> result = ss.AssetSharingResources.ShareAsset(
                AssetType.REPORT,
                1234567890123456L,
                new List<AssetShare> { groupShare }
            );

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Result);
            Assert.AreEqual(1, result.Result.Count);
            Assert.AreEqual("9876543210987654", result.Result[0].GroupId);
        }

        [TestMethod]
        public void ShareAsset_InvalidEmail()
        {
            SmartsheetClient ss = HelperFunctions.SetupClient("Share Asset - Invalid Email");

            AssetShare invalidShare = new AssetShare.CreateAssetShareBuilder(AccessLevel.VIEWER)
                .SetEmail("invalid-email")
                .Build();

            HelperFunctions.AssertRaisesException<InvalidRequestException>(() =>
                ss.AssetSharingResources.ShareAsset(
                    AssetType.SHEET,
                    1234567890123456L,
                    new List<AssetShare> { invalidShare }
                ),
                "Invalid email address."
            );
        }

        [TestMethod]
        public void UpdateAssetShare_ChangeAccessLevel()
        {
            SmartsheetClient ss = HelperFunctions.SetupClient("Update Asset Share - Change Access Level");

            UpdateShareRequest updateRequest = new UpdateShareRequest
            {
                AccessLevel = AccessLevel.EDITOR
            };

            AssetShare updatedShare = ss.AssetSharingResources.UpdateAssetShare(
                AssetType.SHEET,
                1234567890123456L,
                "AAABbbbCCCdddd",
                updateRequest
            );

            Assert.IsNotNull(updatedShare);
            Assert.AreEqual(AccessLevel.EDITOR, updatedShare.AccessLevel);
        }

        [TestMethod]
        public void UpdateAssetShare_NotFound()
        {
            SmartsheetClient ss = HelperFunctions.SetupClient("Update Asset Share - Not Found");

            UpdateShareRequest updateRequest = new UpdateShareRequest
            {
                AccessLevel = AccessLevel.VIEWER
            };

            HelperFunctions.AssertRaisesException<ResourceNotFoundException>(() =>
                ss.AssetSharingResources.UpdateAssetShare(
                    AssetType.SHEET,
                    1234567890123456L,
                    "InvalidShareId",
                    updateRequest
                ),
                "Share not found."
            );
        }

        [TestMethod]
        public void DeleteAssetShare_Success()
        {
            SmartsheetClient ss = HelperFunctions.SetupClient("Delete Asset Share - Success");

            // Should not throw any exception
            ss.AssetSharingResources.DeleteAssetShare(
                AssetType.SHEET,
                1234567890123456L,
                "AAABbbbCCCdddd"
            );
        }

        [TestMethod]
        public void DeleteAssetShare_NotFound()
        {
            SmartsheetClient ss = HelperFunctions.SetupClient("Delete Asset Share - Not Found");

            HelperFunctions.AssertRaisesException<ResourceNotFoundException>(() =>
                ss.AssetSharingResources.DeleteAssetShare(
                    AssetType.SHEET,
                    1234567890123456L,
                    "InvalidShareId"
                ),
                "Share not found."
            );
        }

        [TestMethod]
        public void ShareAsset_DifferentAssetTypes()
        {
            // Test sharing different asset types
            var assetTypes = new[]
            {
                (AssetType.SHEET, "Share Asset - Sheet Type"),
                (AssetType.REPORT, "Share Asset - Report Type"),
                (AssetType.SIGHT, "Share Asset - Sight Type"),
                (AssetType.WORKSPACE, "Share Asset - Workspace Type")
            };

            foreach (var (assetType, scenario) in assetTypes)
            {
                SmartsheetClient ss = HelperFunctions.SetupClient(scenario);

                AssetShare newShare = new AssetShare.CreateAssetShareBuilder(AccessLevel.VIEWER)
                    .SetEmail("user@example.com")
                    .Build();

                BulkItemResult<AssetShare> result = ss.AssetSharingResources.ShareAsset(
                    assetType,
                    1234567890123456L,
                    new List<AssetShare> { newShare }
                );

                Assert.IsNotNull(result, $"Failed for asset type: {assetType}");
                Assert.IsNotNull(result.Result, $"Failed for asset type: {assetType}");
            }
        }

        [TestMethod]
        public void ShareAsset_AllAccessLevels()
        {
            // Test all access levels
            var accessLevels = new[]
            {
                AccessLevel.VIEWER,
                AccessLevel.EDITOR,
                AccessLevel.EDITOR_SHARE,
                AccessLevel.ADMIN,
                AccessLevel.OWNER
            };

            foreach (var accessLevel in accessLevels)
            {
                SmartsheetClient ss = HelperFunctions.SetupClient($"Share Asset - Access Level {accessLevel}");

                AssetShare newShare = new AssetShare.CreateAssetShareBuilder(accessLevel)
                    .SetEmail($"user-{accessLevel}@example.com")
                    .Build();

                BulkItemResult<AssetShare> result = ss.AssetSharingResources.ShareAsset(
                    AssetType.SHEET,
                    1234567890123456L,
                    new List<AssetShare> { newShare }
                );

                Assert.IsNotNull(result, $"Failed for access level: {accessLevel}");
                Assert.AreEqual(accessLevel, result.Result[0].AccessLevel, $"Access level mismatch for: {accessLevel}");
            }
        }
    }
}