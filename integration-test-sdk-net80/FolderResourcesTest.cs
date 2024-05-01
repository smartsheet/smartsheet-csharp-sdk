using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace integration_test_sdk_net80
{
    [TestClass]
    public class FolderResourcesTest
    {
        /// <summary>
        /// Create Folder
        /// smartsheet.HomeResources.FolderResources.CreateFolder(new Folder.CreateFolderBuilder("New Folder").Build());
        /// 
        /// Create Folder (Folder)
        /// smartsheet.FolderResources.CreateFolder(123, new Folder.CreateFolderBuilder("New Folder").Build());
        /// 
        /// Create Folder (Workspace)
        /// smartsheet.WorkspaceResources.FolderResources.CreateFolder(123, new Folder.CreateFolderBuilder("New Folder").Build());
        /// 
        /// Delete Folder
        /// smartsheet.FolderResources.DeleteFolder(123);
        /// 
        /// Get Folder
        /// smartsheet.FolderResources.GetFolder(123);
        /// 
        /// List Folder
        /// smartsheet.HomeResources.FolderResources.ListFolders(null);
        /// 
        /// List Folders (Folder)
        /// smartsheet.FolderResources.ListFolders(123, null);
        /// 
        /// List Folders (Workspace)
        /// smartsheet.WorkspaceResources.FolderResources.ListFolders(123, null);
        /// 
        /// Update Folder
        /// smartsheet.FolderResources.UpdateFolder(123, new Folder.UpdateFolderBuilder("New name for folder").Build());
        /// </summary>

        private static string folderInHomeName = "new folder in home";
        private static string folderInFolderName = "new folder in folder";
        private static string updatedFolderInFolderName = "updated folder in folder";

        [TestMethod]
        public void TestFolderResources()
        {
            SmartsheetClient smartsheet = new SmartsheetBuilder().SetMaxRetryTimeout(30000).Build();

            long createdFolderInHomeId = CreateFolderInHome(smartsheet);
            long createdFolderInFolderId = CreateFolderInFolder(smartsheet, createdFolderInHomeId);
            ListFoldersInFolder(smartsheet, createdFolderInHomeId, createdFolderInFolderId);
            UpdateFolderInFolder(smartsheet, createdFolderInFolderId);
            GetFolderInHome(smartsheet, createdFolderInHomeId, createdFolderInFolderId);
            DeleteFolders(smartsheet, createdFolderInHomeId, createdFolderInFolderId);
        }


        private static void DeleteFolders(SmartsheetClient smartsheet, long createdFolderInHomeId, long createdFolderInFolderId)
        {
            smartsheet.FolderResources.DeleteFolder(createdFolderInFolderId);
            try
            {
                smartsheet.FolderResources.GetFolder(createdFolderInFolderId);
                Assert.Fail("Exception should have been thrown. Cannot get a deleted folder.");
            }
            catch
            {
                // Should be "Not Found".
            }
            smartsheet.FolderResources.DeleteFolder(createdFolderInHomeId);
            try
            {
                smartsheet.FolderResources.GetFolder(createdFolderInHomeId);
                Assert.Fail("Exception should have been thrown. Cannot get a deleted folder.");
            }
            catch
            {
                // Should be "Not Found".
            }
        }

        private static void GetFolderInHome(SmartsheetClient smartsheet, long createdFolderInHomeId, long createdFolderInFolderId)
        {
            Folder getFolderWithoutPagination = smartsheet.FolderResources.GetFolder(createdFolderInHomeId);
            Assert.IsTrue(getFolderWithoutPagination.Id == createdFolderInHomeId);
            Assert.IsTrue(getFolderWithoutPagination.Name == folderInHomeName);
            Assert.IsTrue(getFolderWithoutPagination.Folders.Count == 1);
            Assert.IsTrue(getFolderWithoutPagination.Folders[0].Id == createdFolderInFolderId);
            Assert.IsTrue(getFolderWithoutPagination.Folders[0].Name == updatedFolderInFolderName);

            List<FolderInclusion> inclusionParameters = new List<FolderInclusion>();
            inclusionParameters.Add(FolderInclusion.SHEET_VERSION);
            Folder getFolderWithPagination = smartsheet.FolderResources.GetFolder(createdFolderInHomeId, inclusionParameters);
            Assert.IsTrue(getFolderWithPagination.Id == createdFolderInHomeId);
            Assert.IsTrue(getFolderWithPagination.Name == folderInHomeName);
            Assert.IsTrue(getFolderWithPagination.Folders.Count == 1);
            Assert.IsTrue(getFolderWithPagination.Folders[0].Id == createdFolderInFolderId);
            Assert.IsTrue(getFolderWithPagination.Folders[0].Name == updatedFolderInFolderName);
        }

        private static void UpdateFolderInFolder(SmartsheetClient smartsheet, long createdFolderInFolderId)
        {
            Folder updatedFolderInFolder = smartsheet.FolderResources.UpdateFolder(new Folder.UpdateFolderBuilder(createdFolderInFolderId, updatedFolderInFolderName).Build());
            Assert.IsTrue(updatedFolderInFolder.Name == updatedFolderInFolderName);
        }

        private static void ListFoldersInFolder(SmartsheetClient smartsheet, long createdFolderInHomeId, long createdFolderInFolderId)
        {
            PaginatedResult<Folder> folderResults = smartsheet.FolderResources.ListFolders(createdFolderInHomeId);
            Assert.IsTrue(folderResults.Data.Count == 1);
            Assert.IsTrue(folderResults.TotalCount == 1);
            Assert.IsTrue(folderResults.Data[0].Id == createdFolderInFolderId);
        }

        private static long CreateFolderInFolder(SmartsheetClient smartsheet, long createdFolderInHomeId)
        {
            Folder createdFolderInFolder = smartsheet.FolderResources.CreateFolder(createdFolderInHomeId, new Folder.CreateFolderBuilder(folderInFolderName).Build());
            Assert.IsTrue(createdFolderInFolder.Name == folderInFolderName);
            return createdFolderInFolder.Id.Value;
        }

        private static long CreateFolderInHome(SmartsheetClient smartsheet)
        {
            Folder createdFolderInHome = smartsheet.HomeResources.FolderResources.CreateFolder(new Folder.CreateFolderBuilder(folderInHomeName).Build());
            Assert.IsTrue(createdFolderInHome.Name == folderInHomeName);
            return createdFolderInHome.Id.Value;
        }
    }
}