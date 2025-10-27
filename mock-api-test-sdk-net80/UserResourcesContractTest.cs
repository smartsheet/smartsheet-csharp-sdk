using System.Web;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class UserResourcesContractTest
    {
        [TestMethod]
        public async Task TestListUserPlansGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeader(new Dictionary<string, string> { { "x-test-name", "/users/list-user-plans/all-response-body-properties" }, { "x-request-id", requestId.ToString() } });
            
            long userId = 12345678L;
            String lastKey = "abcDefGhIjKlMnOpQrStUvWxYz";
            long maxItems = 100L;

            ss.UserResources.ListUserPlans(userId, lastKey, maxItems);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual($"/2.0/users/{userId}/plans", path);
            Assert.AreEqual(maxItems.ToString(), queryParams["maxItems"]);
            Assert.AreEqual(lastKey, queryParams["lastKey"]);
        }

        [TestMethod]
        public void TestListUserPlansAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeader(new Dictionary<string, string> { { "x-test-name", "/users/list-user-plans/all-response-body-properties" }, { "x-request-id", requestId.ToString() } });

            long userId = 12345678L;
            String lastKey = "abcDefGhIjKlMnOpQrStUvWxYz";
            long maxItems = 100L;

            TokenPaginatedResult<UserPlan> response = ss.UserResources.ListUserPlans(userId, lastKey, maxItems);

            Assert.IsNotNull(response);
            Assert.AreEqual("12345678901234569", response.LastKey);
            Assert.AreEqual(1234567890123456L, response.Data[0].PlanId);
            Assert.AreEqual(SeatType.MEMBER, response.Data[0].SeatType);
            Assert.AreEqual(DateTime.Parse("2025-01-01T00:00:00.123456789Z", null, System.Globalization.DateTimeStyles.RoundtripKind), response.Data[0].SeatTypeLastChangedAt);
            Assert.AreEqual(DateTime.Parse("2026-12-13T12:17:52.525696Z", null, System.Globalization.DateTimeStyles.RoundtripKind), response.Data[0].ProvisionalExpirationDate);
            Assert.IsFalse(response.Data[0].IsInternal);
        }

        [TestMethod]
        public void TestListUserPlansRequiredResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeader(new Dictionary<string, string> { { "x-test-name", "/users/list-user-plans/required-response-body-properties" }, { "x-request-id", requestId.ToString() } });

            long userId = 12345678L;

            TokenPaginatedResult<UserPlan> response = ss.UserResources.ListUserPlans(userId, null, null);

            Assert.IsNotNull(response);
            Assert.AreEqual(1234567890123456L, response.Data[0].PlanId);
            Assert.AreEqual(SeatType.MEMBER, response.Data[0].SeatType);
            Assert.IsNull(response.Data[0].SeatTypeLastChangedAt);
            Assert.IsNull(response.Data[0].ProvisionalExpirationDate);
            Assert.IsFalse(response.Data[0].IsInternal);
        }

        [TestMethod]
        public void TestListUserPlansError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeader(new Dictionary<string, string> { { "x-test-name", "/errors/500-response" }, { "x-request-id", requestId.ToString() } });

            long userId = 12345678L;

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.UserResources.ListUserPlans(userId, null, null));
            Assert.AreEqual("Internal Server Error", exception.Message);
        }

        [TestMethod]
        public void TestListUserPlansError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeader(new Dictionary<string, string> { { "x-test-name", "/errors/400-response" }, { "x-request-id", requestId.ToString() } });

            long userId = 12345678L;

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.UserResources.ListUserPlans(userId, null, null));
            Assert.AreEqual("Malformed Request", exception.Message);
        }

        [TestMethod]
        public async Task TestListUsersGeneratedUrlIsCorrectIncludeAllTrue()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeader(new Dictionary<string, string> { { "x-test-name", "/users/list-users/required-response-body-properties" }, { "x-request-id", requestId.ToString() } });

            List<String> emails = new List<String> { "test.user@smartsheet.com" };
            long planId = 1234567890123456L;
            SeatType seatType = SeatType.MEMBER;
            bool includeAll = true;

            PaginationParameters pagination = new PaginationParameters(includeAll, null, null);

            ss.UserResources.ListUsers(emails, planId, seatType, pagination);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual($"/2.0/users", path);
            Assert.AreEqual(seatType.ToString(), queryParams["seatType"]);
            Assert.IsNull(queryParams["page"]);
            Assert.IsNull(queryParams["pageSize"]);
            Assert.IsTrue(bool.Parse(queryParams["includeAll"]));
            Assert.AreEqual(emails.Contains(queryParams["email"]), true);
        }

        [TestMethod]
        public async Task TestListUsersGeneratedUrlIsCorrectIncludeAllFalse()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeader(new Dictionary<string, string> { { "x-test-name", "/users/list-users/required-response-body-properties" }, { "x-request-id", requestId.ToString() } });

            List<String> emails = new List<String> { "test.user@smartsheet.com" };
            long planId = 1234567890123456L;
            SeatType seatType = SeatType.MEMBER;
            bool includeAll = false;
            int page = 1;
            int pageSize = 100;

            PaginationParameters pagination = new PaginationParameters(includeAll, pageSize, page);

            ss.UserResources.ListUsers(emails, planId, seatType, pagination);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual($"/2.0/users", path);
            Assert.AreEqual(seatType.ToString(), queryParams["seatType"]);
            Assert.AreEqual(page.ToString(), queryParams["page"]);
            Assert.AreEqual(pageSize.ToString(), queryParams["pageSize"]);
            Assert.IsFalse(bool.Parse(queryParams["includeAll"]));
            Assert.AreEqual(emails.Contains(queryParams["email"]), true);
        }

        [TestMethod]
        public async Task TestListUsersAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeader(new Dictionary<string, string> { { "x-test-name", "/users/list-users/all-response-body-properties" }, { "x-request-id", requestId.ToString() } });

            long planId = 1234567890123456;

            PaginatedResult<User> response = ss.UserResources.ListUsers(null, planId, null, null);

            Assert.IsNotNull(response);
            Assert.AreEqual(SeatType.MEMBER, response.Data[0].SeatType);
            Assert.AreEqual(DateTime.Parse("2025-06-14T09:55:30Z", null, System.Globalization.DateTimeStyles.RoundtripKind), response.Data[0].SeatTypeLastChangedAt);
            Assert.AreEqual(DateTime.Parse("2026-12-13T12:17:52.525696Z", null, System.Globalization.DateTimeStyles.RoundtripKind), response.Data[0].ProvisionalExpirationDate);
            Assert.IsTrue(response.Data[0].IsInternal);
            Assert.AreEqual("Test", response.Data[0].FirstName);
            Assert.AreEqual("User", response.Data[0].LastName);
            Assert.AreEqual("Test User", response.Data[0].Name);
            Assert.AreEqual("test.user@smartsheet.com", response.Data[0].Email);
            Assert.IsTrue(response.Data[0].Admin);
            Assert.IsTrue(response.Data[0].GroupAdmin);
            Assert.IsTrue(response.Data[0].ResourceViewer);
            Assert.IsTrue(response.Data[0].LicensedSheetCreator);
            Assert.AreEqual(UserStatus.ACTIVE, response.Data[0].Status);
            Assert.AreEqual(-1, response.Data[0].SheetCount);
            Assert.AreEqual(DateTime.Parse("2020-10-04T18:32:47Z", null, System.Globalization.DateTimeStyles.RoundtripKind), response.Data[0].LastLogin);
            Assert.AreEqual(DateTime.Parse("2020-08-25T12:15:47Z", null, System.Globalization.DateTimeStyles.RoundtripKind), response.Data[0].CustomWelcomeScreenViewed);
            Assert.AreEqual(1234567890123456L, response.Data[0].Id);
        }

        [TestMethod]
        public async Task TestListUsersRequiredResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeader(new Dictionary<string, string> { { "x-test-name", "/users/list-users/required-response-body-properties" }, { "x-request-id", requestId.ToString() } });

            long planId = 1234567890123456;

            PaginatedResult<User> response = ss.UserResources.ListUsers(null, planId, null, null);

            Assert.IsNotNull(response);
            Assert.AreEqual(SeatType.MEMBER, response.Data[0].SeatType);
            Assert.IsNull(response.Data[0].SeatTypeLastChangedAt);
            Assert.IsTrue(response.Data[0].IsInternal);
            Assert.AreEqual("Test", response.Data[0].FirstName);
            Assert.AreEqual("User", response.Data[0].LastName);
            Assert.AreEqual("Test User", response.Data[0].Name);
            Assert.AreEqual("test.user@smartsheet.com", response.Data[0].Email);
            Assert.IsTrue(response.Data[0].Admin);
            Assert.IsTrue(response.Data[0].GroupAdmin);
            Assert.IsTrue(response.Data[0].ResourceViewer);
            Assert.IsTrue(response.Data[0].LicensedSheetCreator);
            Assert.AreEqual(UserStatus.ACTIVE, response.Data[0].Status);
            Assert.AreEqual(-1, response.Data[0].SheetCount);
            Assert.AreEqual(1234567890123456L, response.Data[0].Id);
        }

        [TestMethod]
        public void TestListUsersError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeader(new Dictionary<string, string> { { "x-test-name", "/errors/500-response" }, { "x-request-id", requestId.ToString() } });

            long planId = 1234567890123456;

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.UserResources.ListUsers(null, planId, null, null));
            Assert.AreEqual("Internal Server Error", exception.Message);
        }

        [TestMethod]
        public void TestListUsersError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClientWithCustomHeader(new Dictionary<string, string> { { "x-test-name", "/errors/400-response" }, { "x-request-id", requestId.ToString() } });

            long planId = 1234567890123456;

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.UserResources.ListUsers(null, planId, null, null));
            Assert.AreEqual("Malformed Request", exception.Message);
        }
    }
}