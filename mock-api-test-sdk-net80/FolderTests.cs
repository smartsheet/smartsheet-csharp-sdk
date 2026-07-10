using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smartsheet.Api;
using Smartsheet.Api.Models;
using System.Linq;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class FolderTests
    {
        [TestMethod]
        public void GetFolderChildren_NoParams()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Get Folder Children - No Params");

            // No params - use minimal required parameters
            TokenPaginatedResult<object> children = smartsheet.FolderResources.GetFolderChildren(456);

            Assert.IsNotNull(children);
            Assert.IsNotNull(children.Data);
            Assert.AreEqual(4, children.Data.Count);

            // Find specific items by ID and validate their properties using proper model classes
            var childrenList = children.Data.ToList();

            // Subfolder (id: 987)
            var subfolder = childrenList.FirstOrDefault(c => GetItemId(c) == 987) as Folder;
            Assert.IsNotNull(subfolder);
            Assert.AreEqual("Subfolder", subfolder.Name);
            Assert.AreEqual(ChildResourceType.FOLDER, subfolder.ResourceType);

            // Task List (id: 234)
            var taskList = childrenList.FirstOrDefault(c => GetItemId(c) == 234) as Sheet;
            Assert.IsNotNull(taskList);
            Assert.AreEqual("Task List", taskList.Name);
            Assert.AreEqual(AccessLevel.EDITOR, taskList.AccessLevel);
            Assert.AreEqual(ChildResourceType.SHEET, taskList.ResourceType);

            // Project Dashboard (id: 567)
            var projectDashboard = childrenList.FirstOrDefault(c => GetItemId(c) == 567) as Sight;
            Assert.IsNotNull(projectDashboard);
            Assert.AreEqual("Project Dashboard", projectDashboard.Name);
            Assert.AreEqual(AccessLevel.EDITOR, projectDashboard.AccessLevel);
            Assert.AreEqual(ChildResourceType.SIGHT, projectDashboard.ResourceType);

            // Status Report (id: 890)
            var statusReport = childrenList.FirstOrDefault(c => GetItemId(c) == 890) as Report;
            Assert.IsNotNull(statusReport);
            Assert.AreEqual("Status Report", statusReport.Name);
            Assert.AreEqual(AccessLevel.VIEWER, statusReport.AccessLevel);
            Assert.AreEqual(ChildResourceType.REPORT, statusReport.ResourceType);
        }

        [TestMethod]
        public void GetFolderChildren_IncludeSourceAndOwnerInfo()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Get Folder Children - Include Source and OwnerInfo");

            // Include source and owner info as specified in scenario title
            TokenPaginatedResult<object> children = smartsheet.FolderResources.GetFolderChildren(
                456,
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
                    case 987: // Subfolder
                        var subfolder = item as Folder;
                        Assert.IsNotNull(subfolder);
                        Assert.IsNotNull(subfolder.Source);
                        Assert.AreEqual(444L, subfolder.Source.Id);
                        Assert.AreEqual("folder", subfolder.Source.Type);
                        Assert.AreEqual(ChildResourceType.FOLDER, subfolder.ResourceType);
                        break;
                    case 234: // Task List
                        var taskList = item as Sheet;
                        Assert.IsNotNull(taskList);
                        Assert.IsNotNull(taskList.Source);
                        Assert.AreEqual(333L, taskList.Source.Id);
                        Assert.AreEqual("sheet", taskList.Source.Type);
                        // Verify owner info for sheet
                        Assert.AreEqual("jane.smith@example.com", taskList.Owner);
                        Assert.AreEqual(2002L, taskList.OwnerId);
                        Assert.AreEqual(ChildResourceType.SHEET, taskList.ResourceType);
                        break;
                    case 567: // Project Dashboard
                        var projectDashboard = item as Sight;
                        Assert.IsNotNull(projectDashboard);
                        Assert.IsNotNull(projectDashboard.Source);
                        Assert.AreEqual(222L, projectDashboard.Source.Id);
                        Assert.AreEqual("sight", projectDashboard.Source.Type);
                        Assert.AreEqual(ChildResourceType.SIGHT, projectDashboard.ResourceType);
                        break;
                    case 890: // Status Report
                        var statusReport = item as Report;
                        Assert.IsNotNull(statusReport);
                        Assert.IsNotNull(statusReport.Source);
                        Assert.AreEqual(111L, statusReport.Source.Id);
                        Assert.AreEqual("report", statusReport.Source.Type);
                        Assert.AreEqual(ChildResourceType.REPORT, statusReport.ResourceType);
                        break;
                }
            }
        }

        [TestMethod]
        public void GetFolderChildren_FilterSightsAndReports()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Get Folder Children - Filter Sights and Reports");

            // Filter to only sights and reports as specified in scenario title
            TokenPaginatedResult<object> children = smartsheet.FolderResources.GetFolderChildren(
                456,
                new List<ChildrenResourceType> { ChildrenResourceType.REPORTS, ChildrenResourceType.SIGHTS }
            );

            Assert.IsNotNull(children);
            Assert.IsNotNull(children.Data);
            Assert.AreEqual(3, children.Data.Count);

            // Verify the returned items using proper model classes
            var childrenList = children.Data.ToList();

            // Project Dashboard (id: 567)
            var projectDashboard = childrenList.FirstOrDefault(c => GetItemId(c) == 567) as Sight;
            Assert.IsNotNull(projectDashboard);
            Assert.AreEqual("Project Dashboard", projectDashboard.Name);
            Assert.AreEqual(AccessLevel.EDITOR, projectDashboard.AccessLevel);
            Assert.AreEqual(ChildResourceType.SIGHT, projectDashboard.ResourceType);

            // Status Report (id: 890)
            var statusReport = childrenList.FirstOrDefault(c => GetItemId(c) == 890) as Report;
            Assert.IsNotNull(statusReport);
            Assert.AreEqual("Status Report", statusReport.Name);
            Assert.AreEqual(AccessLevel.VIEWER, statusReport.AccessLevel);
            Assert.AreEqual(ChildResourceType.REPORT, statusReport.ResourceType);

            // Executive Summary (id: 1567)
            var executiveSummary = childrenList.FirstOrDefault(c => GetItemId(c) == 1567) as Sight;
            Assert.IsNotNull(executiveSummary);
            Assert.AreEqual("Executive Summary", executiveSummary.Name);
            Assert.AreEqual(AccessLevel.VIEWER, executiveSummary.AccessLevel);
            Assert.AreEqual(ChildResourceType.SIGHT, executiveSummary.ResourceType);

            // Verify only sights and reports are returned (no folders or sheets)
            Assert.IsTrue(childrenList.All(item => item is Sight || item is Report));
        }

        [TestMethod]
        public void GetFolderMetadata_NoParams()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Get Folder Metadata - No Params");

            // No params - use minimal required parameters
            Folder folder = smartsheet.FolderResources.GetFolderMetadata(456);

            Assert.IsNotNull(folder);
            Assert.AreEqual(456, folder.Id);
            Assert.AreEqual("Project Folder", folder.Name);
            Assert.AreEqual("https://app.smartsheet.com/b/home?lx=*****************", folder.Permalink);
            Assert.IsNotNull(folder.CreatedAt);
            Assert.IsNotNull(folder.ModifiedAt);
        }

        [TestMethod]
        public void GetFolderMetadata_IncludeSource()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Get Folder Metadata - Include Source");

            // Include source as specified in scenario title
            Folder folder = smartsheet.FolderResources.GetFolderMetadata(
                456,
                new List<FolderInclusion> { FolderInclusion.SOURCE }
            );

            Assert.IsNotNull(folder);
            Assert.AreEqual(456, folder.Id);
            Assert.AreEqual("Project Folder", folder.Name);
            Assert.IsNotNull(folder.Source);
            Assert.AreEqual(888, folder.Source.Id);
            Assert.AreEqual("folder", folder.Source.Type);
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