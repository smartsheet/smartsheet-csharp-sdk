
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace integration_test_sdk_net80
{
    [TestClass]
    public class WorkspaceResourcesCopyTest
    {
        [TestMethod]
        public void TestWorkspaceCopyResources()
        {
            SmartsheetClient smartsheet = new SmartsheetBuilder().SetMaxRetryTimeout(30000).SetSmartsheetIntegrationSource("AI,MyOrg,MyGPT").Build();
            // Before
            // Workspace1
            // Folder2

            // After
            // Workspace1
            // Folder2-----Workspace1Copy

            Workspace workspace = smartsheet.WorkspaceResources.CreateWorkspace(new Workspace.CreateWorkspaceBuilder("Workspace1").Build());
            Assert.IsNotNull(workspace.Id);
            long workspaceId = workspace.Id.Value;

            ContainerDestination destination = new ContainerDestination
            {
                NewName = "Workspace1Copy"
            };
            Workspace newCopiedWorkspace = smartsheet.WorkspaceResources.CopyWorkspace(workspaceId, destination, new WorkspaceCopyInclusion[] { WorkspaceCopyInclusion.ALL }, new WorkspaceRemapExclusion[] { WorkspaceRemapExclusion.CELL_LINKS });

            Assert.IsTrue(newCopiedWorkspace.Name == "Workspace1Copy");

            Assert.IsNotNull(newCopiedWorkspace.Id);
            long copiedWorkspaceId = newCopiedWorkspace.Id.Value;

            Workspace copiedWorkspace = smartsheet.WorkspaceResources.GetWorkspace(copiedWorkspaceId);
            Assert.IsTrue(copiedWorkspace.Name == "Workspace1Copy");

            smartsheet.WorkspaceResources.DeleteWorkspace(workspaceId);
            smartsheet.WorkspaceResources.DeleteWorkspace(copiedWorkspaceId);
        }

        private static void DeleteFolders(SmartsheetClient smartsheet, long folder1)
        {
            smartsheet.FolderResources.DeleteFolder(folder1);
            try
            {
                smartsheet.FolderResources.GetFolder(folder1);
                Assert.Fail("Exception should have been thrown. Cannot get a deleted folder.");
            }
            catch
            {
                // Should be "Not Found".
            }
        }

        private static long CreateWorkspaceInFolder(SmartsheetClient smartsheet, long folderId, string folderName)
        {
            Folder createdFolderInFolder = smartsheet.WorkspaceResources.FolderResources.CreateFolder(folderId, new Folder.CreateFolderBuilder(folderName).Build());
            Assert.IsNotNull(createdFolderInFolder.Id);
            return createdFolderInFolder.Id.Value;
        }

        private static long CreateFolderInHome(SmartsheetClient smartsheet, string folderName)
        {
            Folder createdFolderInHome = smartsheet.HomeResources.FolderResources.CreateFolder(new Folder.CreateFolderBuilder(folderName).Build());
            Assert.IsNotNull(createdFolderInHome.Id);
            return createdFolderInHome.Id.Value;
        }
    }
}