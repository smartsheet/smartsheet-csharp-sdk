using Newtonsoft.Json;

namespace mock_api_test_sdk_net80
{
    public class QueryParam
    {
        [JsonProperty("key")]
        public string? Key { get; set; }

        [JsonProperty("values")]
        public List<string>? Values { get; set; }
    }
}