using Newtonsoft.Json;
using System.Globalization;
using System.Web;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestListEvents
    {
        private const string NEXT_STREAM_POSITION = "2.1.Y-oF8RMroSCo4WLS9p78QEz-LXzzzzzzjnXA2hFnCN_w";
        private const string SINCE_STRING = "2024-05-06T00:00:00Z";
        private static readonly DateTime SINCE = DateTime.Parse(SINCE_STRING, null, DateTimeStyles.RoundtripKind);

        private static readonly EventResult EXPECTED_ALL_RESPONSE = new EventResult
        {
            MoreAvailable = true,
            NextStreamPosition = NEXT_STREAM_POSITION,
            Data = new List<Event>
            {
                new Event
                {
                    EventId = "4f12345678901234",
                    ObjectType = EventObjectType.SHEET,
                    ObjectId = 1234567890123456L,
                    ObjectIdStr = "1234567890123456",
                    UserId = 12345678L,
                    RequestUserId = 12345678L,
                    EventTimestamp = "2024-05-06T10:30:00Z",
                    Action = EventAction.UPDATE,
                    Source = EventSource.WEB_APP,
                    AdditionalDetails = new Dictionary<string, object> { { "emailAddress", "test@test.com" } }
                },
                new Event
                {
                    EventId = "4f12345678901235",
                    ObjectType = EventObjectType.WORKSPACE,
                    ObjectId = 9876543210987654L,
                    ObjectIdStr = "9876543210987654",
                    UserId = 12345679L,
                    RequestUserId = 12345679L,
                    EventTimestamp = "2024-05-06T09:15:00Z",
                    Action = EventAction.CREATE,
                    Source = EventSource.API_UNDEFINED_APP,
                    AdditionalDetails = new Dictionary<string, object> { { "emailAddress", "test@test.com" } }
                },
                new Event
                {
                    EventId = "2.1.Y-oF8RMroSCo4WLS9p78QEz-LXxxxyyyjnXA2hFnCN_w",
                    ObjectType = EventObjectType.SHEET,
                    Action = EventAction.PURGE,
                    ObjectId = 3573510329814916L,
                    ObjectIdStr = "3573510329814916",
                    EventTimestamp = "2024-05-06T09:09:33Z",
                    UserId = 12345678L,
                    RequestUserId = 12345678L,
                    Source = EventSource.UNKNOWN,
                    AdditionalDetails = new Dictionary<string, object> { { "emailAddress", "test@test.com" } }
                },
                new Event
                {
                    EventId = "2.1.SuwpcrfUcr75nP591Hce4_zQxxxyyy_0CDfvRRx6V0",
                    ObjectType = EventObjectType.ATTACHMENT,
                    Action = EventAction.CREATE,
                    ObjectId = 4230707048648580L,
                    ObjectIdStr = "4230707048648580",
                    EventTimestamp = "2024-05-06T10:30:00Z",
                    UserId = 12345678L,
                    RequestUserId = 12345678L,
                    Source = EventSource.UNKNOWN,
                    AdditionalDetails = new Dictionary<string, object>
                    {
                        { "emailAddress", "test@test.com" },
                        { "sheetId", "102030405" },
                        { "attachmentName", "picture.jpg" }
                    }
                },
                new Event
                {
                    EventId = "2.1.ifR6WlBin9DQVYHDkQEx1D3EAxxxyyyXtcLWa9Oio",
                    ObjectType = EventObjectType.SHEET,
                    Action = EventAction.LOAD,
                    ObjectId = 8462951303303044L,
                    ObjectIdStr = "8462951303303044",
                    EventTimestamp = "2024-05-06T10:35:15Z",
                    UserId = 8737233684457348L,
                    RequestUserId = 8737233684457348L,
                    Source = EventSource.API_INTEGRATED_APP,
                    AdditionalDetails = new Dictionary<string, object> { { "emailAddress", "test@test.com" } }
                }
            }
        };

        private static readonly EventResult EXPECTED_REQUIRED_RESPONSE = new EventResult
        {
            MoreAvailable = false,
            Data = new List<Event>
            {
                new Event
                {
                    EventId = "4f12345678901234",
                    ObjectType = EventObjectType.SHEET,
                    ObjectId = 1234567890123456L,
                    UserId = 12345678L,
                    RequestUserId = 12345678L,
                    EventTimestamp = "2024-05-06T10:30:00Z",
                    Action = EventAction.UPDATE,
                    Source = EventSource.WEB_APP
                },
                new Event
                {
                    EventId = "4f12345678901235",
                    ObjectType = EventObjectType.WORKSPACE,
                    ObjectId = 9876543210987654L,
                    UserId = 12345679L,
                    RequestUserId = 12345679L,
                    EventTimestamp = "2024-05-06T09:15:00Z",
                    Action = EventAction.CREATE,
                    Source = EventSource.API_UNDEFINED_APP
                },
                new Event
                {
                    EventId = "2.1.Y-oF8RMroSCo4WLS9p78QEz-LXxxxyyyjnXA2hFnCN_w",
                    ObjectType = EventObjectType.SHEET,
                    Action = EventAction.PURGE,
                    ObjectId = 3573510329814916L,
                    EventTimestamp = "2024-05-06T09:09:33Z",
                    UserId = 12345678L,
                    RequestUserId = 12345678L,
                    Source = EventSource.UNKNOWN
                },
                new Event
                {
                    EventId = "2.1.SuwpcrfUcr75nP591Hce4_zQxxxyyy_0CDfvRRx6V0",
                    ObjectType = EventObjectType.ATTACHMENT,
                    Action = EventAction.CREATE,
                    ObjectId = 4230707048648580L,
                    EventTimestamp = "2024-05-06T10:30:00Z",
                    UserId = 12345678L,
                    RequestUserId = 12345678L,
                    Source = EventSource.UNKNOWN
                },
                new Event
                {
                    EventId = "2.1.ifR6WlBin9DQVYHDkQEx1D3EAxxxyyyXtcLWa9Oio",
                    ObjectType = EventObjectType.SHEET,
                    Action = EventAction.LOAD,
                    ObjectId = 8462951303303044L,
                    EventTimestamp = "2024-05-06T10:35:15Z",
                    UserId = 8737233684457348L,
                    RequestUserId = 8737233684457348L,
                    Source = EventSource.API_INTEGRATED_APP
                }
            }
        };

        [TestMethod]
        public async Task TestListEventsGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/events/list-events/all-response-body-properties", requestId.ToString());

            smartsheet.EventResources.ListEvents(SINCE, NEXT_STREAM_POSITION, 100, false);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsNotNull(foundRequest.AbsoluteUrl);
            var uri = new Uri(foundRequest.AbsoluteUrl);

            Assert.AreEqual("/2.0/events", uri.AbsolutePath);
            Assert.AreEqual("GET", foundRequest.Method);

            var queryParams = HttpUtility.ParseQueryString(uri.Query);
            CollectionAssert.AreEquivalent(
                new Dictionary<string, string>
                {
                    { "since", SINCE_STRING },
                    { "streamPosition", NEXT_STREAM_POSITION },
                    { "maxCount", "100" },
                    { "numericDates", "False" }
                },
                queryParams.AllKeys.ToDictionary(k => k, k => queryParams[k])
            );
        }

        [TestMethod]
        public async Task TestListEventsAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/events/list-events/all-response-body-properties", requestId.ToString());

            EventResult result = smartsheet.EventResources.ListEvents(null, null, null, null);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsTrue(string.IsNullOrEmpty(foundRequest.Body));
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_ALL_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public async Task TestListEventsRequiredResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/events/list-events/required-response-body-properties", requestId.ToString());

            EventResult result = smartsheet.EventResources.ListEvents(null, null, null, null);

            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.IsTrue(string.IsNullOrEmpty(foundRequest.Body));
            Assert.AreEqual(JsonConvert.SerializeObject(EXPECTED_REQUIRED_RESPONSE), JsonConvert.SerializeObject(result));
        }

        [TestMethod]
        public void TestListEventsError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.EventResources.ListEvents(null, null, null, null)
            );

            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }

        [TestMethod]
        public void TestListEventsError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.EventResources.ListEvents(null, null, null, null)
            );

            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }
    }
}
