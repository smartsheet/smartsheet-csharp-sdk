using Newtonsoft.Json;

namespace mock_api_test_sdk_net80
{
    public class LogModel
    {
        [JsonProperty("url")]
        public string? Url { get; set; }

        [JsonProperty("absoluteUrl")]
        public string? AbsoluteUrl { get; set; }

        [JsonProperty("method")]
        public string? Method { get; set; }

        [JsonProperty("clientIp")]
        public string? ClientIp { get; set; }

        [JsonProperty("headers")]
        public Dictionary<string, string>? Headers { get; set; }

        [JsonProperty("cookies")]
        public Dictionary<string, string>? Cookies { get; set; }

        [JsonProperty("queryParams")]
        public Dictionary<string, QueryParam>? QueryParams { get; set; }

        [JsonProperty("formParams")]
        public Dictionary<string, string>? FormParams { get; set; }

        [JsonProperty("loggedDate")]
        public long LoggedDate { get; set; }

        [JsonProperty("loggedDateString")]
        public string? LoggedDateString { get; set; }
    }
}