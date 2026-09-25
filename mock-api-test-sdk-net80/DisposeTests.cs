using Smartsheet.Api;
using Smartsheet.Api.Internal.Http;
using Smartsheet.Api.OAuth;
using RestSharp;

namespace mock_api_test_sdk_net80
{
    /// <summary>
    /// Unit tests for deterministic disposal of the SDK client and HTTP layer.
    /// These tests do not require a running WireMock server.
    /// </summary>
    [TestClass]
    public class DisposeTests
    {
        /// <summary>
        /// Builds a DefaultHttpClient over an unroutable port. No request is ever
        /// completed in these tests; only disposal behavior is under test.
        /// </summary>
        private static DefaultHttpClient CreateHttpClient()
        {
            return new DefaultHttpClient(
                new RestClient(new RestClientOptions("http://localhost:9/")),
                new Smartsheet.Api.Internal.Json.JsonNetSerializer());
        }

        [TestMethod]
        public void TestDefaultHttpClientDisposeDisposesRestClient()
        {
            DefaultHttpClient httpClient = CreateHttpClient();

            httpClient.Dispose();

            // A disposed RestSharp RestClient throws ObjectDisposedException from ExecuteAsync.
            HttpRequest request = new HttpRequest();
            request.Uri = new Uri("http://localhost:9/sheets");
            request.Method = Smartsheet.Api.Internal.Http.HttpMethod.GET;

            Assert.ThrowsException<ObjectDisposedException>(() => httpClient.Request(request));
        }

        [TestMethod]
        public void TestDefaultHttpClientDisposeIsIdempotent()
        {
            DisposableTestRestClient restClient = new DisposableTestRestClient();
            DefaultHttpClient httpClient = new DefaultHttpClient(
                restClient, new Smartsheet.Api.Internal.Json.JsonNetSerializer());

            httpClient.Dispose();
            httpClient.Dispose();

            Assert.AreEqual(1, restClient.DisposeCallCount,
                "DefaultHttpClient.Dispose() must be idempotent; the RestClient should be disposed exactly once.");
        }

        /// <summary>
        /// A second RestClient.Dispose() does not throw, so counting calls is the only way to
        /// observe DefaultHttpClient's own guard.
        /// </summary>
        private class DisposableTestRestClient : RestClient
        {
            public DisposableTestRestClient() : base(new RestClientOptions("http://localhost:9/")) { }

            public int DisposeCallCount { get; private set; }

