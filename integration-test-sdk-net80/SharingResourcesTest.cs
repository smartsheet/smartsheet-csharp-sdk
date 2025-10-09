using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace integration_test_sdk_net80
{
    [TestClass]
    public class SharingResourcesTest
    {
        [TestMethod]
        public void TestSharingResources()
        {
            SmartsheetClient smartsheet = new SmartsheetBuilder().SetMaxRetryTimeout(30000).Build();
            
            // Create test assets
            long sheetId = CreateSheet(smartsheet);
            long workspaceId = CreateWorkspace(smartsheet);
            
            // Create a share object
            Share share = new Share.CreateShareBuilder("sharingexp_test@smartsheet.biz", AccessLevel.EDITOR).Build();
            
            // Test sharing for different asset types
            TestAssetSharing(smartsheet, AssetType.SHEET, sheetId, share);
            TestAssetSharing(smartsheet, AssetType.WORKSPACE, workspaceId, share);
            
            // Clean up
            smartsheet.SheetResources.DeleteSheet(sheetId);
            smartsheet.WorkspaceResources.DeleteWorkspace(workspaceId);
        }
        
        private static void TestAssetSharing(SmartsheetClient smartsheet, AssetType assetType, long assetId, Share share)
        {
            // Share the asset
            BulkItemResult<Share> shares = smartsheet.SharingResources.ShareAsset(assetType, assetId, new Share[] { share }, false);
            Assert.IsNotNull(shares);
            Assert.IsTrue(shares.Result.Count > 0);
            string shareId = shares.Result[0].Id;

            // Assert.IsNotNull(shareId);
            
            // List shares for the asset
            GetSharesResponse shareList = smartsheet.SharingResources.ListAssetShares(assetType, assetId);
            Assert.IsNotNull(shareList);
            Assert.IsTrue(shareList.Items.Count > 0);
            
            // Get a specific share
            Share retrievedShare = smartsheet.SharingResources.GetAssetShare(assetType, assetId, shareId);
            Assert.IsNotNull(retrievedShare);
            Assert.AreEqual(shareId, retrievedShare.Id);
            Assert.AreEqual(AccessLevel.EDITOR, retrievedShare.AccessLevel);
            
            // Update the share
            Share updatedShare = smartsheet.SharingResources.UpdateShare(assetType, assetId, shareId, new UpdateShareRequest { AccessLevel = AccessLevel.VIEWER });
            Assert.IsNotNull(updatedShare);
            Assert.AreEqual(AccessLevel.VIEWER, updatedShare.AccessLevel);
            
            // Delete the share
            smartsheet.SharingResources.DeleteShare(assetType, assetId, shareId);
        }
        
        private static long CreateWorkspace(SmartsheetClient smartsheet)
        {
            Workspace ws = smartsheet.WorkspaceResources.CreateWorkspace(new Workspace.CreateWorkspaceBuilder("Test Workspace").Build());
            Assert.IsNotNull(ws.Id);
            long workspaceId = ws.Id.Value;
            return workspaceId;
        }
        
        private static long CreateSheet(SmartsheetClient smartsheet)
        {
            Column[] columnsToCreate = new Column[] {
                new Column.CreateSheetColumnBuilder("Column 1", true, ColumnType.TEXT_NUMBER).Build(),
                new Column.CreateSheetColumnBuilder("Column 2", false, ColumnType.DATE).Build(),
                new Column.CreateSheetColumnBuilder("Column 3", false, ColumnType.TEXT_NUMBER).Build(),
            };
            Sheet createdSheet = smartsheet.SheetResources.CreateSheet(new Sheet.CreateSheetBuilder("Test Sheet", columnsToCreate).Build());
            Assert.IsTrue(createdSheet.Columns.Count == 3);
            Assert.IsNotNull(createdSheet.Id);
            return createdSheet.Id.Value;
        }
    }
}