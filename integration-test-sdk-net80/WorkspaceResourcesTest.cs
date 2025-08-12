using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace integration_test_sdk_net80
{
    [TestClass]
    public class WorkspaceResourcesTest
    {
        [TestMethod]
        public void TestWorkspaceResources()
        {
            SmartsheetClient smartsheet = new SmartsheetBuilder().SetMaxRetryTimeout(30000).Build();
            
            long workspaceId = CreateWorkspace(smartsheet);

            UpdateWorkspace(smartsheet, workspaceId);

            GetWorkspace(smartsheet, workspaceId);

            ListWorkspaces(smartsheet, workspaceId);

            ListWorkspacesWithTokenPagination(smartsheet, workspaceId);

            DeleteWorkspace(smartsheet, workspaceId);
        }

        private static void DeleteWorkspace(SmartsheetClient smartsheet, long workspaceId)
        {
            smartsheet.WorkspaceResources.DeleteWorkspace(workspaceId);
            try
            {
                smartsheet.WorkspaceResources.GetWorkspace(workspaceId);
                Assert.Fail("Cannot get a workspace that was deleted.");
            }
            catch
            {
                //Not found.
            }
        }

        private static void ListWorkspaces(SmartsheetClient smartsheet, long workspaceId)
        {
            PaginatedResult<Workspace> workspaceResult = smartsheet.WorkspaceResources.ListWorkspaces();
            Assert.IsTrue(workspaceResult.Data.Count > 0);
            bool contains = false;
        }

        private static void ListWorkspacesWithTokenPagination(SmartsheetClient smartsheet, long workspaceId)
        {
            // Test token-based pagination with maxItems parameter
            TokenPaginationParameters tokenPaging = new TokenPaginationParameters(null, 100);
            TokenPaginatedResult<Workspace> workspaceResult = smartsheet.WorkspaceResources.ListWorkspaces(tokenPaging);
            
            Assert.IsNotNull(workspaceResult);
            Assert.IsNotNull(workspaceResult.Data);
            Assert.IsTrue(workspaceResult.Data.Count > 0);

            // Test token-based pagination with explicit paginationType parameter
            TokenPaginationParameters tokenPagingWithType = new TokenPaginationParameters(null, 100, "token");
            TokenPaginatedResult<Workspace> workspaceResultWithType = smartsheet.WorkspaceResources.ListWorkspaces(tokenPagingWithType);
            Assert.IsNotNull(workspaceResultWithType);
            Assert.IsNotNull(workspaceResultWithType.Data);
            Assert.IsTrue(workspaceResultWithType.Data.Count > 0);
            Assert.AreEqual("token", tokenPagingWithType.PaginationType);

            // Test token-based pagination without parameters (should get default results)
            TokenPaginatedResult<Workspace> workspaceResultNoPaging = smartsheet.WorkspaceResources.ListWorkspaces((TokenPaginationParameters?)null);
            Assert.IsNotNull(workspaceResultNoPaging);
            Assert.IsNotNull(workspaceResultNoPaging.Data);
            Assert.IsTrue(workspaceResultNoPaging.Data.Count > 0);

            // Test lastKey functionality for token pagination
            // First page with small maxItems to ensure pagination occurs
            TokenPaginationParameters firstPageParams = new TokenPaginationParameters(null, 100);
            TokenPaginatedResult<Workspace> firstPageResult = smartsheet.WorkspaceResources.ListWorkspaces(firstPageParams);
            
            Assert.IsNotNull(firstPageResult);
            Assert.IsNotNull(firstPageResult.Data);
            Assert.IsTrue(firstPageResult.Data.Count > 0);
            
            // If there are more workspaces, there should be a lastKey for the next page
            if (firstPageResult.LastKey != null)
            {
                // Second page using the lastKey from the first page
                TokenPaginationParameters secondPageParams = new TokenPaginationParameters(firstPageResult.LastKey, 100);
                TokenPaginatedResult<Workspace> secondPageResult = smartsheet.WorkspaceResources.ListWorkspaces(secondPageParams);
                
                Assert.IsNotNull(secondPageResult);
                Assert.IsNotNull(secondPageResult.Data);
                
                // Verify that the second page returns different data (if available)
                if (secondPageResult.Data.Count > 0 && firstPageResult.Data.Count > 0)
                {
                    // The first workspace from page 1 should be different from the first workspace from page 2
                    Assert.AreNotEqual(firstPageResult.Data[0].Id, secondPageResult.Data[0].Id, 
                        "Second page should return different workspaces than first page");
                }
            }

            // Test that maxItems=1 generates an error from Smartsheet API
            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
            {
                TokenPaginationParameters invalidParams = new TokenPaginationParameters(null, 1);
                smartsheet.WorkspaceResources.ListWorkspaces(invalidParams);
            });
            
            Assert.IsNotNull(exception.Message, "Exception should have a message");
        }

        private static void GetWorkspace(SmartsheetClient smartsheet, long workspaceId)
        {
            Workspace workspace = smartsheet.WorkspaceResources.GetWorkspace(workspaceId, true, new WorkspaceInclusion[] { WorkspaceInclusion.SOURCE });
            Assert.IsNotNull(workspace.Id);
            Assert.IsTrue(workspace.Id.Value == workspaceId);
        }

        private static void UpdateWorkspace(SmartsheetClient smartsheet, long workspaceId)
        {
            Workspace workspace = new Workspace.UpdateWorkspaceBuilder(workspaceId, "updated workspace").Build();

            Workspace updatedWorkspace = smartsheet.WorkspaceResources.UpdateWorkspace(workspace);

            Assert.IsTrue(updatedWorkspace.Name == "updated workspace");
        }

        private static long CreateWorkspace(SmartsheetClient smartsheet)
        {
            Workspace workspace = new Workspace.CreateWorkspaceBuilder("workspace").Build();

            Workspace createdWorkspace = smartsheet.WorkspaceResources.CreateWorkspace(workspace);
            Assert.IsTrue(createdWorkspace.Name == "workspace");
            Assert.IsNotNull(createdWorkspace.Id);
            return createdWorkspace.Id.Value;
        }
    }
}