            protected override void Dispose(bool disposing)
            {
                DisposeCallCount++;
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// A custom HTTP client that counts disposals, to prove the SDK disposes an injected
        /// client rather than only its own.
        /// </summary>
        private class DisposableTestHttpClient : DefaultHttpClient
        {
            public int DisposeCallCount { get; private set; }

            protected override void Dispose(bool disposing)
            {
                DisposeCallCount++;
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// A custom HTTP client implementing the interface directly rather than subclassing
        /// DefaultHttpClient, to prove the SDK disposes a third-party implementation.
        /// </summary>
        private class CustomTestHttpClient : Smartsheet.Api.Internal.Http.HttpClient
        {
            public int DisposeCallCount { get; private set; }

            public void Dispose()
            {
                DisposeCallCount++;
            }

            public void ReleaseConnection()
            {
            }

            public HttpResponse Request(HttpRequest request)
            {
                throw new NotImplementedException();
            }

            public HttpResponse Request(HttpRequest request, string objectType, string file, string fileType)
            {
                throw new NotImplementedException();
            }

            public Task<HttpResponse> RequestAsync(HttpRequest request, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<HttpResponse> RequestAsync(HttpRequest request, string objectType, string file,
                string fileType, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void TestSmartsheetClientDisposeDisposesInjectedHttpClient()
        {
            DisposableTestHttpClient httpClient = new DisposableTestHttpClient();
            SmartsheetClient smartsheet = new SmartsheetBuilder()
                .SetBaseURI("http://localhost:9/")
                .SetAccessToken("aaaaaaaaaaaaaaaaaaaaaaaaaa")
                .SetHttpClient(httpClient)
                .Build();

            smartsheet.Dispose();

            Assert.AreEqual(1, httpClient.DisposeCallCount,
                "SmartsheetClient.Dispose() should call the IDisposable path on the injected HTTP client.");
        }

        [TestMethod]
        public void TestSmartsheetClientDisposesCustomHttpClientImplementation()
        {
            CustomTestHttpClient httpClient = new CustomTestHttpClient();
            SmartsheetClient smartsheet = new SmartsheetBuilder()
                .SetBaseURI("http://localhost:9/")
                .SetAccessToken("aaaaaaaaaaaaaaaaaaaaaaaaaa")
                .SetHttpClient(httpClient)
                .Build();

            smartsheet.Dispose();

            Assert.AreEqual(1, httpClient.DisposeCallCount,
                "Dispose() should dispose an HttpClient that implements the interface directly.");
        }

        [TestMethod]
        public void TestSmartsheetClientDisposeDisposesDefaultHttpClient()
        {
            SmartsheetClient smartsheet = new SmartsheetBuilder()
                .SetBaseURI("http://localhost:9/")
                .SetAccessToken("aaaaaaaaaaaaaaaaaaaaaaaaaa")
                .Build();

            smartsheet.Dispose();

            // No SetHttpClient call, so this exercises the transport the SDK builds itself.
            Assert.ThrowsException<ObjectDisposedException>(
                () => smartsheet.SheetResources.GetSheet(1, null, null));
        }

        [TestMethod]
        public void TestSmartsheetClientSupportsUsingStatement()
        {
            DisposableTestHttpClient httpClient = new DisposableTestHttpClient();

            using (SmartsheetClient smartsheet = new SmartsheetBuilder()
                .SetBaseURI("http://localhost:9/")
                .SetAccessToken("aaaaaaaaaaaaaaaaaaaaaaaaaa")
                .SetHttpClient(httpClient)
                .Build())
            {
                Assert.IsNotNull(smartsheet.SheetResources);
            }

            // The using block should have disposed the client exactly once.
            Assert.AreEqual(1, httpClient.DisposeCallCount,
                "Using statement should dispose the client via the IDisposable path.");
        }

        [TestMethod]
        public void TestSmartsheetClientDisposeIsIdempotent()
        {
            DisposableTestHttpClient httpClient = new DisposableTestHttpClient();
            SmartsheetClient smartsheet = new SmartsheetBuilder()
                .SetBaseURI("http://localhost:9/")
                .SetAccessToken("aaaaaaaaaaaaaaaaaaaaaaaaaa")
                .SetHttpClient(httpClient)
                .Build();

            smartsheet.Dispose();
            smartsheet.Dispose();

            // The HTTP client should have been disposed exactly once, despite two calls to
            // SmartsheetClient.Dispose(). This proves the Interlocked guard is working.
            Assert.AreEqual(1, httpClient.DisposeCallCount,
                "SmartsheetClient.Dispose() must be idempotent; the HTTP client should be disposed exactly once.");
        }

        [TestMethod]
        public void TestOAuthFlowDisposeDisposesHttpClient()
        {
            DisposableTestHttpClient httpClient = new DisposableTestHttpClient();
            OAuthFlow oauthFlow = new OAuthFlowBuilder()
                .SetClientId("aaaaaaaaaaaa")
                .SetClientSecret("bbbbbbbbbbbb")
                .SetRedirectURL("https://example.com/callback")
                .SetHttpClient(httpClient)
                .Build();

            oauthFlow.Dispose();

            Assert.AreEqual(1, httpClient.DisposeCallCount,
                "OAuthFlow.Dispose() should dispose the OAuth flow's HTTP client via the IDisposable path.");
        }

        [TestMethod]
        public void TestOAuthFlowDisposeIsIdempotent()
        {
            DisposableTestHttpClient httpClient = new DisposableTestHttpClient();
            OAuthFlow oauthFlow = new OAuthFlowBuilder()
                .SetClientId("aaaaaaaaaaaa")
                .SetClientSecret("bbbbbbbbbbbb")
                .SetRedirectURL("https://example.com/callback")
                .SetHttpClient(httpClient)
                .Build();

            oauthFlow.Dispose();
            oauthFlow.Dispose();

            Assert.AreEqual(1, httpClient.DisposeCallCount,
                "OAuthFlow.Dispose() must be idempotent; the HTTP client should be disposed exactly once.");
        }
    }
}
