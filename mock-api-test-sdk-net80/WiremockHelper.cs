using System.Text;
using Newtonsoft.Json;

namespace mock_api_test_sdk_net80
{
    public class WiremockHelper
    {
        internal class WireMockRequestsWrapper
        {
            [JsonProperty("requests")]
            public List<LogModel>? Requests { get; set; }

            [JsonProperty("requestJournalDisabled")]
            public bool RequestJournalDisabled { get; set; }
        }
    
        private static readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:8082")
        };

        public WiremockHelper()
        {
        }

        public async Task<LogModel> FindWiremockRequestAsync(string requestId)
        {
            var requestBody = new
            {
                headers = new Dictionary<string, object>
                {
                    {
                        "x-request-id", new
                        {
                            equalTo = requestId
                        }
                    }
                }
            };

            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/__admin/requests/find", httpContent);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var wrapper = JsonConvert.DeserializeObject<WireMockRequestsWrapper>(json);

            if (wrapper?.Requests == null || !wrapper.Requests.Any())
            {
                throw new InvalidOperationException("No requests found in the WireMock response.");
            }

            var matchingRequests = wrapper.Requests.ToList();

            if (!matchingRequests.Any())
            {
                throw new ArgumentException($"No requests found with ID: {requestId}");
            }
            if (matchingRequests.Count > 1)
            {
                throw new ArgumentException($"Multiple requests found with ID: {requestId}");
            }

            return matchingRequests.First();
        }
    }
}