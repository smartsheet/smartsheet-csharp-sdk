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
        }
    }
}