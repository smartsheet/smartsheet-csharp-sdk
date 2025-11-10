using System.Web;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestListUsers
    {
        private const string TEST_EMAIL = "test.user@smartsheet.com";
        private const string TEST_FIRST_NAME = "Test";
        private const string TEST_LAST_NAME = "User";
        private const string TEST_NAME = "Test User";
        private const int TEST_PAGE = 1;
        private const int TEST_PAGE_SIZE = 100;
        private const bool TEST_INCLUDE_ALL = false;
        private static readonly List<string> TEST_EMAILS = new List<string> { TEST_EMAIL };
        private const SeatType TEST_SEAT_TYPE = SeatType.MEMBER;
        private const int TEST_SHEET_COUNT = -1;
        private const UserStatus TEST_USER_STATUS = UserStatus.ACTIVE;
        private static readonly DateTime TEST_SEAT_TYPE_LAST_CHANGED_AT = DateTime.Parse("2025-06-14T09:55:30Z", null, System.Globalization.DateTimeStyles.RoundtripKind);
        private static readonly DateTime TEST_PROVISIONAL_EXPIRATION_DATE = DateTime.Parse("2026-12-13T12:17:52.525696Z", null, System.Globalization.DateTimeStyles.RoundtripKind);
        private static readonly DateTime TEST_LAST_LOGIN = DateTime.Parse("2020-10-04T18:32:47Z", null, System.Globalization.DateTimeStyles.RoundtripKind);
        private static readonly DateTime TEST_CUSTOM_WELCOME_SCREEN_VIEWED = DateTime.Parse("2020-08-25T12:15:47Z", null, System.Globalization.DateTimeStyles.RoundtripKind);

        [TestMethod]
        public async Task TestListUsersGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/users/list-users/required-response-body-properties", requestId.ToString());

            PaginationParameters pagination = new PaginationParameters(TEST_INCLUDE_ALL, TEST_PAGE_SIZE, TEST_PAGE);

            ss.UserResources.ListUsers(TEST_EMAILS, CommonTestConstants.TEST_PLAN_ID, TEST_SEAT_TYPE, pagination);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl);
            string path = uri.AbsolutePath;

            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual($"/2.0/users", path);
            Assert.AreEqual(TEST_SEAT_TYPE.ToString(), queryParams["seatType"]);
            Assert.AreEqual(TEST_PAGE.ToString(), queryParams["page"]);
            Assert.AreEqual(TEST_PAGE_SIZE.ToString(), queryParams["pageSize"]);
            Assert.IsFalse(bool.Parse(queryParams["includeAll"]));
            Assert.AreEqual(TEST_EMAILS.Contains(queryParams["email"]), true);
        }

        [TestMethod]
        public async Task TestListUsersAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/users/list-users/all-response-body-properties", requestId.ToString());

            PaginatedResult<User> response = ss.UserResources.ListUsers(null, CommonTestConstants.TEST_PLAN_ID, null, null);

            Assert.IsNotNull(response);
            Assert.AreEqual(TEST_SEAT_TYPE, response.Data[0].SeatType);
            Assert.AreEqual(TEST_SEAT_TYPE_LAST_CHANGED_AT, response.Data[0].SeatTypeLastChangedAt);
            Assert.AreEqual(TEST_PROVISIONAL_EXPIRATION_DATE, response.Data[0].ProvisionalExpirationDate);
            Assert.IsTrue(response.Data[0].IsInternal);
            Assert.AreEqual(TEST_FIRST_NAME, response.Data[0].FirstName);
            Assert.AreEqual(TEST_LAST_NAME, response.Data[0].LastName);
            Assert.AreEqual(TEST_NAME, response.Data[0].Name);
            Assert.AreEqual(TEST_EMAIL, response.Data[0].Email);
            Assert.IsTrue(response.Data[0].Admin);
            Assert.IsTrue(response.Data[0].GroupAdmin);
            Assert.IsTrue(response.Data[0].ResourceViewer);
            Assert.IsTrue(response.Data[0].LicensedSheetCreator);
            Assert.AreEqual(TEST_USER_STATUS, response.Data[0].Status);
            Assert.AreEqual(TEST_SHEET_COUNT, response.Data[0].SheetCount);
            Assert.AreEqual(TEST_LAST_LOGIN, response.Data[0].LastLogin);
            Assert.AreEqual(TEST_CUSTOM_WELCOME_SCREEN_VIEWED, response.Data[0].CustomWelcomeScreenViewed);
            Assert.AreEqual(CommonTestConstants.TEST_PLAN_ID, response.Data[0].Id);
        }

        [TestMethod]
        public async Task TestListUsersRequiredResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/users/list-users/required-response-body-properties", requestId.ToString());

            PaginatedResult<User> response = ss.UserResources.ListUsers(null, CommonTestConstants.TEST_PLAN_ID, null, null);

            Assert.IsNotNull(response);
            Assert.AreEqual(TEST_SEAT_TYPE, response.Data[0].SeatType);
            Assert.IsNull(response.Data[0].SeatTypeLastChangedAt);
            Assert.IsTrue(response.Data[0].IsInternal);
            Assert.AreEqual(TEST_FIRST_NAME, response.Data[0].FirstName);
            Assert.AreEqual(TEST_LAST_NAME, response.Data[0].LastName);
            Assert.AreEqual(TEST_NAME, response.Data[0].Name);
            Assert.AreEqual(TEST_EMAIL, response.Data[0].Email);
            Assert.IsTrue(response.Data[0].Admin);
            Assert.IsTrue(response.Data[0].GroupAdmin);
            Assert.IsTrue(response.Data[0].ResourceViewer);
            Assert.IsTrue(response.Data[0].LicensedSheetCreator);
            Assert.AreEqual(TEST_USER_STATUS, response.Data[0].Status);
            Assert.AreEqual(TEST_SHEET_COUNT, response.Data[0].SheetCount);
            Assert.AreEqual(CommonTestConstants.TEST_PLAN_ID, response.Data[0].Id);
        }

        [TestMethod]
        public void TestListUsersError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.UserResources.ListUsers(null, CommonTestConstants.TEST_PLAN_ID, null, null));
            Assert.AreEqual("Internal Server Error", exception.Message);
        }

        [TestMethod]
        public void TestListUsersError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient ss = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() => ss.UserResources.ListUsers(null, CommonTestConstants.TEST_PLAN_ID, null, null));
            Assert.AreEqual("Malformed Request", exception.Message);
        }
    }
}