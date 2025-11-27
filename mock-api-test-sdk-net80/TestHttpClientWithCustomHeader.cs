namespace mock_api_test_sdk_net80
{
    using Smartsheet.Api.Internal.Http;
    using RestSharp;

    class TestHttpClientWithCustomHeader : DefaultHttpClient
    {
        private Dictionary<string, string> customHeaders;

        public TestHttpClientWithCustomHeader(string testName, string requestId)
        {
            this.customHeaders = new Dictionary<string, string>
            {
                { "x-test-name", testName },
                { "x-request-id", requestId }
            };
        }

        public override RestRequest CreateRestRequest(HttpRequest smartsheetRequest)
        {
            RestRequest restRequest = base.CreateRestRequest(smartsheetRequest);
            restRequest.AddHeaders(this.customHeaders);
            return restRequest;
        }
    }
}
