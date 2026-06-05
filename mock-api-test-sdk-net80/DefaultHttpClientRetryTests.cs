using Smartsheet.Api;
using Smartsheet.Api.Models;
using Smartsheet.Api.Internal.Http;
using RestSharp;
using System.Net;
using System.Text;

namespace mock_api_test_sdk_net80
{
    /// <summary>
    /// Unit tests for DefaultHttpClient retry mechanism.
    /// Tests ShouldRetry(), RetrySleep(), and CalcBackoff() methods.
    /// </summary>
    [TestClass]
    public class DefaultHttpClientRetryTests
    {
        /// <summary>
        /// Helper class that exposes protected methods for unit testing
        /// </summary>
        private class TestableDefaultHttpClient : DefaultHttpClient
        {
            public bool SkipSleep { get; set; } = false;

            public TestableDefaultHttpClient() : base() { }

            public TestableDefaultHttpClient(Smartsheet.Api.Internal.Json.JsonSerializer jsonSerializer)
                : base(new RestClient(new RestClientOptions("http://localhost")), jsonSerializer)
            {
            }

            // Override RetrySleep to optionally skip the actual sleep for fast tests
            public override async Task<bool> RetrySleep(int previousAttempts, long totalElapsedTime,
                HttpStatusCode statusCode, Error error, CancellationToken cancellationToken = default)
            {
                long backoff = CalcBackoff(previousAttempts, totalElapsedTime, error);
                if (backoff < 0)
                    return false;

                logger.Info(string.Format("HttpError StatusCode={0}: Retrying in {1} milliseconds",
                    statusCode, backoff));

                // Skip sleep in tests for fast execution
                if (!SkipSleep)
                    await Task.Delay(TimeSpan.FromMilliseconds(backoff), cancellationToken).ConfigureAwait(false);

                return true;
            }

            // Expose protected methods for direct testing
            public bool TestShouldRetry(int attempts, long elapsed, HttpResponse response)
                => ShouldRetry(attempts, elapsed, response);

            public long TestCalcBackoff(int attempts, long elapsed, Error error)
                => CalcBackoff(attempts, elapsed, error);

            public void TestSetMaxRetryTimeout(long timeout)
                => SetMaxRetryTimeout(timeout);
        }

        /// <summary>
        /// Mock JsonSerializer for controlled error deserialization in tests
        /// </summary>
        private class MockJsonSerializer : Smartsheet.Api.Internal.Json.JsonSerializer
        {
            private Error mockError;
            private Exception exceptionToThrow;

            public MockJsonSerializer(Error mockError)
            {
                this.mockError = mockError;
            }

            public MockJsonSerializer(Exception exceptionToThrow)
            {
                this.exceptionToThrow = exceptionToThrow;
            }

            public T deserialize<T>(string json)
            {
                if (exceptionToThrow != null)
                    throw exceptionToThrow;
                return (T)(object)mockError;
            }

            public T deserialize<T>(StreamReader stream)
            {
                if (exceptionToThrow != null)
                    throw exceptionToThrow;
                return (T)(object)mockError;
            }

            // Implement all required interface methods (stubs for testing)
            public void serialize<T>(T obj, StreamWriter outputStream)
                => throw new NotImplementedException();

            public IList<T> deserializeList<T>(StreamReader inputStream)
                => throw new NotImplementedException();

            public PaginatedResult<T> DeserializeDataWrapper<T>(StreamReader inputStream)
                => throw new NotImplementedException();

            public TokenPaginatedResult<T> DeserializeTokenDataWrapper<T>(StreamReader inputStream)
                => throw new NotImplementedException();

            public IDictionary<string, object> DeserializeMap(StreamReader inputStream)
                => throw new NotImplementedException();

            public RequestResult<T> deserializeResult<T>(StreamReader inputStream)
                => throw new NotImplementedException();

            public RequestResult<IList<T>> deserializeListResult<T>(StreamReader inputStream)
                => throw new NotImplementedException();

            public CopyOrMoveRowResult DeserializeRowResult(StreamReader inputStream)
                => throw new NotImplementedException();

