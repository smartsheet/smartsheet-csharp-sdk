using System.Web;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestRemoveUserFromPlan
    {
        [TestMethod]
        public async Task TestRemoveUserFromPlanGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/users/remove-user-from-plan/all-response-body-properties", requestId.ToString());

            smartsheet.UserResources.RemoveUserFromPlan(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual($"/2.0/users/{CommonTestConstants.TEST_USER_ID}/plans/{CommonTestConstants.TEST_PLAN_ID}", path);
            Assert.AreEqual("DELETE", foundRequest.Method);
        }

        [TestMethod]
        public void TestRemoveUserFromPlanAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/users/remove-user-from-plan/all-response-body-properties", requestId.ToString());

            // If this throws an exception, the test will fail automatically
            smartsheet.UserResources.RemoveUserFromPlan(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID);
        }

        [TestMethod]
        public void TestRemoveUserFromPlanError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.UserResources.RemoveUserFromPlan(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID));
            
            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestRemoveUserFromPlanError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.UserResources.RemoveUserFromPlan(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID));
            
            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }
    }
}