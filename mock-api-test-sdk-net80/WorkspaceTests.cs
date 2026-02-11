using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smartsheet.Api;
using Smartsheet.Api.Models;
using System.Linq;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class WorkspaceTests
    {
        [TestMethod]
        public void GetWorkspaceChildren_NoParams()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Get Workspace Children - No Params");

            // No params - use minimal required parameters
            TokenPaginatedResult<object> children = smartsheet.WorkspaceResources.GetWorkspaceChildren(123);

            Assert.IsNotNull(children);
            Assert.IsNotNull(children.Data);
            Assert.AreEqual(4, children.Data.Count);

            // Find specific items by ID and validate their properties
            var childrenList = children.Data.ToList();

            // Project Folder (id: 456)
            var projectFolder = childrenList.FirstOrDefault(c => GetItemId(c) == 456) as Folder;
            Assert.IsNotNull(projectFolder);
            Assert.AreEqual("Project Folder", projectFolder.Name);

            // Budget Sheet (id: 789)
            var budgetSheet = childrenList.FirstOrDefault(c => GetItemId(c) == 789) as Sheet;
            Assert.IsNotNull(budgetSheet);
            Assert.AreEqual("Budget Sheet", budgetSheet.Name);
            Assert.AreEqual(AccessLevel.EDITOR, budgetSheet.AccessLevel);

            // Dashboard Overview (id: 321)
            var dashboardOverview = childrenList.FirstOrDefault(c => GetItemId(c) == 321) as Sight;
            Assert.IsNotNull(dashboardOverview);
            Assert.AreEqual("Dashboard Overview", dashboardOverview.Name);
            Assert.AreEqual(AccessLevel.VIEWER, dashboardOverview.AccessLevel);

            // Monthly Report (id: 654)
            var monthlyReport = childrenList.FirstOrDefault(c => GetItemId(c) == 654) as Report;
            Assert.IsNotNull(monthlyReport);
            Assert.AreEqual("Monthly Report", monthlyReport.Name);
            Assert.AreEqual(AccessLevel.ADMIN, monthlyReport.AccessLevel);
        }

        [TestMethod]
        public void GetWorkspaceChildren_IncludeSourceAndOwnerInfo()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Get Workspace Children - Include Source and OwnerInfo");

            // Include source and owner info as specified in scenario title
            TokenPaginatedResult<object> children = smartsheet.WorkspaceResources.GetWorkspaceChildren(
                123,
                null,
                new List<ChildrenInclusion> { ChildrenInclusion.SOURCE, ChildrenInclusion.OWNER_INFO }
            );

            Assert.IsNotNull(children);
            Assert.IsNotNull(children.Data);
            Assert.AreEqual(4, children.Data.Count);

            // Verify specific source values for each child using proper model classes
            foreach (var item in children.Data)
            {
                var itemId = GetItemId(item);

                // Assert specific source values based on the expected response
                switch (itemId)
                {
                    case 456: // Project Folder
                        var projectFolder = item as Folder;
                        Assert.IsNotNull(projectFolder);
                        Assert.IsNotNull(projectFolder.Source);
                        Assert.AreEqual(888L, projectFolder.Source.Id);
                        Assert.AreEqual("folder", projectFolder.Source.Type);
                        break;
                    case 789: // Budget Sheet
                        var budgetSheet = item as Sheet;
                        Assert.IsNotNull(budgetSheet);
                        Assert.IsNotNull(budgetSheet.Source);
                        Assert.AreEqual(777L, budgetSheet.Source.Id);
                        Assert.AreEqual("sheet", budgetSheet.Source.Type);
                        // Verify owner info for sheet
                        Assert.AreEqual("john.doe@example.com", budgetSheet.Owner);
                        Assert.AreEqual(1001L, budgetSheet.OwnerId);
                        break;
                    case 321: // Dashboard Overview
                        var dashboardOverview = item as Sight;
                        Assert.IsNotNull(dashboardOverview);
                        Assert.IsNotNull(dashboardOverview.Source);
                        Assert.AreEqual(666L, dashboardOverview.Source.Id);
                        Assert.AreEqual("sight", dashboardOverview.Source.Type);
                        break;
                    case 654: // Monthly Report
                        var monthlyReport = item as Report;
                        Assert.IsNotNull(monthlyReport);
                        Assert.IsNotNull(monthlyReport.Source);
                        Assert.AreEqual(555L, monthlyReport.Source.Id);
                        Assert.AreEqual("report", monthlyReport.Source.Type);
                        break;
                }
            }
        }

        [TestMethod]
        public void GetWorkspaceChildren_FilterSheetsAndFolders()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Get Workspace Children - Filter Sheets and Folders");

            // Filter to only sheets and folders as specified in scenario title
            TokenPaginatedResult<object> children = smartsheet.WorkspaceResources.GetWorkspaceChildren(
                123,
                new List<ChildrenResourceType> { ChildrenResourceType.FOLDERS, ChildrenResourceType.SHEETS }
            );

            Assert.IsNotNull(children);
            Assert.IsNotNull(children.Data);
            Assert.AreEqual(3, children.Data.Count);

            // Verify the returned items using proper model classes
            var childrenList = children.Data.ToList();

            // Project Folder (id: 456)
            var projectFolder = childrenList.FirstOrDefault(c => GetItemId(c) == 456) as Folder;
            Assert.IsNotNull(projectFolder);
            Assert.AreEqual("Project Folder", projectFolder.Name);

            // Budget Sheet (id: 789)
            var budgetSheet = childrenList.FirstOrDefault(c => GetItemId(c) == 789) as Sheet;
            Assert.IsNotNull(budgetSheet);
            Assert.AreEqual("Budget Sheet", budgetSheet.Name);
            Assert.AreEqual(AccessLevel.EDITOR, budgetSheet.AccessLevel);

            // Project Timeline (id: 1234)
            var projectTimeline = childrenList.FirstOrDefault(c => GetItemId(c) == 1234) as Sheet;
            Assert.IsNotNull(projectTimeline);
            Assert.AreEqual("Project Timeline", projectTimeline.Name);
            Assert.AreEqual(AccessLevel.EDITOR, projectTimeline.AccessLevel);

            // Verify only folders and sheets are returned (no sights or reports)
            Assert.IsTrue(childrenList.All(item => item is Folder || item is Sheet));
        }

        [TestMethod]
        public void GetWorkspaceMetadata_NoParams()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Get Workspace Metadata - No Params");

            // No params - use minimal required parameters
            Workspace workspace = smartsheet.WorkspaceResources.GetWorkspaceMetadata(123);

            Assert.IsNotNull(workspace);
            Assert.AreEqual(123, workspace.Id);
            Assert.AreEqual("Sample Workspace", workspace.Name);
            Assert.AreEqual("https://app.smartsheet.com/b/home?lx=*****************", workspace.Permalink);
            Assert.AreEqual(AccessLevel.VIEWER, workspace.AccessLevel);
            Assert.IsNotNull(workspace.CreatedAt);
            Assert.IsNotNull(workspace.ModifiedAt);
        }

        [TestMethod]
        public void GetWorkspaceMetadata_IncludeSource()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Get Workspace Metadata - Include Source");

            // Include source as specified in scenario title
            Workspace workspace = smartsheet.WorkspaceResources.GetWorkspaceMetadata(
                123,
                new List<WorkspaceInclusion> { WorkspaceInclusion.SOURCE }
            );

            Assert.IsNotNull(workspace);
            Assert.AreEqual(123, workspace.Id);
            Assert.AreEqual("Sample Workspace", workspace.Name);
            Assert.AreEqual(AccessLevel.ADMIN, workspace.AccessLevel);
            Assert.IsNotNull(workspace.Source);
            Assert.AreEqual(999, workspace.Source.Id);
            Assert.AreEqual("workspace", workspace.Source.Type);
        }

        /// <summary>
        /// Helper method to get the ID from any model object
        /// </summary>
        private long GetItemId(object item)
        {
            if (item is NamedModel namedModel)
            {
                return namedModel.Id ?? 0;
            }
            return 0;
        }
    }
}