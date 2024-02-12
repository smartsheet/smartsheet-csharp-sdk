namespace mock_api_test_sdk_net80
{
    using Smartsheet.Api.Internal.Http;
    using RestSharp;

    class TestHttpClient : DefaultHttpClient
    {
        private string apiScenario;

        public TestHttpClient(string apiScenario)
        {
            this.apiScenario = apiScenario;
        }

        public override RestRequest CreateRestRequest(HttpRequest smartsheetRequest)
        {
            RestRequest restRequest = base.CreateRestRequest(smartsheetRequest);
            restRequest.AddHeader("Api-Scenario", this.apiScenario);
            return restRequest;
        }
    }
}
