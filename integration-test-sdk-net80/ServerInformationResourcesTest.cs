using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace integration_test_sdk_net60
{
    [TestClass]
    public class ServerInformationResourcesTest
    {

        [TestMethod]
        public void TestServerInfoResources()
        {
            SmartsheetClient smartsheet = new SmartsheetBuilder().SetMaxRetryTimeout(30000).Build();

            ServerInfo info = smartsheet.ServerInfoResources.GetServerInfo();
            Assert.IsTrue(info.FeatureInfo != null);
            Assert.IsTrue(info.Formats != null);
            Assert.IsTrue(info.SupportedLocales != null);

        }
    }
}