using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class SightsTest
    {
        [TestMethod]
        public void ListSights()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("List Sights");
            PaginatedResult<Sight> sights = smartsheet.SightResources.ListSights();
            Assert.IsNotNull(sights?.TotalCount);
            Assert.AreEqual(6, (long)sights.TotalCount);
        }

        [TestMethod]
        public void GetSight()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Get Sight");
            Sight sight = smartsheet.SightResources.GetSight(52);
            Assert.IsNotNull(sight?.Id);
            Assert.AreEqual(52, (long)sight.Id);
        }

        [Ignore]
        [TestMethod]
        public void CopySight()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Copy Sight");
            ContainerDestination dest = new ContainerDestination();
            dest.DestinationType = DestinationType.FOLDER;
            dest.DestinationId = 424;
            dest.NewName = "new sight";
            Sight sight = smartsheet.SightResources.CopySight(52, dest);
        }

        [TestMethod]
        public void UpdateSight()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Update Sight");
            Sight sight = new Sight();
            sight.Id = 812;
            sight.Name = "new new sight";
            smartsheet.SightResources.UpdateSight(sight);
        }

        [TestMethod]
        public void SetPublishStatus()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Set Sight Publish Status");
            SightPublish publish = new SightPublish();
            publish.ReadOnlyFullEnabled = true;
            publish.ReadOnlyFullAccessibleBy = "ALL";
            smartsheet.SightResources.SetPublishStatus(812, publish);
        }

        [TestMethod]
        public void GetPublishStatus()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Get Sight Publish Status");
            SightPublish publish = smartsheet.SightResources.GetPublishStatus(812);
            Assert.IsNotNull(publish.ReadOnlyFullEnabled);
            Assert.IsTrue(publish.ReadOnlyFullEnabled.Value);
        }

        [TestMethod]
        public void DeleteSight()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Delete Sight");
            smartsheet.SightResources.DeleteSight(700);
        }
    }
}
