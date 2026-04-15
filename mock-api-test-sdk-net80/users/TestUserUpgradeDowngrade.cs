using System.Web;
using Newtonsoft.Json;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestUserUpgradeDowngrade
    {
        private const UpgradeSeatType TEST_UPGRADE_SEAT_TYPE = UpgradeSeatType.MEMBER;
        private const DowngradeSeatType TEST_DOWNGRADE_SEAT_TYPE = DowngradeSeatType.VIEWER;

        [TestMethod]
        public async Task TestUpgradeUserGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/users/upgrade-user/all-response-body-properties", requestId.ToString());

            smartsheet.UserResources.UpgradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, TEST_UPGRADE_SEAT_TYPE);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual($"/2.0/users/{CommonTestConstants.TEST_USER_ID}/plans/{CommonTestConstants.TEST_PLAN_ID}/upgrade", path);
            Assert.AreEqual("POST", foundRequest.Method);
        }

        [TestMethod]
        public async Task TestUpgradeUserAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/users/upgrade-user/all-response-body-properties", requestId.ToString());

            // If this throws an exception, the test will fail automatically
            smartsheet.UserResources.UpgradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, TEST_UPGRADE_SEAT_TYPE);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var expectedBodyMap = new Dictionary<string, string> { { "seatType", "MEMBER" } };
            var actualBodyMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(foundRequest.Body ?? "{}");
            CollectionAssert.AreEquivalent(expectedBodyMap, actualBodyMap);
        }

        [TestMethod]
        public void TestUpgradeUserNoSeatType()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/users/upgrade-user/all-response-body-properties", requestId.ToString());

            // If this throws an exception, the test will fail automatically
            smartsheet.UserResources.UpgradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, null);
        }

        [TestMethod]
        public void TestUpgradeUserError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.UserResources.UpgradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, TEST_UPGRADE_SEAT_TYPE));
            
            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestUpgradeUserError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.UserResources.UpgradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, TEST_UPGRADE_SEAT_TYPE));
            
            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }

        [TestMethod]
        public async Task TestDowngradeUserGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/users/downgrade-user/all-response-body-properties", requestId.ToString());

            smartsheet.UserResources.DowngradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, TEST_DOWNGRADE_SEAT_TYPE);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual($"/2.0/users/{CommonTestConstants.TEST_USER_ID}/plans/{CommonTestConstants.TEST_PLAN_ID}/downgrade", path);
            Assert.AreEqual("POST", foundRequest.Method);
        }

        [TestMethod]
        public async Task TestDowngradeUserAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/users/downgrade-user/all-response-body-properties", requestId.ToString());

            // If this throws an exception, the test will fail automatically
            smartsheet.UserResources.DowngradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, TEST_DOWNGRADE_SEAT_TYPE);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var expectedBodyMap = new Dictionary<string, string> { { "seatType", "VIEWER" } };
            var actualBodyMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(foundRequest.Body ?? "{}");
            CollectionAssert.AreEquivalent(expectedBodyMap, actualBodyMap);
        }

        [TestMethod]
        public void TestDowngradeUserError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.UserResources.DowngradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, TEST_DOWNGRADE_SEAT_TYPE));
            
            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestDowngradeUserError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.UserResources.DowngradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, TEST_DOWNGRADE_SEAT_TYPE));

            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }

        [TestMethod]
        public async Task TestDowngradeUserToContributorGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/users/downgrade-user/to-contributor", requestId.ToString());

            smartsheet.UserResources.DowngradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, DowngradeSeatType.CONTRIBUTOR);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual($"/2.0/users/{CommonTestConstants.TEST_USER_ID}/plans/{CommonTestConstants.TEST_PLAN_ID}/downgrade", path);
            Assert.AreEqual("POST", foundRequest.Method);
        }

        [TestMethod]
        public async Task TestDowngradeUserToContributorRequestBody()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/users/downgrade-user/to-contributor", requestId.ToString());

            smartsheet.UserResources.DowngradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, DowngradeSeatType.CONTRIBUTOR);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var expectedBodyMap = new Dictionary<string, string> { { "seatType", "CONTRIBUTOR" } };
            var actualBodyMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(foundRequest.Body ?? "{}");
            CollectionAssert.AreEquivalent(expectedBodyMap, actualBodyMap);
        }

        [TestMethod]
        public async Task TestDowngradeUserToContributorResponse()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/users/downgrade-user/to-contributor", requestId.ToString());

            // Should not throw an exception
            smartsheet.UserResources.DowngradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, DowngradeSeatType.CONTRIBUTOR);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            Assert.IsNotNull(foundRequest);
            Assert.AreEqual("POST", foundRequest.Method);
        }
    }
}