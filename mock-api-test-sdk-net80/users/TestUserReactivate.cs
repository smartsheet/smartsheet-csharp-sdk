using System.Web;
using Newtonsoft.Json;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestUserReactivate
    {
        [TestMethod]
        public async Task TestReactivateUserGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/users/reactivate-user/all-response-body-properties", requestId.ToString());

            smartsheet.UserResources.ReactivateUser(CommonTestConstants.TEST_USER_ID);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual($"/2.0/users/{CommonTestConstants.TEST_USER_ID}/reactivate", path);
        }

        [TestMethod]
        public async Task TestReactivateUserAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/users/reactivate-user/all-response-body-properties", requestId.ToString());

            // If this throws an exception, the test will fail automatically
            smartsheet.UserResources.ReactivateUser(CommonTestConstants.TEST_USER_ID);
        }

        [TestMethod]
        public void TestReactivateUserError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.UserResources.ReactivateUser(CommonTestConstants.TEST_USER_ID));
            
            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestReactivateUserError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.UserResources.ReactivateUser(CommonTestConstants.TEST_USER_ID));
            
            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }
    }
}