            public EventResult DeserializeEventResult(StreamReader inputStream)
                => throw new NotImplementedException();
        }

        #region Helper Methods

        /// <summary>
        /// Creates a mock HttpResponse with specified parameters
        /// </summary>
        private HttpResponse CreateMockResponse(HttpStatusCode statusCode, string contentType, string jsonBody)
        {
            var response = new HttpResponse
            {
                StatusCode = statusCode,
                Headers = new System.Collections.Generic.Dictionary<string, string>()
            };

            if (jsonBody != null)
            {
                var entity = new HttpEntity
                {
                    ContentType = contentType,
                    Content = Encoding.UTF8.GetBytes(jsonBody),
                    ContentLength = jsonBody.Length
                };
                response.Entity = entity;
            }

            return response;
        }

        /// <summary>
        /// Creates a mock Error object
        /// </summary>
        private Error CreateMockError(int errorCode, string message)
        {
            return new Error
            {
                ErrorCode = errorCode,
                Message = message
            };
        }

        #endregion

        #region Status Code Tests

        [TestMethod]
        public void ShouldRetry_Status429_ReturnsTrue()
        {
            var client = new TestableDefaultHttpClient();
            client.SkipSleep = true; // Skip sleep for fast test execution
            client.TestSetMaxRetryTimeout(5000);

            var response = CreateMockResponse(HttpStatusCode.TooManyRequests, "application/json", "{}");

            bool result = client.TestShouldRetry(1, 0, response);

            Assert.IsTrue(result, "Should retry on 429 TooManyRequests");
        }

        [TestMethod]
        public void ShouldRetry_Status502_ReturnsTrue()
        {
            var client = new TestableDefaultHttpClient();
            client.SkipSleep = true;
            client.TestSetMaxRetryTimeout(5000);

            var response = CreateMockResponse(HttpStatusCode.BadGateway, "application/json", "{}");

            bool result = client.TestShouldRetry(1, 0, response);

            Assert.IsTrue(result, "Should retry on 502 BadGateway");
        }

        [TestMethod]
        public void ShouldRetry_Status503_ReturnsTrue()
        {
            var client = new TestableDefaultHttpClient();
            client.SkipSleep = true;
            client.TestSetMaxRetryTimeout(5000);

            var response = CreateMockResponse(HttpStatusCode.ServiceUnavailable, "application/json", "{}");

            bool result = client.TestShouldRetry(1, 0, response);

            Assert.IsTrue(result, "Should retry on 503 ServiceUnavailable");
        }

        [TestMethod]
        public void ShouldRetry_Status400_ReturnsFalse()
        {
            var mockError = CreateMockError(1000, "Bad Request");
            var mockSerializer = new MockJsonSerializer(mockError);
            var client = new TestableDefaultHttpClient(mockSerializer);

            var response = CreateMockResponse(HttpStatusCode.BadRequest, "application/json",
                "{\"errorCode\": 1000, \"message\": \"Bad Request\"}");

            bool result = client.TestShouldRetry(1, 0, response);

            Assert.IsFalse(result, "Should not retry on 400 BadRequest with non-retryable error code");
        }

        [TestMethod]
        public void ShouldRetry_Status404_ReturnsFalse()
        {
            var mockError = CreateMockError(1003, "Not Found");
            var mockSerializer = new MockJsonSerializer(mockError);
            var client = new TestableDefaultHttpClient(mockSerializer);

            var response = CreateMockResponse(HttpStatusCode.NotFound, "application/json",
                "{\"errorCode\": 1003, \"message\": \"Not Found\"}");

            bool result = client.TestShouldRetry(1, 0, response);

            Assert.IsFalse(result, "Should not retry on 404 NotFound");
        }

