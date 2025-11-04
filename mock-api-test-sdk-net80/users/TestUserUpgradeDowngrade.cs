using System.Web;
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
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> {
                { "x-test-name", "/users/upgrade-user/all-response-body-properties" },
                { "x-request-id", requestId.ToString() }
            });

            ss.UserResources.UpgradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, TEST_UPGRADE_SEAT_TYPE);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual($"/2.0/users/{CommonTestConstants.TEST_USER_ID}/plans/{CommonTestConstants.TEST_PLAN_ID}/upgrade", path);
            Assert.AreEqual("POST", foundRequest.Method);
        }

        [TestMethod]
        public void TestUpgradeUserAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> {
                { "x-test-name", "/users/upgrade-user/all-response-body-properties" },
                { "x-request-id", requestId.ToString() }
            });

            // If this throws an exception, the test will fail automatically
            ss.UserResources.UpgradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, TEST_UPGRADE_SEAT_TYPE);
        }

        [TestMethod]
        public void TestUpgradeUserNoSeatType()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> {
                { "x-test-name", "/users/upgrade-user/all-response-body-properties" },
                { "x-request-id", requestId.ToString() }
            });

            // If this throws an exception, the test will fail automatically
            ss.UserResources.UpgradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, null);
        }

        [TestMethod]
        public void TestUpgradeUserError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> {
                { "x-test-name", "/errors/500-response" },
                { "x-request-id", requestId.ToString() }
            });

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                ss.UserResources.UpgradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, TEST_UPGRADE_SEAT_TYPE));
            
            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestUpgradeUserError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> {
                { "x-test-name", "/errors/400-response" },
                { "x-request-id", requestId.ToString() }
            });

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                ss.UserResources.UpgradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, TEST_UPGRADE_SEAT_TYPE));
            
            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }

        [TestMethod]
        public async Task TestDowngradeUserGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> {
                { "x-test-name", "/users/downgrade-user/all-response-body-properties" },
                { "x-request-id", requestId.ToString() }
            });

            ss.UserResources.DowngradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, TEST_DOWNGRADE_SEAT_TYPE);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            Assert.AreEqual($"/2.0/users/{CommonTestConstants.TEST_USER_ID}/plans/{CommonTestConstants.TEST_PLAN_ID}/downgrade", path);
            Assert.AreEqual("POST", foundRequest.Method);
        }

        [TestMethod]
        public void TestDowngradeUserAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> {
                { "x-test-name", "/users/downgrade-user/all-response-body-properties" },
                { "x-request-id", requestId.ToString() }
            });

            // If this throws an exception, the test will fail automatically
            ss.UserResources.DowngradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, TEST_DOWNGRADE_SEAT_TYPE);
        }

        [TestMethod]
        public void TestDowngradeUserError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> {
                { "x-test-name", "/errors/500-response" },
                { "x-request-id", requestId.ToString() }
            });

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                ss.UserResources.DowngradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, TEST_DOWNGRADE_SEAT_TYPE));
            
            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestDowngradeUserError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeaders(new Dictionary<string, string> {
                { "x-test-name", "/errors/400-response" },
                { "x-request-id", requestId.ToString() }
            });

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                ss.UserResources.DowngradeUser(CommonTestConstants.TEST_USER_ID, CommonTestConstants.TEST_PLAN_ID, TEST_DOWNGRADE_SEAT_TYPE));
            
            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }
    }
}