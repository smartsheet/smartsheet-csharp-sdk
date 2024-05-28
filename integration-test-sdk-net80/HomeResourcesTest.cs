using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace integration_test_sdk_net80
{
    [TestClass]
    public class HomeResourcesTest
    {
        [TestMethod]
        public void TestHomeResources()
        {
            SmartsheetClient smartsheet = new SmartsheetBuilder().SetMaxRetryTimeout(30000).Build();

            PersonalFolder personalFolder = smartsheet.HomeResources.GetFoldersPersonal(new HomeInclusion[] { HomeInclusion.SOURCE });

            Assert.IsTrue(personalFolder != null);

            //Test without includes or excludes
            personalFolder = smartsheet.HomeResources.GetFoldersPersonal();

            Assert.IsTrue(personalFolder != null);
        }

        [TestMethod]
        public void TestHomeFolderResources()
        {
            SmartsheetClient smartsheet = new SmartsheetBuilder().SetMaxRetryTimeout(30000).Build();

            var folders = smartsheet.HomeResources.FolderResources.ListFolders();

            Assert.IsTrue(folders != null);

            PaginationParameters paginationParameters = new PaginationParameters(true, 100, 1);
            folders = smartsheet.HomeResources.FolderResources.ListFolders(paginationParameters);

            Assert.IsTrue(folders != null);
        }
    }
}