        [TestMethod]
        public void ShouldRetry_Status500_WithNonRetryableErrorCode_ReturnsFalse()
        {
            var mockError = CreateMockError(5000, "Internal Server Error");
            var mockSerializer = new MockJsonSerializer(mockError);
            var client = new TestableDefaultHttpClient(mockSerializer);

            var response = CreateMockResponse(HttpStatusCode.InternalServerError, "application/json",
                "{\"errorCode\": 5000, \"message\": \"Internal Server Error\"}");

            bool result = client.TestShouldRetry(1, 0, response);

            Assert.IsFalse(result, "Should not retry on 500 with non-retryable error code");
        }

        #endregion

        #region Error Code Tests

        [TestMethod]
        public void ShouldRetry_ErrorCode4001_ReturnsTrue()
        {
            var mockError = CreateMockError(4001, "Rate limit exceeded");
            var mockSerializer = new MockJsonSerializer(mockError);
            var client = new TestableDefaultHttpClient(mockSerializer);
            client.SkipSleep = true; // Skip sleep for fast test execution
            client.TestSetMaxRetryTimeout(5000);

            var response = CreateMockResponse(HttpStatusCode.BadRequest, "application/json",
                "{\"errorCode\": 4001, \"message\": \"Rate limit exceeded\"}");

            bool result = client.TestShouldRetry(1, 0, response);

            Assert.IsTrue(result, "Should retry on error code 4001");
        }

        [TestMethod]
        public void ShouldRetry_ErrorCode4002_ReturnsTrue()
        {
            var mockError = CreateMockError(4002, "Request timeout");
            var mockSerializer = new MockJsonSerializer(mockError);
            var client = new TestableDefaultHttpClient(mockSerializer);
            client.SkipSleep = true;
            client.TestSetMaxRetryTimeout(5000);

            var response = CreateMockResponse(HttpStatusCode.BadRequest, "application/json",
                "{\"errorCode\": 4002, \"message\": \"Request timeout\"}");

            bool result = client.TestShouldRetry(1, 0, response);

            Assert.IsTrue(result, "Should retry on error code 4002");
        }

        [TestMethod]
        public void ShouldRetry_ErrorCode4003_ReturnsTrue()
        {
            var mockError = CreateMockError(4003, "Server throttle");
            var mockSerializer = new MockJsonSerializer(mockError);
            var client = new TestableDefaultHttpClient(mockSerializer);
            client.SkipSleep = true;
            client.TestSetMaxRetryTimeout(5000);

            var response = CreateMockResponse(HttpStatusCode.BadRequest, "application/json",
                "{\"errorCode\": 4003, \"message\": \"Server throttle\"}");

            bool result = client.TestShouldRetry(1, 0, response);

            Assert.IsTrue(result, "Should retry on error code 4003");
        }

        [TestMethod]
        public void ShouldRetry_ErrorCode4004_ReturnsTrue()
        {
            var mockError = CreateMockError(4004, "Concurrent request limit");
            var mockSerializer = new MockJsonSerializer(mockError);
            var client = new TestableDefaultHttpClient(mockSerializer);
            client.SkipSleep = true;
            client.TestSetMaxRetryTimeout(5000);

            var response = CreateMockResponse(HttpStatusCode.BadRequest, "application/json",
                "{\"errorCode\": 4004, \"message\": \"Concurrent request limit\"}");

            bool result = client.TestShouldRetry(1, 0, response);

            Assert.IsTrue(result, "Should retry on error code 4004");
        }

        [TestMethod]
        public void ShouldRetry_ErrorCode4000_ReturnsFalse()
        {
            var mockError = CreateMockError(4000, "Other client error");
            var mockSerializer = new MockJsonSerializer(mockError);
            var client = new TestableDefaultHttpClient(mockSerializer);

            var response = CreateMockResponse(HttpStatusCode.BadRequest, "application/json",
                "{\"errorCode\": 4000, \"message\": \"Other client error\"}");

            bool result = client.TestShouldRetry(1, 0, response);

            Assert.IsFalse(result, "Should not retry on error code 4000");
        }

