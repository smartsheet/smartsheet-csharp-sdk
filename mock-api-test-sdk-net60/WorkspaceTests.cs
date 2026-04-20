using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net60
{
    [TestClass]
    public class WorkspaceTests
    {
        private Workspace createWorkspace(SmartsheetClient client, string workspaceName) {
            Workspace workspace = new Workspace();

            workspace.Name = workspaceName;

            return client.WorkspaceResources.CreateWorkspace(workspace);
        }


        [TestMethod]
        public void CreateWorkspace_Success()
        {
            SmartsheetClient client = HelperFunctions.SetupClient("Create Workspace - Valid");

            Workspace newWorkspace = new Workspace();

            newWorkspace.Name = "New workspace";
            
            Workspace workspace = client.WorkspaceResources.CreateWorkspace(newWorkspace);
            
            Assert.IsNotNull(workspace);
            Assert.IsNotNull(workspace.Id);
            Assert.IsNotNull(workspace.Name);
            Assert.IsNotNull(workspace.AccessLevel);
            Assert.IsNotNull(workspace.Permalink);
        }

        [TestMethod]
        public void CreateFolder_Success()
        {
            SmartsheetClient client = HelperFunctions.SetupClient("Create Folder in Workspace - Valid");

            Folder newFolder = new Folder();

            newFolder.Name = "New Folder";

            Folder folder = client.WorkspaceFolderResources.CreateFolder(12345, newFolder);

            Assert.IsNotNull(folder);
            Assert.IsNotNull(folder.Name);
            Assert.AreEqual("New Folder", folder.Name);
        }

        [TestMethod]
        public void CreateSheet_NoColumns()
        {
            SmartsheetClient ss = HelperFunctions.SetupClient("Create Sheet - Invalid - No Columns");

            Sheet sheetA = new Sheet
            {
                Name = "New Sheet",
                Columns = new List<Column>()
            };

            HelperFunctions.AssertRaisesException<SmartsheetException>(() =>
                ss.SheetResources.CreateSheet(sheetA),
                "The new sheet requires either a fromId or columns.");
        }
    }
}
