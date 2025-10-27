namespace mock_api_test_sdk_net80
{
    using Smartsheet.Api.Internal.Http;
    using RestSharp;

    class TestHttpClientWithCustomHeader : DefaultHttpClient
    {
        private Dictionary<string, string> customHeaders;

        public TestHttpClientWithCustomHeader(Dictionary<string, string> customHeaders)
        {
            this.customHeaders = customHeaders;
        }

        public override RestRequest CreateRestRequest(HttpRequest smartsheetRequest)
        {
            RestRequest restRequest = base.CreateRestRequest(smartsheetRequest);
            restRequest.AddHeaders(this.customHeaders);
            return restRequest;
        }
    }
}
