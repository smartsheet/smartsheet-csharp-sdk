using System.Web;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestListUserPlans
    {
        private const string TEST_LAST_KEY = "12345678901234569";
        private const long TEST_MAX_ITEMS = 100L;
        private const SeatType TEST_SEAT_TYPE = SeatType.MEMBER;
        private static readonly DateTime TEST_SEAT_TYPE_LAST_CHANGED_AT = DateTime.Parse("2025-01-01T00:00:00.123456789Z", null, System.Globalization.DateTimeStyles.RoundtripKind);
        private static readonly DateTime TEST_PROVISIONAL_EXPIRATION_DATE = DateTime.Parse("2026-12-13T12:17:52.525696Z", null, System.Globalization.DateTimeStyles.RoundtripKind);

        [TestMethod]
        public async Task TestListUserPlansGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/users/list-user-plans/all-response-body-properties", requestId.ToString());
            
            ss.UserResources.ListUserPlans(CommonTestConstants.TEST_USER_ID, TEST_LAST_KEY, TEST_MAX_ITEMS);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual($"/2.0/users/{CommonTestConstants.TEST_USER_ID}/plans", path);
            Assert.AreEqual(TEST_MAX_ITEMS.ToString(), queryParams["maxItems"]);
            Assert.AreEqual(TEST_LAST_KEY, queryParams["lastKey"]);
        }

        [TestMethod]
        public void TestListUserPlansAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/users/list-user-plans/all-response-body-properties", requestId.ToString());

            TokenPaginatedResult<UserPlan> response = ss.UserResources.ListUserPlans(CommonTestConstants.TEST_USER_ID, TEST_LAST_KEY, TEST_MAX_ITEMS);

            Assert.IsNotNull(response);
            Assert.AreEqual(TEST_LAST_KEY, response.LastKey);
            Assert.AreEqual(CommonTestConstants.TEST_PLAN_ID, response.Data[0].PlanId);
            Assert.AreEqual(TEST_SEAT_TYPE, response.Data[0].SeatType);
            Assert.AreEqual(TEST_SEAT_TYPE_LAST_CHANGED_AT, response.Data[0].SeatTypeLastChangedAt);
            Assert.AreEqual(TEST_PROVISIONAL_EXPIRATION_DATE, response.Data[0].ProvisionalExpirationDate);
            Assert.IsFalse(response.Data[0].IsInternal);
        }

        [TestMethod]
        public void TestListUserPlansRequiredResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/users/list-user-plans/required-response-body-properties", requestId.ToString());

            TokenPaginatedResult<UserPlan> response = ss.UserResources.ListUserPlans(CommonTestConstants.TEST_USER_ID, null, null);

            Assert.IsNotNull(response);
            Assert.AreEqual(CommonTestConstants.TEST_PLAN_ID, response.Data[0].PlanId);
            Assert.AreEqual(TEST_SEAT_TYPE, response.Data[0].SeatType);
            Assert.IsNull(response.Data[0].SeatTypeLastChangedAt);
            Assert.IsNull(response.Data[0].ProvisionalExpirationDate);
            Assert.IsFalse(response.Data[0].IsInternal);
        }

        [TestMethod]
        public void TestListUserPlansError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.UserResources.ListUserPlans(CommonTestConstants.TEST_USER_ID, null, null));
            Assert.AreEqual("Internal Server Error", exception.Message);
        }

        [TestMethod]
        public void TestListUserPlansError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.UserResources.ListUserPlans(CommonTestConstants.TEST_USER_ID, null, null));
            Assert.AreEqual("Malformed Request", exception.Message);
        }
    }
}