        [TestMethod]
        public void ShouldRetry_ErrorCode5000_ReturnsFalse()
        {
            var mockError = CreateMockError(5000, "Server error");
            var mockSerializer = new MockJsonSerializer(mockError);
            var client = new TestableDefaultHttpClient(mockSerializer);

            var response = CreateMockResponse(HttpStatusCode.InternalServerError, "application/json",
                "{\"errorCode\": 5000, \"message\": \"Server error\"}");

            bool result = client.TestShouldRetry(1, 0, response);

            Assert.IsFalse(result, "Should not retry on error code 5000");
        }

        #endregion

        #region Backoff Calculation Tests

        [TestMethod]
        public void CalcBackoff_FirstAttempt_ReturnsApproximately2000ms()
        {
            var client = new TestableDefaultHttpClient();
            client.TestSetMaxRetryTimeout(15000);

            long backoff = client.TestCalcBackoff(1, 0, null);

            // 2^1 * 1000 + random(0-1000) = 2000-3000ms
            Assert.IsTrue(backoff >= 2000 && backoff <= 3000,
                $"First attempt backoff should be ~2000-3000ms, got {backoff}ms");
        }

        [TestMethod]
        public void CalcBackoff_SecondAttempt_ReturnsApproximately4000ms()
        {
            var client = new TestableDefaultHttpClient();
            client.TestSetMaxRetryTimeout(15000);

            long backoff = client.TestCalcBackoff(2, 2000, null);

            // 2^2 * 1000 + random(0-1000) = 4000-5000ms
            Assert.IsTrue(backoff >= 4000 && backoff <= 5000,
                $"Second attempt backoff should be ~4000-5000ms, got {backoff}ms");
        }

        [TestMethod]
        public void CalcBackoff_ThirdAttempt_ReturnsApproximately8000ms()
        {
            var client = new TestableDefaultHttpClient();
            client.TestSetMaxRetryTimeout(15000);

            long backoff = client.TestCalcBackoff(3, 6000, null);

            // 2^3 * 1000 + random(0-1000) = 8000-9000ms
            Assert.IsTrue(backoff >= 8000 && backoff <= 9000,
                $"Third attempt backoff should be ~8000-9000ms, got {backoff}ms");
        }

        [TestMethod]
        public void CalcBackoff_IncludesJitter_MultipleCallsVary()
        {
            var client = new TestableDefaultHttpClient();
            client.TestSetMaxRetryTimeout(15000);

            // Call multiple times and verify jitter causes variation
            long backoff1 = client.TestCalcBackoff(1, 0, null);
            long backoff2 = client.TestCalcBackoff(1, 0, null);
            long backoff3 = client.TestCalcBackoff(1, 0, null);

            // All should be in range 2000-3000
            Assert.IsTrue(backoff1 >= 2000 && backoff1 <= 3000);
            Assert.IsTrue(backoff2 >= 2000 && backoff2 <= 3000);
            Assert.IsTrue(backoff3 >= 2000 && backoff3 <= 3000);

            // At least one should be different (very high probability with random jitter)
            bool hasVariation = (backoff1 != backoff2) || (backoff2 != backoff3) || (backoff1 != backoff3);
            Assert.IsTrue(hasVariation, "Jitter should cause variation in backoff values");
        }

        #endregion

        #region Timeout Enforcement Tests

        [TestMethod]
        public void CalcBackoff_TimeoutExceeded_ReturnsNegativeOne()
        {
            var client = new TestableDefaultHttpClient();
            client.TestSetMaxRetryTimeout(5000); // 5 second max

            // Attempt where totalElapsed + backoff would exceed maxRetryTimeout
            // 2^3 * 1000 = 8000ms minimum, plus 4500 already elapsed = 12500+ > 5000
            long backoff = client.TestCalcBackoff(3, 4500, null);

            Assert.AreEqual(-1, backoff, "Should return -1 when timeout would be exceeded");
        }

        [TestMethod]
        public void CalcBackoff_TimeoutBoundary_ReturnsNegativeOne()
        {
            var client = new TestableDefaultHttpClient();
            client.TestSetMaxRetryTimeout(3000); // 3 second max

            // 2^2 * 1000 = 4000ms minimum, already at 0ms, would exceed 3000
            long backoff = client.TestCalcBackoff(2, 0, null);

            Assert.AreEqual(-1, backoff, "Should return -1 when backoff alone exceeds timeout");
        }

        [TestMethod]
        public void CalcBackoff_WithinTimeout_ReturnsPositiveValue()
        {
            var client = new TestableDefaultHttpClient();
            client.TestSetMaxRetryTimeout(10000); // 10 second max

            // 2^1 * 1000 = 2000ms + jitter, at 1000ms elapsed = ~3000-4000 < 10000
            long backoff = client.TestCalcBackoff(1, 1000, null);

            Assert.IsTrue(backoff > 0, "Should return positive backoff when within timeout");
        }

        #endregion

        #region Content-Type Tests

        [TestMethod]
        public void ShouldRetry_NonJsonContentType_ReturnsFalse()
        {
            var client = new TestableDefaultHttpClient();

            var response = CreateMockResponse(HttpStatusCode.BadRequest, "text/html",
                "<html><body>Error</body></html>");

            bool result = client.TestShouldRetry(1, 0, response);

            Assert.IsFalse(result, "Should not retry when content-type is not JSON");
        }

        [TestMethod]
        public void ShouldRetry_JsonContentTypeWithCharset_ParsesError()
        {
            var mockError = CreateMockError(4001, "Rate limit");
            var mockSerializer = new MockJsonSerializer(mockError);
            var client = new TestableDefaultHttpClient(mockSerializer);
            client.SkipSleep = true;
            client.TestSetMaxRetryTimeout(5000);

            var response = CreateMockResponse(HttpStatusCode.BadRequest, "application/json; charset=utf-8",
                "{\"errorCode\": 4001}");

            bool result = client.TestShouldRetry(1, 0, response);

            Assert.IsTrue(result, "Should parse JSON with charset in content-type");
        }

        [TestMethod]
        public void ShouldRetry_XmlContentType_ReturnsFalse()
        {
            var client = new TestableDefaultHttpClient();

            var response = CreateMockResponse(HttpStatusCode.BadRequest, "application/xml",
                "<error><code>400</code></error>");

            bool result = client.TestShouldRetry(1, 0, response);

            Assert.IsFalse(result, "Should not retry when content-type is XML");
        }

        #endregion

        #region Exception Handling Tests

        [TestMethod]
        public void ShouldRetry_JsonSerializationException_ThrowsSmartsheetException()
        {
            var mockSerializer = new MockJsonSerializer(
                new Smartsheet.Api.Internal.Json.JsonSerializationException("Parse failed"));
            var client = new TestableDefaultHttpClient(mockSerializer);

            var response = CreateMockResponse(HttpStatusCode.BadRequest, "application/json",
                "{invalid json}");

            Assert.ThrowsException<SmartsheetException>(() =>
                client.TestShouldRetry(1, 0, response),
                "Should throw SmartsheetException on JsonSerializationException");
        }

        [TestMethod]
        public void ShouldRetry_JsonException_ThrowsSmartsheetException()
        {
            var mockSerializer = new MockJsonSerializer(
                new Newtonsoft.Json.JsonException("Invalid JSON"));
            var client = new TestableDefaultHttpClient(mockSerializer);

            var response = CreateMockResponse(HttpStatusCode.BadRequest, "application/json",
                "{invalid json}");

            Assert.ThrowsException<SmartsheetException>(() =>
                client.TestShouldRetry(1, 0, response),
                "Should throw SmartsheetException on JsonException");
        }

        [TestMethod]
        public void ShouldRetry_IOException_ThrowsSmartsheetException()
        {
            var mockSerializer = new MockJsonSerializer(
                new IOException("Read failed"));
            var client = new TestableDefaultHttpClient(mockSerializer);

            var response = CreateMockResponse(HttpStatusCode.BadRequest, "application/json",
                "{\"errorCode\": 4001}");

            Assert.ThrowsException<SmartsheetException>(() =>
                client.TestShouldRetry(1, 0, response),
                "Should throw SmartsheetException on IOException");
        }

        #endregion
    }
}
