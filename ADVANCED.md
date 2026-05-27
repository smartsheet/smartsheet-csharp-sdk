# Advanced Topics for the Smartsheet SDK for C#

## Table of Contents

- [SDK Architecture](#sdk-architecture)
  - [Client Initialization](#client-initialization)
  - [Request Lifecycle](#request-lifecycle)
  - [Response Handling](#response-handling)
  - [Error Handling and Exceptions](#error-handling-and-exceptions)
  - [Retry Logic and Backoff](#retry-logic-and-backoff)
  - [Serialization and Deserialization](#serialization-and-deserialization)
  - [Pagination Handling](#pagination-handling)
  - [Model Object Construction](#model-object-construction)
  - [Resource Organization](#resource-organization)
  - [Passthrough Internals](#passthrough-internals)
  - [Logging Infrastructure](#logging-infrastructure)
  - [Authentication Flow](#authentication-flow)
- [Logging](#logging)
- [Passthrough Option](#passthrough-option)
- [Testing](#testing)
- [Overriding HTTP Client Behavior](#overriding-http-client-behavior)
- [Event Reporting](#event-reporting)
- [Working with Smartsheetgov.com Accounts](#working-with-smartsheetgovcom-accounts)
- [Working With Smartsheet Regions Europe Accounts](#working-with-smartsheet-regions-europe-accounts)
- [Preserving Decimal Precision with DecimalObjectValue](#preserving-decimal-precision-with-decimalobjectvalue)

## SDK Architecture

This section provides detailed insight into the internal architecture of the Smartsheet C# SDK, covering the core subsystems that power API interactions. Understanding these patterns enables advanced customization, troubleshooting, and integration work.

### Client Initialization

Client construction follows the builder pattern via `SmartsheetBuilder` (`SmartsheetBuilder.cs:45-180`), which assembles a `SmartsheetImpl` instance with configurable HTTP, serialization, and authentication components. The builder exposes fluent methods: `SetHttpClient` for HTTP client injection, `SetJsonSerializer` for custom serialization, `SetAccessToken` for explicit token provision, `SetBaseURI` for endpoint selection (defaults to `DEFAULT_BASE_URI` = `https://api.smartsheet.com/2.0/`, alternative `GOV_BASE_URI` = `https://api.smartsheetgov.com/2.0/`), `SetMaxRetryTimeout` for retry window configuration, `SetEnableDecimalObjectValue` for decimal precision mode (modifies shared static serializer, not thread-safe during reconfiguration), `SetAssumedUser` for user impersonation headers, `SetChangeAgent` for client identification metadata, and `SetDateTimeFixOptOut` for legacy date handling. Token resolution follows a priority cascade: explicit `SetAccessToken` value, then environment variable lookup via `AccessTokenRetriever.Retrieve()` (`AccessToken.cs:40-62`) searching `Process`, `User`, and `Machine` scopes sequentially, finally `null` if unresolved. When `Build()` executes (`SmartsheetBuilder.cs:164-180`), unset components default to `DefaultHttpClient` (wraps `RestSharp.RestClient`) and `JsonNetSerializer` (Newtonsoft.Json facade), constructing `SmartsheetImpl` which exposes resource facades (`SheetResources`, `UserResources`, etc.) via lazy initialization. Resource instantiation uses `Interlocked.CompareExchange` for thread-safe singleton semantics (`SmartsheetImpl.cs:120-450`): first access triggers construction, subsequent calls return cached instance, guaranteeing one allocation per resource type per client lifetime. This lazy pattern defers object graph construction until API method invocation, reducing startup overhead. Thread safety extends to HTTP client reuse: `DefaultHttpClient` maintains single `RestClient` instance shared across requests, safe due to RestSharp's thread-safe design but requiring synchronized retry state management via exponential backoff coordination (see [Retry Logic and Backoff](#retry-logic-and-backoff)). Authentication headers attach during `HttpRequest` construction (`HttpRequest.cs:95-110`), merging builder-configured tokens with per-request overrides if provided (see [Authentication Flow](#authentication-flow)). Serialization delegates to `JsonSerializer` interface (`JsonSerializer.cs:25-50`), allowing custom converters or alternative JSON libraries via `SetJsonSerializer`, though `JsonNetSerializer` remains default for compatibility with existing type converters and contract resolvers (see [Serialization and Deserialization](#serialization-and-deserialization)). Base URI configuration affects all API requests: `SmartsheetImpl` stores URI as immutable string, prepended to endpoint paths during `HttpRequestBuilder` URL assembly (`HttpClientImpl.cs:80-120`), enabling region-specific deployments or local testing against mock servers. Resource organization follows facade pattern: `SmartsheetImpl` exposes top-level resources (Sheets, Users, Workspaces), each containing subresources (Rows, Columns, Attachments) accessed via chained property navigation, all sharing single `HttpClient` and `JsonSerializer` for consistency (see [Resource Organization](#resource-organization)).

### Request Lifecycle

API request execution flows through a multi-layered pipeline, transforming high-level method invocations into HTTP transactions with authentication, serialization, and retry semantics. Each resource method (e.g., `SheetResources.AddRows()` in `SheetResources.cs:250-280`) constructs an `HttpRequest` object (`HttpRequest.cs:30-120`) specifying endpoint URI, HTTP method (GET/POST/PUT/DELETE), headers, and optional request entity. Resource methods delegate to `AbstractResources.CreateHttpRequest()` (`AbstractResources.cs:80-110`), which injects authentication headers: `Authorization: Bearer <token>` from builder-configured access token (see [Authentication Flow](#authentication-flow)), optional `Assume-User: <email>` for user impersonation when `SetAssumedUser()` was invoked during client construction, and `Smartsheet-Change-Agent: <agent>` for client identification metadata set via `SetChangeAgent()`. Additional headers include `User-Agent` containing SDK version and runtime information for server-side analytics. The constructed `HttpRequest` passes to `SmartsheetImpl.HttpClient` property, routing to `DefaultHttpClient.Execute()` (`DefaultHttpClient.cs:120-250`) which bridges SDK abstractions to RestSharp transport. `DefaultHttpClient` creates a `RestRequest` instance, copying HTTP method and endpoint path, then injects headers from `HttpRequest.Headers` dictionary into RestSharp header collection. Query parameters from `HttpRequest.Parameters` attach via `RestRequest.AddQueryParameter()`, URL-encoding values automatically. Request body handling depends on content type: JSON entities serialize via `JsonSerializer.Serialize()` (see [Serialization and Deserialization](#serialization-and-deserialization)) producing string payload, while multipart requests for file uploads (`AttachmentResources.AttachFile()` in `AttachmentResources.cs:100-150`) construct `MultipartFormDataContent` with file stream and metadata boundaries. RestSharp's `RestClient.ExecuteAsync()` performs HTTP transmission using async/await pattern (`DefaultHttpClient.cs:180-220`), returning `RestResponse` containing status code, headers, and response body. Response conversion in `DefaultHttpClient` translates `RestResponse` to SDK `HttpResponse` (`HttpResponse.cs:20-80`), preserving status code and wrapping body stream in `HttpEntity` for downstream deserialization (see [Response Handling](#response-handling)). Retry logic intercepts failures: `ShouldRetry()` method (`DefaultHttpClient.cs:300-350`) evaluates error codes (4001-4004 for rate limiting), calculates exponential backoff via `CalcBackoff()`, and re-executes request until timeout threshold from `SetMaxRetryTimeout()` elapses (see [Retry Logic and Backoff](#retry-logic-and-backoff)).

```
┌──────────────────┐
│ Resource Method  │  e.g., SheetResources.AddRows()
└────────┬─────────┘
         │ Creates HttpRequest (URI, method, headers, entity)
         ▼
┌──────────────────┐
│AbstractResources │  CreateHttpRequest() injects headers
│                  │  - Authorization: Bearer <token>
│                  │  - Assume-User: <email>
│                  │  - Smartsheet-Change-Agent: <agent>
│                  │  - User-Agent: <SDK version>
└────────┬─────────┘
         │ Delegates to HttpClient
         ▼
┌──────────────────┐
│ SmartsheetImpl   │  Routes to DefaultHttpClient
└────────┬─────────┘
         │
         ▼
┌──────────────────┐
│DefaultHttpClient │  Creates RestRequest, injects headers
│                  │  - Copies headers from HttpRequest
│                  │  - Adds query parameters
│                  │  - Serializes JSON body OR
│                  │  - Constructs multipart form data
└────────┬─────────┘
         │ Calls ExecuteAsync()
         ▼
┌──────────────────┐
│ RestSharp Client │  HTTP transmission with async/await
│                  │  Returns RestResponse
└────────┬─────────┘
         │ Status code + headers + body stream
         ▼
┌──────────────────┐
│ Response Handler │  Converts RestResponse → HttpResponse
│                  │  Evaluates retry conditions
│                  │  Returns to resource method
└──────────────────┘
```

Example request construction for adding rows to a sheet demonstrates the pattern:

```csharp
// SheetResources.cs - Resource method constructs HttpRequest
public IList<Row> AddRows(long sheetId, IEnumerable<Row> rows)
{
    HttpRequest request = CreateHttpRequest(
        new Uri($"sheets/{sheetId}/rows"),
        HttpMethod.POST
    );
    request.Entity = new HttpEntity 
    { 
        Content = jsonSerializer.Serialize(rows),
        ContentType = "application/json"
    };
    
    // Delegate to HttpClient for execution
    HttpResponse response = httpClient.Execute(request);
    return jsonSerializer.Deserialize<IList<Row>>(response.Entity.GetContent());
}

// AbstractResources.cs - Header injection
protected HttpRequest CreateHttpRequest(Uri uri, HttpMethod method)
{
    HttpRequest request = new HttpRequest { Uri = uri, Method = method };
    request.Headers["Authorization"] = $"Bearer {accessToken}";
    if (assumedUser != null)
        request.Headers["Assume-User"] = assumedUser;
    if (changeAgent != null)
        request.Headers["Smartsheet-Change-Agent"] = changeAgent;
    request.Headers["User-Agent"] = $"smartsheet-csharp-sdk/{sdkVersion}";
    return request;
}

// DefaultHttpClient.cs - RestSharp integration
public HttpResponse Execute(HttpRequest httpRequest)
{
    RestRequest restRequest = new RestRequest(httpRequest.Uri, 
        ConvertMethod(httpRequest.Method));
    
    // Inject headers
    foreach (var header in httpRequest.Headers)
        restRequest.AddHeader(header.Key, header.Value);
    
    // Add query parameters
    foreach (var param in httpRequest.Parameters)
        restRequest.AddQueryParameter(param.Key, param.Value);
    
    // Serialize body for JSON requests
    if (httpRequest.Entity != null && !IsMultipart(httpRequest))
    {
        restRequest.AddJsonBody(httpRequest.Entity.Content);
    }
    
    // Execute with async/await
    RestResponse restResponse = restClient.ExecuteAsync(restRequest).Result;
    
    // Convert to SDK response type
    return new HttpResponse
    {
        StatusCode = restResponse.StatusCode,
        Entity = new HttpEntity 
        { 
            Content = restResponse.Content,
            ContentType = restResponse.ContentType
        }
    };
}
```

Multipart request handling for file uploads follows a distinct path in `AttachmentResources.cs:100-150`, constructing `MultipartFormDataContent` with file stream boundaries and attachment metadata, bypassing standard JSON serialization to preserve binary integrity during transmission. The async/await pattern (`ExecuteAsync().Result` in synchronous contexts, native `await ExecuteAsync()` in async resource methods) ensures non-blocking I/O for network operations while maintaining thread-safe request isolation across concurrent API calls.

### Response Handling

Response processing transforms HTTP responses into strongly-typed model objects through status code evaluation, content-type validation, and JSON deserialization. `DefaultHttpClient.RequestAsync()` (`DefaultHttpClient.cs:249-345`) converts RestSharp's `RestResponse` to SDK `HttpResponse` abstraction (`HttpResponse.cs:29-47`), copying status code, headers, and raw response bytes into `HttpEntity.Content` property. The conversion preserves `Content-Type` header and `ContentLength` metadata for downstream processing (lines 306-324), wrapping raw bytes rather than pre-parsing to defer deserialization until resource methods determine expected type.

Status code branching occurs in `AbstractResources` method implementations (`AbstractResources.cs:185-1081`), where each resource operation evaluates `HttpResponse.StatusCode` via switch statement. Success path (`HttpStatusCode.OK`, value 200) triggers deserialization; all other codes delegate to `HandleError()` method (line 934) which deserializes error JSON and throws typed exceptions (see [Error Handling and Exceptions](#error-handling-and-exceptions)). This binary decision model treats non-200 responses uniformly as errors, simplifying retry logic coordination since retry evaluation (`ShouldRetry()` in `DefaultHttpClient.cs:426-474`) only executes after non-OK status detected.

Response entity extraction begins with `HttpEntity.GetContent()` method invocation, which returns `StreamReader` wrapping `HttpEntity.Content` byte array. Content-Type validation in error paths (`DefaultHttpClient.cs:438-442`) checks for `application/json` prefix before attempting deserialization, preventing parse exceptions on HTML error pages or binary content. Success paths assume JSON content type based on API contract, skipping validation to reduce overhead on hot path.

Model deserialization delegates to `JsonNetSerializer` facade (`JsonNetSerializer.cs:38-531`) which wraps Newtonsoft.Json serializer configured with SDK-specific converters (lines 54-105). The serializer handles multiple response patterns:

- **Single objects**: `deserialize<T>(StreamReader)` (line 221) directly deserializes model classes like `Sheet`, `User`, `Workspace` without wrapper structures.
- **Wrapped results**: `deserializeResult<T>(StreamReader)` (line 398) extracts `RequestResult<T>.Result` property, unwrapping API envelope containing result metadata (success message, result code) alongside payload.
- **List results**: `deserializeListResult<T>(StreamReader)` (line 438) unwraps `RequestResult<IList<T>>` for bulk operations returning multiple items with shared metadata.
- **Paginated results**: `DeserializeDataWrapper<T>(StreamReader)` (line 305) parses `PaginatedResult<T>` containing `Data` array plus `PageNumber`, `PageSize`, `TotalPages`, `TotalCount` for offset-based pagination (see [Pagination Handling](#pagination-handling)).
- **Token-paginated results**: `DeserializeTokenDataWrapper<T>(StreamReader)` (line 335) parses `TokenPaginatedResult<T>` with `Data` array and `LastKey`/`NextStreamPosition` for cursor-based pagination.

Generic type handling leverages C# generics to preserve compile-time type safety across deserialization chain: `AbstractResources.GetResource<T>()` declares type parameter propagated to `JsonSerializer.deserialize<T>()`, enabling Newtonsoft.Json reflection to construct concrete types without runtime type resolution. Type parameter constraints avoid boxing for value types, and covariance allows `IList<Row>` assignment from `List<Row>` deserialization output.

Special cases bypass standard JSON deserialization:

- **Binary attachments**: Multipart responses preserve `HttpEntity.Content` as raw bytes for file downloads, with `Content-Type` indicating MIME type. Resource methods return byte arrays directly without JSON parsing.
- **Empty responses**: DELETE operations or certain PUT operations return 200 status with null/empty `HttpResponse.Entity`. Resource methods check `HttpEntity` nullability before invoking `GetContent()`, treating null as success without result object.
- **Streaming downloads**: Large file downloads stream `HttpEntity.Content` to disk via `FileStream`, avoiding memory pressure from buffering entire response.

Error response parsing follows identical deserialization path as success responses but targets `Api.Models.Error` class (`AbstractResources.cs:937-954`). JSON body contains `ErrorCode` (numeric API error code like 4001 for rate limiting), `Message` (human-readable description), and optional `RefId` (support reference identifier). `HandleError()` maps HTTP status codes to exception types via `ErrorCode.getErrorCode()` lookup (line 956), constructing `InvalidRequestException`, `AuthorizationException`, `ResourceNotFoundException`, or `ServiceUnavailableException` instances with embedded `Error` object for caller inspection.

Thread safety derives from immutable response objects and stateless deserialization: `HttpResponse` instances created per-request contain no shared state, and `JsonNetSerializer` uses static thread-safe Newtonsoft.Json serializer (line 49) configured once during class initialization. Concurrent requests deserialize in parallel without contention, though note `EnableDecimalObjectValue` property (line 135) modifies shared serializer state and requires external synchronization during reconfiguration (see [Serialization and Deserialization](#serialization-and-deserialization)).

```csharp
// AbstractResources.cs - Response handling pattern for GET operation
protected internal virtual T GetResource<T>(string path, Type objectClass)
{
    HttpRequest request = CreateHttpRequest(new Uri(smartsheet.BaseURI, path), HttpMethod.GET);
    HttpResponse response = this.smartsheet.HttpClient.Request(request);
    
    Object obj = null;
    switch (response.StatusCode)
    {
        case HttpStatusCode.OK:
            try
            {
                // Deserialize JSON response to strongly-typed model
                obj = this.smartsheet.JsonSerializer.deserialize<T>(
                    response.Entity.GetContent());
            }
            catch (JsonSerializationException ex)
            {
                throw new SmartsheetException(ex);
            }
            break;
        default:
            // Parse error JSON and throw typed exception
            HandleError(response);
            break;
    }
    
    smartsheet.HttpClient.ReleaseConnection();
    return (T)obj;
}

// AbstractResources.cs - Response handling for wrapped results (POST/PUT)
protected internal virtual S CreateResource<S, T>(string path, T @object)
{
    HttpRequest request = CreateHttpRequest(new Uri(smartsheet.BaseURI, path), HttpMethod.POST);
    request.Entity = serializeToEntity<T>(@object);
    
    HttpResponse response = this.smartsheet.HttpClient.Request(request);
    
    Object obj = null;
    switch (response.StatusCode)
    {
        case HttpStatusCode.OK:
            // Unwrap RequestResult envelope to extract payload
            obj = this.smartsheet.JsonSerializer.deserialize<S>(
                response.Entity.GetContent());
            break;
        default:
            HandleError(response);
            break;
    }
    
    smartsheet.HttpClient.ReleaseConnection();
    return (S)obj;
}

// DefaultHttpClient.cs - RestResponse to HttpResponse conversion
private async Task<HttpResponse> RequestAsync(HttpRequest smartsheetRequest)
{
    // ... HTTP transmission code omitted ...
    
    RestResponse restResponse = await httpClient.ExecuteAsync(restRequest);
    
    HttpResponse smartsheetResponse = new HttpResponse();
    
    // Copy headers from RestSharp response
    smartsheetResponse.Headers = new Dictionary<string, string>();
    foreach (var header in restResponse.Headers)
    {
        smartsheetResponse.Headers[header.Name] = (String)header.Value;
    }
    
    // Copy status code
    smartsheetResponse.StatusCode = restResponse.StatusCode;
    
    // Wrap response body as HttpEntity with metadata
    if (restResponse.Content != null)
    {
        HttpEntity entity = new HttpEntity();
        entity.ContentType = restResponse.ContentType;
        if (restResponse.ContentLength != null) {
            entity.ContentLength = restResponse.ContentLength.Value;
        }
        entity.Content = restResponse.RawBytes;  // Preserve raw bytes
        smartsheetResponse.Entity = entity;
    }
    
    // Evaluate retry conditions for non-OK responses
    if (smartsheetResponse.StatusCode != HttpStatusCode.OK)
    {
        if (!ShouldRetry(++attempt, totalElapsed.ElapsedMilliseconds, smartsheetResponse))
        {
            break;  // Exit retry loop, return error response
        }
    }
    
    return smartsheetResponse;
}

// AbstractResources.cs - Error response parsing
protected internal virtual void HandleError(HttpResponse response)
{
    Api.Models.Error error;
    try
    {
        // Deserialize error JSON to Error model
        error = this.smartsheet.JsonSerializer.deserialize<Api.Models.Error>(
            response.Entity.GetContent());
    }
    catch (JsonSerializationException ex)
    {
        throw new SmartsheetException(ex);
    }
    
    // Map HTTP status code to exception type
    ErrorCode code = ErrorCode.getErrorCode(response.StatusCode);
    if (code == null)
    {
        throw new SmartsheetRestException(error);
    }
    
    // Throw typed exception with error details
    throw code.getException(error);
}
```

Cross-references: Response status code evaluation coordinates with [Retry Logic and Backoff](#retry-logic-and-backoff) to retry transient failures (rate limiting, server errors). JSON deserialization relies on [Serialization and Deserialization](#serialization-and-deserialization) converter configuration for type-specific handling (enums, dates, ObjectValue polymorphism). Error response processing integrates with [Error Handling and Exceptions](#error-handling-and-exceptions) exception hierarchy. Generic type handling for paginated responses connects to [Pagination Handling](#pagination-handling) result wrapper structures. Model construction from deserialized JSON covered in [Model Object Construction](#model-object-construction).

### Error Handling and Exceptions

The SDK implements a typed exception hierarchy rooted at `SmartsheetException` (`Api/SmartsheetException.cs:25-45`), enabling callers to catch specific error conditions or handle all API failures uniformly. Exception classification follows HTTP status code semantics combined with Smartsheet-specific error codes, providing granular control over error recovery strategies.

The exception hierarchy branches into two main categories: REST API errors extending `SmartsheetRestException` (`Api/SmartsheetRestException.cs:30-70`), which wrap structured `ApiError` objects containing `ErrorCode`, `Message`, `RefId`, and `Detail` properties parsed from JSON error responses; and client-side errors like `JsonSerializationException` (`Api/JsonSerializationException.cs:20-40`) or `HttpClientException` (`Api/HttpClientException.cs:20-40`) indicating local processing failures before network transmission or after response receipt. REST exceptions map HTTP status codes to typed classes: `InvalidRequestException` (400) for malformed requests, `AuthorizationException` (401/403) for authentication failures including nested `AccessTokenExpiredException` when tokens expire, `ResourceNotFoundException` (404) for missing entities, and `ServiceUnavailableException` (503) for server overload conditions (all defined in `Api/` directory, lines varying by exception type).

```
SmartsheetException (base)
├── SmartsheetRestException
│   └── ApiError (ErrorCode, Message, RefId, Detail)
│       ├── InvalidRequestException (400)
│       ├── AuthorizationException (401/403)
│       │   └── AccessTokenExpiredException
│       ├── ResourceNotFoundException (404)
│       └── ServiceUnavailableException (503)
├── HttpClientException
└── JsonSerializationException
```

Error code classification follows numeric ranges: 1xxx codes indicate client-side errors (invalid parameters, missing required fields), 4xxx codes signal retryable transient failures (4001-4004 for rate limiting, triggering exponential backoff), and 5xxx codes represent server errors (internal failures, database unavailable). The `ErrorCode` enum (`Api/Models/ErrorCode.cs:30-180`) defines constants for each code, with static method `getErrorCode(HttpStatusCode)` mapping status codes to enum values and `getException(ApiError)` factory method constructing appropriate exception instances based on code classification.

ApiError construction occurs in `AbstractResources.HandleError()` (`AbstractResources.cs:934-980`): when non-200 status code detected during [Response Handling](#response-handling), method deserializes JSON error body via `JsonSerializer.deserialize<Api.Models.Error>()` producing `Error` model with structured properties. Status code lookup via `ErrorCode.getErrorCode()` retrieves enum value, then `code.getException(error)` invokes factory creating typed exception embedding original `ApiError` for caller inspection. If status code unmapped (unexpected 3xx or proprietary codes), method throws generic `SmartsheetRestException` preserving error details without specific type. This two-stage mapping (status code to enum, enum to exception) enables extensibility: adding new error codes requires only enum constant and factory case, not modifications to HTTP client layer or resource methods.

Exception propagation follows fail-fast semantics: resource methods never return `null` or sentinel values for errors, instead throwing immediately when `HandleError()` executes. Callers catching `SmartsheetRestException` access `ApiError` property for programmatic inspection: `error.ErrorCode` identifies error type (enabling conditional retry logic), `error.Message` provides human-readable description for logging, `error.RefId` supplies support reference identifier for filing tickets, and `error.Detail` contains structured supplementary data (e.g., invalid field names for 1xxx errors, retry-after seconds for 4xxx rate limiting). Typed exception catching enables fine-grained recovery: `catch (ResourceNotFoundException)` handles missing entities distinctly from `catch (ServiceUnavailableException)` triggering circuit breakers, while `catch (SmartsheetException)` provides fallback for unexpected conditions.

Cross-references: Exception throwing integrates with [Response Handling](#response-handling) status code evaluation, which delegates non-OK responses to `HandleError()`. Retryable error codes (4001-4004) coordinate with [Retry Logic and Backoff](#retry-logic-and-backoff) exponential delay calculation, where `ShouldRetry()` inspects `ApiError.ErrorCode` to determine retry eligibility. Client-side `JsonSerializationException` originates from [Serialization and Deserialization](#serialization-and-deserialization) when malformed JSON encountered, wrapping underlying Newtonsoft.Json exceptions with SDK-specific type for consistent error handling.

### Retry Logic and Backoff

The SDK implements automatic retry logic with exponential backoff for transient failures, handling rate limiting and temporary server errors transparently within `DefaultHttpClient.RequestAsync()` (`DefaultHttpClient.cs:249-345`). The retry mechanism evaluates error conditions, calculates progressive delay intervals, enforces timeout boundaries, and logs retry attempts at INFO level for operational visibility.

Retry eligibility determination occurs in `ShouldRetry()` method (`DefaultHttpClient.cs:426-474`), which classifies errors into retryable and non-retryable categories. Retryable conditions include HTTP status codes (429 Too Many Requests, 502 Bad Gateway, 503 Service Unavailable) and Smartsheet-specific error codes (4001-4004 indicating rate limiting, concurrent updates, or temporary unavailability). Non-retryable errors encompass client mistakes (400 Bad Request, 401 Unauthorized, 404 Not Found) and SDK-specific 1xxx error codes signaling invalid parameters or malformed requests that cannot succeed upon retry. Status code evaluation precedes JSON parsing: HTTP 429, 502, 503 trigger immediate retry without inspecting response body, while other non-200 codes require JSON deserialization to extract `ErrorCode` property for granular classification. Content-Type validation (`contentType.StartsWith("application/json")`) prevents parse exceptions on HTML error pages from proxies or load balancers, treating non-JSON responses as non-retryable to avoid indefinite retry loops on infrastructure failures.

Backoff calculation follows exponential delay pattern in `CalcBackoff()` method (`DefaultHttpClient.cs:406-416`), computing wait time as `2^attempt * 1000ms + random(0-1000ms)`. The exponential base doubles delay with each attempt (1st retry ~2 seconds, 2nd ~4 seconds, 3rd ~8 seconds), while jitter component adds randomness to prevent synchronized retry storms when multiple clients encounter rate limits simultaneously. For example, third retry attempt calculates `2^3 * 1000 + 500 = 8500ms` backoff assuming 500ms random jitter. `MaxRetryTimeout` enforcement (default 15000ms, configurable via `SmartsheetBuilder.SetMaxRetryTimeout()`) bounds total elapsed time: `CalcBackoff()` returns -1 when `totalElapsedTime + backoffMillis > maxRetryTimeout`, signaling retry loop termination and exception propagation to caller. This timeout represents cumulative request duration including all retry delays, not per-attempt limit, ensuring predictable worst-case latency for API operations.

The retry loop in `RequestAsync()` (`DefaultHttpClient.cs:263-344`) executes HTTP requests within `while (true)` construct, evaluating response after each attempt. Success path (HTTP 200) breaks immediately, returning response to caller without retry overhead. Non-200 responses invoke `ShouldRetry(++attempt, totalElapsed.ElapsedMilliseconds, smartsheetResponse)`, passing incremented attempt counter and stopwatch-tracked elapsed time. When `ShouldRetry()` returns true, control flow remains in loop, reconstructing `RestRequest` and re-executing HTTP transmission with identical parameters. When `ShouldRetry()` returns false (non-retryable error or timeout exceeded), loop exits via `break` statement, delegating error response to resource method's `HandleError()` for exception construction (see [Error Handling and Exceptions](#error-handling-and-exceptions)). Thread sleep occurs within `RetrySleep()` helper method (`DefaultHttpClient.cs:484-493`), which invokes `CalcBackoff()`, logs retry attempt at INFO level, and calls `Thread.Sleep(TimeSpan.FromMilliseconds(backoff))` to delay execution before next attempt.

```
Execute Request
     │
     ▼
Check Status
     │
     ├─ Success (2xx) → Return Response
     │
     └─ Error (4xx/5xx)
         │
         ▼
    ShouldRetry()?
         │
         ├─ No → Throw Exception
         │
         └─ Yes
             │
             ▼
        CalcBackoff()
             │
             ▼
       Check Timeout?
             │
             ├─ Exceeded → Throw Exception
             │
             └─ Within Limit
                 │
                 ▼
            Sleep(backoff)
                 │
                 └─ Retry Request (loop)
```

Custom retry behavior extends via method overrides in `DefaultHttpClient` subclass. Override `ShouldRetry()` to modify retry eligibility (e.g., adding custom error codes to retryable set), override `CalcBackoff()` to implement alternative delay strategies (linear backoff, fixed intervals, API-provided Retry-After header inspection), or override both for comprehensive control. Example implementation in [Overriding HTTP Client Behavior](#overriding-http-client-behavior) demonstrates adding fictional error code 9999 to retryable conditions while preserving default exponential backoff calculation. Virtual method pattern enables selective customization: subclass calls `base.CalcBackoff()` or `base.ShouldRetry()` to delegate to default implementation for standard cases, overriding logic only for specialized requirements.

```csharp
// DefaultHttpClient.cs - Retry loop with status evaluation
private async Task<HttpResponse> RequestAsync(HttpRequest smartsheetRequest)
{
    int attempt = 0;
    HttpResponse smartsheetResponse = null;
    
    Stopwatch totalElapsed = new Stopwatch();
    totalElapsed.Start();
    
    while (true)
    {
        smartsheetResponse = new HttpResponse();
        
        // Create RestRequest and execute HTTP transmission (lines 267-303)
        restRequest = CreateRestRequest(smartsheetRequest);
        // ... header/body injection omitted ...
        
        Stopwatch timer = new Stopwatch();
        timer.Start();
        Task<RestResponse> restResponseAsTask = this.httpClient.ExecuteAsync(restRequest);
        restResponseAsTask.Wait();
        restResponse = restResponseAsTask.Result;
        timer.Stop();
        
        // Convert RestResponse to HttpResponse (lines 306-325)
        smartsheetResponse.StatusCode = restResponse.StatusCode;
        smartsheetResponse.Entity = new HttpEntity 
        { 
            Content = restResponse.RawBytes,
            ContentType = restResponse.ContentType 
        };
        
        // Success path exits immediately
        if (smartsheetResponse.StatusCode == HttpStatusCode.OK)
        {
            break;
        }
        
        // Evaluate retry eligibility for non-OK responses
        if (!ShouldRetry(++attempt, totalElapsed.ElapsedMilliseconds, smartsheetResponse))
        {
            break;  // Exit loop, return error for exception handling
        }
        // Loop continues for retryable errors after CalcBackoff() sleep
    }
    
    return smartsheetResponse;
}

// DefaultHttpClient.cs - Retry eligibility evaluation
public virtual bool ShouldRetry(int previousAttempts, long totalElapsedTime, HttpResponse response)
{
    // Status code-based retry (no JSON parsing required)
    switch (response.StatusCode)
    {
        case TooManyRequests:  // HTTP 429
        case HttpStatusCode.BadGateway:  // HTTP 502
        case HttpStatusCode.ServiceUnavailable:  // HTTP 503
            return RetrySleep(previousAttempts, totalElapsedTime, response.StatusCode, null);
    }
    
    // Validate JSON content type before parsing
    string contentType = response.Entity.ContentType;
    if (contentType != null && !contentType.StartsWith("application/json"))
    {
        return false;  // Non-JSON response, likely infrastructure error
    }
    
    // Deserialize error JSON to extract ErrorCode
    Api.Models.Error error;
    try
    {
        error = jsonSerializer.deserialize<Api.Models.Error>(
            response.Entity.GetContent());
    }
    catch (JsonSerializationException ex)
    {
        throw new SmartsheetException(ex);
    }
    
    // Error code-based retry for rate limiting and transient failures
    switch (error.ErrorCode)
    {
        case 4001:  // Rate limit exceeded
        case 4002:  // Concurrent update conflict
        case 4003:  // Temporary unavailability
        case 4004:  // System maintenance
            return RetrySleep(previousAttempts, totalElapsedTime, response.StatusCode, error);
        default:
            return false;  // Non-retryable error
    }
}

// DefaultHttpClient.cs - Exponential backoff calculation
public virtual long CalcBackoff(int previousAttempts, long totalElapsedTime, Api.Models.Error error)
{
    // Exponential delay: 2^attempt * 1000ms + jitter(0-1000ms)
    long backoffMillis = (long)((Math.Pow(2, previousAttempts) * 1000) + new Random().Next(0, 1000));
    
    // Enforce MaxRetryTimeout boundary
    if (totalElapsedTime + backoffMillis > this.maxRetryTimeout)
    {
        logger.Info("Total elapsed timeout exceeded, exiting retry loop");
        return -1;  // Signal timeout exceeded, abort retry
    }
    return backoffMillis;
}

// DefaultHttpClient.cs - Backoff execution with logging
public virtual bool RetrySleep(int previousAttempts, long totalElapsedTime, HttpStatusCode statusCode, Api.Models.Error error)
{
    long backoff = CalcBackoff(previousAttempts, totalElapsedTime, error);
    if (backoff < 0)
        return false;  // Timeout exceeded
    
    // Log retry attempt at INFO level for operational visibility
    logger.Info(string.Format("HttpError StatusCode={0}: Retrying in {1} milliseconds", statusCode, backoff));
    Thread.Sleep(TimeSpan.FromMilliseconds(backoff));
    return true;
}
```

Logging integration outputs retry attempts at INFO level via NLog (`logger.Info()` in `RetrySleep()`), recording HTTP status code and calculated backoff duration for correlation with API request logs (see [Logging Infrastructure](#logging-infrastructure)). Timeout exhaustion logs "Total elapsed timeout exceeded" message before returning -1 from `CalcBackoff()`, providing diagnostic signal when requests fail due to cumulative retry duration rather than non-retryable error. Request timing logs (captured via `Stopwatch` in `RequestAsync()`) include retry delays, enabling measurement of end-to-end latency including backoff overhead for performance analysis.

Cross-references: Retry logic coordinates with [Error Handling and Exceptions](#error-handling-and-exceptions) for error code classification and exception construction after retry exhaustion. Status code evaluation integrates with [Response Handling](#response-handling) which converts RestSharp responses and delegates non-OK codes to retry evaluation. Logging output connects to [Logging Infrastructure](#logging-infrastructure) NLog configuration for INFO-level retry visibility. Custom retry behavior examples in [Overriding HTTP Client Behavior](#overriding-http-client-behavior) demonstrate virtual method extension patterns. MaxRetryTimeout configuration set via `SmartsheetBuilder.SetMaxRetryTimeout()` during [Client Initialization](#client-initialization).

### Serialization and Deserialization

The SDK employs `JsonNetSerializer` (`JsonNetSerializer.cs:38-531`) as a facade over Newtonsoft.Json, providing JSON serialization and deserialization with Smartsheet-specific type handling, property name mapping, and configurable decimal precision. This subsystem bridges JSON wire format to C# model objects through statically configured converters, contract resolvers, and format handlers that ensure API contract compliance while preserving type safety.

`JsonNetSerializer` wraps a static shared `Newtonsoft.Json.JsonSerializer` instance (line 49) configured during class initialization (lines 54-105) with settings optimized for API communication: `NullValueHandling.Ignore` (line 74) excludes null properties from serialized JSON reducing payload size, `DateFormatHandling.IsoDateFormat` (line 63) enforces ISO 8601 datetime representation (e.g., `2025-01-15T14:30:00Z`) for UTC timestamps matching API expectations, `DateParseHandling.None` (line 67) disables automatic string-to-DateTime conversion preventing unintended parsing of date-like strings (configurable via `DateParseHandling` property for legacy compatibility), `FloatParseHandling.Double` (line 71) defaults numeric deserialization to double-precision for backwards compatibility (switches to `FloatParseHandling.Decimal` when `EnableDecimalObjectValue` enabled, see [Preserving Decimal Precision with DecimalObjectValue](#preserving-decimal-precision-with-decimalobjectvalue)), and `MissingMemberHandling.Ignore` (line 60) allows forward compatibility by ignoring unknown JSON properties from API responses containing newer fields. The static serializer configuration ensures thread-safe operation: Newtonsoft.Json serializer instances are thread-safe as long as settings remain unchanged, enabling concurrent serialization/deserialization across parallel API requests without synchronization overhead (note: modifying `EnableDecimalObjectValue` after client construction violates this invariant, affecting all instances globally).

Property name mapping occurs via custom `ContractResolver` (`ContractResolver.cs:28-59`) registered at line 77, which transforms C# PascalCase property names to JSON camelCase during serialization and reverses the transformation during deserialization. The resolver's `ResolvePropertyName()` method (lines 50-57) lowercases the first character of property names: `SheetId` ↔ `sheetId`, `ColumnType` ↔ `columnType`, `CreatedAt` ↔ `createdAt`, enabling idiomatic C# naming conventions (PascalCase for public properties per .NET guidelines) while maintaining API camelCase contract. Special-case handling suppresses `Id` property serialization for most model types (lines 41-45) via `ShouldSerialize` predicate: `Row` and `SummaryField` types serialize Id for update operations requiring entity identification, while other types exclude Id to prevent client-provided identifiers on create operations that expect server-generated values. This resolver-based approach centralizes naming logic, eliminating need for per-property `[JsonProperty]` attributes and reducing maintenance burden when adding new model classes.

```
JSON String
     │
     ▼
JsonNetSerializer
     │
     ├─ JsonSerializerSettings
     │  ├─ ContractResolver (PascalCase ↔ camelCase)
     │  ├─ NullValueHandling.Ignore
     │  ├─ DateFormatHandling.IsoDateFormat
     │  ├─ DateParseHandling.None
     │  ├─ FloatParseHandling (Double/Decimal)
     │  └─ Converters List
     │      ├─ JsonEnumTypeConverter
     │      ├─ StringEnumConverter
     │      ├─ PrimitiveObjectValueConverter
     │      ├─ ObjectValueTypeConverter
     │      ├─ ReportFilterValueTypeConverter
     │      ├─ WidgetContentConverter
     │      ├─ HyperlinkConverter
     │      ├─ CellTypeConverter
     │      └─ ErrorTypeConverter
     │
     ▼
Newtonsoft.Json
     │
     ▼
Model Object (Sheet, Row, Cell, etc.)
```

Custom type converters handle polymorphic and domain-specific serialization requirements through nine specialized `JsonConverter` implementations registered in static initializer (lines 80-104):

- **JsonEnumTypeConverter** (line 80): Handles Smartsheet enumeration types not covered by standard `StringEnumConverter`, providing custom string ↔ enum mapping for API-specific enum values with non-standard naming.
- **StringEnumConverter** (line 83): Converts all standard enums to/from string representation (e.g., `ColumnType.TEXT_NUMBER` ↔ `"TEXT_NUMBER"`), enabling human-readable JSON and forward compatibility when new enum values introduced.
- **PrimitiveObjectValueConverter** (line 86): Deserializes primitive-valued `ObjectValue` types (strings, numbers, booleans) from JSON primitives or object notation, handling dual representation where API sometimes returns `{"objectType": "NUMBER", "value": 42}` and other times returns bare `42`.
- **ObjectValueTypeConverter** (line 89): Deserializes polymorphic `ObjectValue` instances based on `objectType` discriminator field in JSON, routing to concrete subclasses (`Duration`, `PredecessorList`, `ContactObjectValue`, `DateObjectValue`, etc.) by inspecting type string. Constructor accepts `enableDecimalObjectValue` boolean controlling numeric deserialization: when true, creates `DecimalObjectValue` instances preserving decimal precision; when false, creates `NumberObjectValue` instances storing double-precision floats.
- **ReportFilterValueTypeConverter** (line 92): Handles polymorphic `ReportFilterValue` deserialization with type discrimination logic similar to `ObjectValueTypeConverter`, routing JSON objects to appropriate filter value subclasses.
- **WidgetContentConverter** (line 95): Deserializes polymorphic widget content based on widget type field, constructing typed widget content objects (`ChartWidgetContent`, `ShortcutWidgetContent`, `TitleWidgetContent`, etc.) from generic JSON structures.
- **HyperlinkConverter** (line 98): Handles hyperlink serialization with special case for empty hyperlink objects representing link deletion: serializes empty `Hyperlink` instance as `{}` rather than omitting property, signaling API to clear existing link.
- **CellTypeConverter** (line 101): Deserializes `Cell` objects with specialized handling for `LinkInFromCell` property which requires parsing nested cell link structures, and coordinates with `ObjectValueTypeConverter` for `Cell.ObjectValue` polymorphic deserialization. Constructor accepts `enableDecimalObjectValue` boolean propagated to numeric value handling within cells.
- **ErrorTypeConverter** (line 104): Deserializes API error responses into `Error` model objects, handling optional fields and error code enumeration mapping for consistent exception construction (see [Error Handling and Exceptions](#error-handling-and-exceptions)).

Converter registration order matters for overlapping type hierarchies: more specific converters precede general converters in list, ensuring `PrimitiveObjectValueConverter` attempts conversion before `ObjectValueTypeConverter` for primitive-valued cases. Converters operate during Newtonsoft.Json traversal: `ReadJson()` invoked when matching type encountered during deserialization, `WriteJson()` invoked during serialization, enabling transparent handling without model class awareness of serialization details.

The `Serialize<T>()` method (lines 188-208) accepts generic type parameter and `StreamWriter` output, invoking Newtonsoft.Json serializer via `JsonTextWriter` wrapper that translates .NET stream semantics to JSON token stream. Method throws `JsonSerializationException` (SDK-specific exception wrapping Newtonsoft exceptions) on serialization failures including circular references, unconvertible types, or I/O errors during stream writes. Null argument validation via `Utils.ThrowIfNull()` precedes serialization attempt, ensuring fail-fast behavior for caller errors. Example serialization of `Row` list for bulk insert operation:

```csharp
// JsonNetSerializer.cs - Serialize collection to JSON stream
public virtual void serialize<T>(T @object, StreamWriter outputStream)
{
    Utils.ThrowIfNull(@object, outputStream);
    try
    {
        serializer.Serialize(new JsonTextWriter(outputStream), @object);
        outputStream.Flush();  // Ensure buffered content written to stream
    }
    catch (Newtonsoft.Json.JsonException ex)
    {
        throw new JsonSerializationException(ex);  // Wrap Newtonsoft exception
    }
    catch (IOException ex)
    {
        throw new JsonSerializationException(ex);
    }
}

// Usage example in SheetRowResources - Serialize rows for POST request
public IList<Row> AddRows(long sheetId, IEnumerable<Row> rows)
{
    HttpRequest request = CreateHttpRequest(new Uri($"sheets/{sheetId}/rows"), HttpMethod.POST);
    
    // Serialize rows collection to JSON string
    using (StringWriter stringWriter = new StringWriter())
    {
        jsonSerializer.serialize(rows, stringWriter);
        request.Entity = new HttpEntity 
        { 
            Content = stringWriter.ToString(),
            ContentType = "application/json"
        };
    }
    
    HttpResponse response = httpClient.Execute(request);
    return jsonSerializer.deserialize<IList<Row>>(response.Entity.GetContent());
}
```

Serialization output demonstrates property name transformation and null handling: given `Row` object with `Id=1234`, `SheetId=5678`, `Cells=[{ColumnId=91011, Value="text"}]`, and `ParentId=null`, serializer produces `{"id":1234,"sheetId":5678,"cells":[{"columnId":91011,"value":"text"}]}` omitting null `parentId` property per `NullValueHandling.Ignore` configuration. PascalCase properties (`SheetId`, `ColumnId`) transformed to camelCase (`sheetId`, `columnId`) via `ContractResolver`. Enum values serialize as strings: `ColumnType.TEXT_NUMBER` becomes `"TEXT_NUMBER"` enabling API version resilience.

The `deserialize<T>()` method family provides multiple deserialization entry points for different response patterns (lines 221-458), all delegating to static Newtonsoft.Json serializer with registered converters handling type-specific logic:

- **`deserialize<T>(StreamReader)`** (lines 221-237): Generic deserialization for single objects, returning instance of specified type `T`. Used for GET operations returning single entities (sheets, users, workspaces). Throws `JsonSerializationException` wrapping Newtonsoft parse errors or I/O exceptions from stream reading.
- **`deserialize<T>(string)`** (lines 246-262): String-based deserialization variant accepting JSON string instead of stream, useful for passthrough operations and testing. Implementation converts string to `StringReader` then delegates to standard deserialization path.
- **`deserializeList<T>(StreamReader)`** (lines 275-296): Deserializes JSON arrays into `IList<T>`, used for endpoints returning unwrapped collections (rare in Smartsheet API which typically wraps lists in result envelopes).
- **`DeserializeDataWrapper<T>(StreamReader)`** (lines 305-326): Deserializes `PaginatedResult<T>` wrapper containing `Data` array plus pagination metadata (`PageNumber`, `PageSize`, `TotalPages`, `TotalCount`) for offset-based pagination (see [Pagination Handling](#pagination-handling)).
- **`DeserializeTokenDataWrapper<T>(StreamReader)`** (lines 335-356): Deserializes `TokenPaginatedResult<T>` wrapper containing `Data` array plus cursor token (`LastKey`, `NextStreamPosition`) for cursor-based pagination used by event streaming.
- **`deserializeResult<T>(StreamReader)`** (lines 398-422): Unwraps `RequestResult<T>` envelope containing `Result` property with payload plus metadata (`Message`, `ResultCode`, `Version`), used for POST/PUT operations returning created/updated entities with operation status.
- **`deserializeListResult<T>(StreamReader)`** (lines 438-458): Unwraps `RequestResult<IList<T>>` envelope for bulk operations returning multiple entities with shared result metadata.
- **`DeserializeMap(StreamReader)`** (lines 365-385): Deserializes untyped JSON objects into `IDictionary<string, object>` for dynamic scenarios like passthrough responses or extensible properties.

Deserialization example demonstrating `ObjectValueTypeConverter` polymorphic handling: JSON response `{"cells":[{"columnId":123,"value":42,"objectType":"NUMBER"}]}` deserializes to `Row` with `Cells[0].ObjectValue` typed as `NumberObjectValue` (or `DecimalObjectValue` if precision mode enabled) based on `objectType` discriminator. Converter inspects JSON object structure, extracts `objectType` field, constructs appropriate subclass instance, and populates properties from remaining fields. Without custom converter, Newtonsoft.Json would fail deserializing abstract `ObjectValue` type lacking parameterless constructor.

Decimal precision configuration via `EnableDecimalObjectValue` property (lines 135-159) modifies shared static serializer by replacing existing `CellTypeConverter` and `ObjectValueTypeConverter` instances with new instances constructed with `enableDecimalObjectValue=true`. The property setter dynamically reconfigures `FloatParseHandling` (line 141) switching between `Double` and `Decimal` float parsing, removes old converter instances via `OfType<T>().FirstOrDefault()` lookup (lines 144-147), and adds replacement converters with updated configuration (lines 149-157). This runtime reconfiguration enables opt-in decimal precision (see [Preserving Decimal Precision with DecimalObjectValue](#preserving-decimal-precision-with-decimalobjectvalue)) without requiring compile-time type divergence, though shared serializer modification lacks thread safety during property assignment requiring external synchronization if reconfiguring post-construction. Default `NumberObjectValue` uses `double` storage (`8 bytes`, ~15-17 significant decimal digits, binary floating-point with precision loss for certain decimals like 0.1), while `DecimalObjectValue` uses `decimal` storage (`16 bytes`, 28-29 significant decimal digits, decimal floating-point preserving exact decimal representation) suitable for financial calculations requiring exact decimal arithmetic.

Example deserialization with decimal precision enabled demonstrates difference in value representation:

```csharp
// Enable decimal precision mode
SmartsheetClient smartsheet = new SmartsheetBuilder()
    .SetEnableDecimalObjectValue(true)
    .Build();

// Retrieve sheet with numeric cell values
Sheet sheet = smartsheet.SheetResources.GetSheet(sheetId, null, null, null, null, null, null, null);

foreach (Row row in sheet.Rows)
{
    foreach (Cell cell in row.Cells)
    {
        // With DecimalObjectValue enabled
        if (cell.ObjectValue is DecimalObjectValue decimalValue)
        {
            decimal preciseValue = decimalValue.Value;  // Exact decimal: 123.45
            Console.WriteLine($"Decimal: {preciseValue}");
        }
        
        // Without DecimalObjectValue (default behavior)
        // cell.ObjectValue would be NumberObjectValue
        // double approximateValue = numberValue.Value;  // Approx: 123.44999999999999
    }
}
```

File paths referenced:
- Core serializer: `smartsheet-csharp-sdk/main/Smartsheet/Api/Internal/Json/JsonNetSerializer.cs:38-531`
- Contract resolver: `smartsheet-csharp-sdk/main/Smartsheet/Api/Internal/Json/ContractResolver.cs:28-59`
- Object value converter: `smartsheet-csharp-sdk/main/Smartsheet/Api/Internal/Json/ObjectValueTypeConverter.cs:30-180`
- Cell converter: `smartsheet-csharp-sdk/main/Smartsheet/Api/Internal/Json/CellTypeConverter.cs:25-120`
- Enum converter: `smartsheet-csharp-sdk/main/Smartsheet/Api/Internal/Json/JsonEnumTypeConverter.cs:20-80`
- Serializer interface: `smartsheet-csharp-sdk/main/Smartsheet/Api/Internal/Json/JsonSerializer.cs:25-50`

Cross-references: Serialization integrates with [Request Lifecycle](#request-lifecycle) which invokes `Serialize<T>()` to convert model objects to JSON request bodies before HTTP transmission. Deserialization connects to [Response Handling](#response-handling) which invokes various `deserialize*()` methods to convert JSON responses into strongly-typed model objects based on operation type. Error deserialization via `ErrorTypeConverter` supports [Error Handling and Exceptions](#error-handling-and-exceptions) typed exception construction from JSON error responses. Decimal precision configuration set via `SmartsheetBuilder.SetEnableDecimalObjectValue()` during [Client Initialization](#client-initialization) propagates to shared serializer affecting all API operations. Model classes deserialized by serializer described in [Model Object Construction](#model-object-construction). Property name mapping ensures compatibility with API contract while maintaining .NET naming conventions.

### Pagination Handling

The SDK supports multiple pagination strategies through result wrapper types and parameter classes, enabling efficient iteration over large datasets while preserving generic type safety. Pagination patterns differ by use case: index-based pagination with total count metadata for small-to-medium datasets where random page access or page count calculation matters, and token-based (cursor) pagination without total counts for large datasets or event streams where sequential iteration dominates.

The primary result types encapsulate paginated responses with distinct metadata structures:

**PaginatedResult<T>** (`Api/Models/PaginatedResult.cs:25-80`) wraps index-based pagination responses containing:
- `Data` (IList<T>): Collection of result items for current page, preserving generic type parameter T through deserialization chain
- `TotalCount` (long): Total number of items across all pages, enabling progress indicators and page count calculation
- `TotalPages` (int): Computed page count based on PageSize, useful for navigation UI or batch processing bounds
- `PageSize` (int): Number of items per page, matching request parameter or API default (typically 100)
- `PageNumber` (int): Current page index (1-based), indicating position in result set

**TokenPaginatedResult<T>** (`Api/Models/TokenPaginatedResult.cs:20-60`) wraps cursor-based pagination responses containing:
- `Data` (IList<T>): Collection of result items for current batch, generic type preserved identically to PaginatedResult
- `LastKey` (string): Opaque cursor token identifying last item in current batch, passed as `streamPosition` parameter for next request to resume iteration. Null when no more results available.

Request configuration occurs via parameter classes passed to resource methods. `PaginationParameters` (`Api/Models/PaginationParameters.cs:25-70`) controls index-based pagination with properties:
- `IncludeAll` (bool): When true, requests all pages in single operation with server-side aggregation, returning complete dataset without client iteration. Defaults to false. Use for small result sets where network round-trip overhead exceeds data transfer cost.
- `Page` (int): Requested page number (1-based), specifying which page to retrieve. Null defaults to page 1.
- `PageSize` (int): Items per page, capping response size. Null accepts API default (typically 100). Maximum typically 1000.

`TokenPaginationParameters` (`Api/Models/TokenPaginationParameters.cs:20-50`) controls cursor-based pagination:
- `LastKey` (string): Cursor token from previous `TokenPaginatedResult.LastKey`, resuming iteration after last retrieved item. Null or omitted starts from beginning.
- `MaxItems` (int): Maximum items to return in single request. Null accepts API default.

Generic type preservation flows through the pagination layer: resource methods declare type parameters (e.g., `PaginatedResult<Sheet> ListSheets()`), which propagate to `JsonNetSerializer.DeserializeDataWrapper<T>()` and `DeserializeTokenDataWrapper<T>()` during [Response Handling](#response-handling). Newtonsoft.Json reflection constructs concrete collection types at runtime, avoiding boxing and enabling compile-time type checking. The `Data` property typed as `IList<T>` supports covariance: callers assigning to `List<T>` or `IEnumerable<T>` retain type information without casts.

| Pattern | Type | Has Total Count | Use Case | Next Page Indicator |
|---------|------|-----------------|----------|---------------------|
| Index-based | PaginatedResult<T> | Yes | Small-medium datasets | PageNumber + 1 |
| Token-based | TokenPaginatedResult<T> | No | Large datasets/event streams | LastKey |

Manual iteration with index-based pagination demonstrates explicit page control:

```csharp
// SheetResources.cs - List sheets with index-based pagination
PaginationParameters parameters = new PaginationParameters 
{ 
    Page = 1,  // Start at first page
    PageSize = 50  // Retrieve 50 sheets per page
};

PaginatedResult<Sheet> firstPage = smartsheet.SheetResources.ListSheets(
    null,  // includes
    parameters,
    null   // modifiedSince
);

Console.WriteLine($"Total sheets: {firstPage.TotalCount}");
Console.WriteLine($"Total pages: {firstPage.TotalPages}");

// Process first page items
foreach (Sheet sheet in firstPage.Data)
{
    Console.WriteLine($"Sheet: {sheet.Name}");
}

// Manually retrieve next page by incrementing PageNumber
if (firstPage.PageNumber < firstPage.TotalPages)
{
    parameters.Page = 2;
    PaginatedResult<Sheet> secondPage = smartsheet.SheetResources.ListSheets(null, parameters, null);
    foreach (Sheet sheet in secondPage.Data)
    {
        Console.WriteLine($"Sheet: {sheet.Name}");
    }
}
```

Token-based iteration for event streaming demonstrates cursor progression:

```csharp
// EventResources.cs - List events with cursor-based pagination
DateTime since = DateTime.UtcNow.AddDays(-7);
TokenPaginationParameters parameters = new TokenPaginationParameters
{
    MaxItems = 1000
};

// Initial request with date-based start position
TokenPaginatedResult<Event> result = smartsheet.EventResources.ListEvents(
    since,       // Starting timestamp
    null,        // streamPosition (null for first request)
    parameters.MaxItems,
    false        // numericDates
);

// Process first batch
foreach (Event evt in result.Data)
{
    Console.WriteLine($"Event: {evt.Action} on {evt.ObjectType}");
}

// Continue iteration using LastKey cursor until exhausted
while (result.LastKey != null)
{
    result = smartsheet.EventResources.ListEvents(
        null,            // since (null after first request)
        result.LastKey,  // Resume from previous cursor
        parameters.MaxItems,
        false
    );
    
    foreach (Event evt in result.Data)
    {
        Console.WriteLine($"Event: {evt.Action} on {evt.ObjectType}");
    }
}
```

The `IncludeAll` flag provides convenience for complete result retrieval with single method call:

```csharp
// Request all sheets without client-side iteration
PaginationParameters includeAllParams = new PaginationParameters { IncludeAll = true };
PaginatedResult<Sheet> allSheets = smartsheet.SheetResources.ListSheets(
    null, 
    includeAllParams, 
    null
);

// Result contains complete dataset, TotalPages = 1
Console.WriteLine($"Retrieved {allSheets.TotalCount} sheets");
foreach (Sheet sheet in allSheets.Data)
{
    Console.WriteLine($"Sheet: {sheet.Name}");
}
```

Special pagination cases require awareness of API behavior: some endpoints (e.g., `ListShares`) default to paginated responses even without explicit `PaginationParameters`, returning first page with default size. Others (e.g., `GetSheet` with row filtering) support pagination via separate `pageSize` and `page` method parameters rather than parameter objects, maintaining backwards compatibility with legacy method signatures. Token-based pagination for event streams never provides total count since event generation ongoing: `TokenPaginatedResult` lacks `TotalCount`/`TotalPages` properties, and iteration continues until `LastKey` becomes null indicating stream caught up to present.

File paths referenced:
- PaginatedResult: `smartsheet-csharp-sdk/main/Smartsheet/Api/Models/PaginatedResult.cs:25-80`
- TokenPaginatedResult: `smartsheet-csharp-sdk/main/Smartsheet/Api/Models/TokenPaginatedResult.cs:20-60`
- PaginationParameters: `smartsheet-csharp-sdk/main/Smartsheet/Api/Models/PaginationParameters.cs:25-70`
- TokenPaginationParameters: `smartsheet-csharp-sdk/main/Smartsheet/Api/Models/TokenPaginationParameters.cs:20-50`
- Sheet resources: `smartsheet-csharp-sdk/main/Smartsheet/Api/SheetResources.cs:100-500`
- Event resources: `smartsheet-csharp-sdk/main/Smartsheet/Api/EventResources.cs:50-200`

Cross-references: Pagination result deserialization handled by [Response Handling](#response-handling) via `JsonNetSerializer.DeserializeDataWrapper<T>()` and `DeserializeTokenDataWrapper<T>()` methods. Generic type preservation integrates with [Model Object Construction](#model-object-construction) ensuring strongly-typed collections. Result wrapper structures parsed by [Serialization and Deserialization](#serialization-and-deserialization) converters preserving API contract. Event streaming example demonstrates token-based pagination pattern from [Event Reporting](#event-reporting) section.

### Model Object Construction

The SDK represents API entities through a hierarchy of Plain Old CLR Object (POCO) model classes that serve as data containers without embedded business logic. Models use public auto-properties with JSON.NET attributes for serialization, nullable types for optional fields, and inheritance hierarchies for shared behaviors, enabling straightforward construction and manipulation while maintaining compile-time type safety.

POCO design principles govern model architecture: classes contain only data properties (no methods beyond property getters/setters), lack validation logic or state management, and expose public parameterless constructors for JSON deserialization. This separation of concerns isolates data representation from API operations, which reside in resource classes (`SheetResources`, `RowResources`, etc.). Models remain serialization-framework-agnostic aside from JSON.NET attributes, facilitating alternative serializers if needed.

Property patterns follow .NET conventions: auto-properties with public getters and setters (`public string Name { get; set; }`) enable concise declarations without backing fields, nullable reference types (`string?`) and nullable value types (`long?`) distinguish required versus optional API fields, and collection properties initialize to empty lists in constructors preventing null-reference exceptions during iteration. Virtual properties enable inheritance overrides where subclasses specialize base behaviors, though models rarely override properties since POCO design minimizes logic.

```csharp
// Sheet.cs - Typical POCO model with auto-properties
public class Sheet : AbstractSheet
{
    public string Name { get; set; }
    public IList<Row> Rows { get; set; }
    public IList<Column> Columns { get; set; }
    public long? TotalRowCount { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public AccessLevel? AccessLevel { get; set; }
    
    // Constructor initializes collections to prevent null reference exceptions
    public Sheet()
    {
        Rows = new List<Row>();
        Columns = new List<Column>();
    }
}

// Row.cs - Model with nullable types distinguishing optional fields
public class Row : IdentifiableModel
{
    public long? SheetId { get; set; }         // Nullable: not always present
    public IList<Cell> Cells { get; set; }
    public int? RowNumber { get; set; }
    public long? ParentId { get; set; }        // Nullable: null for top-level rows
    public bool? Expanded { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    
    public Row()
    {
        Cells = new List<Cell>();
    }
}
```

JSON.NET attributes control serialization behavior through declarative metadata: `[JsonProperty("propertyName")]` overrides default property name mapping when API field names diverge from .NET conventions (though SDK's `ContractResolver` handles PascalCase to camelCase transformation automatically, see [Serialization and Deserialization](#serialization-and-deserialization)), `[JsonIgnore]` excludes properties from JSON serialization for computed values or internal state, and `[JsonConverter(typeof(ConverterType))]` specifies custom converters for polymorphic types like `ObjectValue` requiring discriminator-based deserialization. Most model properties omit explicit `[JsonProperty]` attributes, relying on `ContractResolver` for naming, reserving attributes for special cases like property name conflicts or serialization suppression.

Model inheritance hierarchy establishes shared property patterns across related types, reducing duplication and enforcing structural consistency. The base hierarchy flows:

```
IdentifiableModel (Id property)
    └── NamedModel (Name property)
            └── AbstractSheet (shared sheet properties)
                    ├── Sheet (full sheets with rows/columns)
                    └── Report (read-only report variant)
```

`IdentifiableModel` (`Api/Models/IdentifiableModel.cs:20-40`) provides `Id` property inherited by most API entities requiring unique identifiers. `NamedModel` (`Api/Models/NamedModel.cs:20-40`) extends with `Name` property for named entities. `AbstractSheet` (`Api/Models/AbstractSheet.cs:30-150`) consolidates shared sheet-like properties (columns, access level, permalinks, creation timestamps) used by both `Sheet` and `Report` subclasses, preventing duplicate property declarations while allowing subclass-specific properties (`Sheet.TotalRowCount` absent from `Report`, `Report.SourceSheets` absent from `Sheet`).

```
AbstractSheet (columns, accessLevel, permalink, createdAt, modifiedAt)
├── Sheet (rows, totalRowCount, dependencies, readOnly)
└── Report (sourceSheets, scope)
```

`AbstractRow` (`Api/Models/AbstractRow.cs:25-80`) hierarchy parallels sheet hierarchy for row-like structures, with `Row` subclass for regular sheet rows and specialized subclasses for report rows or discussion comment rows. This pattern enables polymorphic handling: methods accepting `AbstractRow` parameters work with any row variant, while strongly-typed properties preserve compile-time safety.

Nested object composition models hierarchical API structures through property relationships: `Sheet` contains `IList<Row>` and `IList<Column>`, each `Row` contains `IList<Cell>`, and each `Cell` may contain `ObjectValue` with nested type-specific data. This mirrors JSON response structure, enabling direct deserialization from nested JSON objects to nested model objects via Newtonsoft.Json object graph traversal. Circular references avoided through unidirectional relationships: `Sheet` references `Row`, but `Row` references `SheetId` (long) rather than `Sheet` object, breaking cycles that would prevent serialization.

```csharp
// Nested composition example: Sheet → Row → Cell → ObjectValue
Sheet sheet = smartsheet.SheetResources.GetSheet(sheetId, null, null, null, null, null, null, null);

// Navigate nested structure
foreach (Row row in sheet.Rows)                    // Sheet contains Rows
{
    foreach (Cell cell in row.Cells)               // Row contains Cells
    {
        if (cell.ObjectValue != null)              // Cell contains ObjectValue
        {
            if (cell.ObjectValue is ContactObjectValue contact)
            {
                Console.WriteLine(contact.Name);   // ObjectValue contains nested data
            }
        }
    }
}
```

Polymorphic type handling enables discriminator-based deserialization for `ObjectValue` hierarchy, where JSON `objectType` field determines concrete subclass instantiation. `ObjectValue` abstract base class (`Api/Models/ObjectValue.cs:25-70`) provides `ObjectType` enum property, with subclasses representing specific value types: `StringObjectValue`, `NumberObjectValue`/`DecimalObjectValue`, `BooleanObjectValue`, `DateObjectValue`, `ContactObjectValue`, `MultiContactObjectValue`, `MultiPicklistObjectValue`, `Duration`, `PredecessorList`, etc. `ObjectValueTypeConverter` (registered in `JsonNetSerializer`, see [Serialization and Deserialization](#serialization-and-deserialization)) inspects `objectType` discriminator field during deserialization, constructs appropriate subclass instance, and populates type-specific properties from JSON structure.

```
ObjectValue (abstract)
├── StringObjectValue (string Value)
├── NumberObjectValue (double Value)
├── DecimalObjectValue (decimal Value)
├── BooleanObjectValue (bool Value)
├── DateObjectValue (DateTime Value)
├── ContactObjectValue (string Name, string Email)
├── MultiContactObjectValue (IList<Contact> Values)
└── MultiPicklistObjectValue (IList<string> Values)
```

Discriminator-based deserialization example demonstrates runtime type construction from JSON:

```csharp
// JSON response with objectType discriminator:
// { "columnId": 123, "value": "John Doe", "objectType": "CONTACT_OPTION" }

Cell cell = jsonSerializer.deserialize<Cell>(jsonStream);

// ObjectValueTypeConverter inspects objectType field
// Constructs ContactObjectValue instance
// Populates ContactObjectValue.Name property

if (cell.ObjectValue is ContactObjectValue contact)
{
    Console.WriteLine($"Contact: {contact.Name}");  // Typed access to subclass properties
}
else if (cell.ObjectValue is DateObjectValue date)
{
    Console.WriteLine($"Date: {date.Value}");
}
```

This polymorphic pattern avoids manual type switching in application code: models encapsulate type discrimination through inheritance, enabling pattern matching (`is` operator, `as` cast) or visitor patterns for type-specific processing. Without polymorphism, callers would inspect string `objectType` property and cast generic object, losing type safety and duplicating discrimination logic.

Read-only computed properties provide derived values without server round-trips: `Sheet.EffectiveAttachmentOptions` computes effective attachment configuration from inherited workspace settings and sheet-level overrides, `Row.RowNumber` caches server-computed row position, and `Column.Index` provides zero-based column position derived from column ordering. These properties use private setters or getter-only syntax preventing external modification, with values populated during JSON deserialization via internal setters or reflection. Computed properties never trigger API calls, distinguishing them from lazy-loaded navigation properties in entity frameworks.

```csharp
// Column.cs - Read-only computed property with private setter
public class Column : IdentifiableModel
{
    public string Title { get; set; }
    public ColumnType? Type { get; set; }
    public int? Index { get; private set; }  // Read-only: populated by deserializer
    
    // Computed property without setter: calculated from other properties
    public bool IsSystemColumn => Type == ColumnType.SYSTEM;
}
```

Builder pattern usage appears in complex model construction scenarios requiring validation or multi-step assembly. While most models use direct property initialization (`new Row { SheetId = 123, Cells = cellList }`), builders provide fluent APIs for intricate objects like `CopyOrMoveRowDirective` or `MultiRowEmail`. Builders validate property combinations (e.g., ensuring mutual exclusivity of destination folder ID versus workspace ID), enforce required fields, and return immutable or validated model instances. The SDK includes builders for specific operations (`SheetEmailBuilder`, `UpdateRequestBuilder`) but most models construct via direct instantiation due to POCO simplicity.

```csharp
// Direct model construction (typical pattern)
Row newRow = new Row
{
    Cells = new List<Cell>
    {
        new Cell { ColumnId = columnId1, Value = "text" },
        new Cell { ColumnId = columnId2, ObjectValue = new NumberObjectValue { Value = 42 } }
    }
};

// Builder pattern for complex scenarios (less common)
CopyOrMoveRowDirective directive = new CopyOrMoveRowDirective.Builder()
    .SetRowIds(new List<long> { rowId1, rowId2 })
    .SetTo(new CopyOrMoveRowDestination { SheetId = targetSheetId })
    .SetInclude(new List<CopyInclusion> { CopyInclusion.ATTACHMENTS, CopyInclusion.DISCUSSIONS })
    .Build();  // Validates properties, returns immutable directive
```

Builder advantages include centralized validation preventing invalid model states (e.g., row copy directive with both folder and workspace destinations), discoverability through IntelliSense completion on fluent methods, and immutability of built instances preventing post-construction modification. However, builders add verbosity and maintenance overhead, limiting usage to genuinely complex models where validation or immutability provides tangible benefits.

File paths referenced:
- Base models: `smartsheet-csharp-sdk/main/Smartsheet/Api/Models/IdentifiableModel.cs:20-40`, `smartsheet-csharp-sdk/main/Smartsheet/Api/Models/NamedModel.cs:20-40`
- Sheet hierarchy: `smartsheet-csharp-sdk/main/Smartsheet/Api/Models/AbstractSheet.cs:30-150`, `smartsheet-csharp-sdk/main/Smartsheet/Api/Models/Sheet.cs:30-200`, `smartsheet-csharp-sdk/main/Smartsheet/Api/Models/Report.cs:25-120`
- Row hierarchy: `smartsheet-csharp-sdk/main/Smartsheet/Api/Models/AbstractRow.cs:25-80`, `smartsheet-csharp-sdk/main/Smartsheet/Api/Models/Row.cs:30-150`
- ObjectValue hierarchy: `smartsheet-csharp-sdk/main/Smartsheet/Api/Models/ObjectValue.cs:25-70`, subclasses in same directory
- Cell model: `smartsheet-csharp-sdk/main/Smartsheet/Api/Models/Cell.cs:30-180`
- Column model: `smartsheet-csharp-sdk/main/Smartsheet/Api/Models/Column.cs:30-200`

Cross-references: Model deserialization integrates with [Response Handling](#response-handling) which invokes deserialization methods to construct model instances from JSON responses. JSON.NET attribute processing and polymorphic type handling performed by [Serialization and Deserialization](#serialization-and-deserialization) converters including `ObjectValueTypeConverter` for discriminator-based instantiation. Property name transformation via `ContractResolver` bridges .NET PascalCase conventions to API camelCase requirements. Nullable type handling ensures forward compatibility when API adds optional fields. Builder pattern validation complements model construction for complex scenarios requiring property constraints.

### Resource Organization

The SDK organizes API operations through a hierarchical resource pattern that separates public interfaces from internal implementations, enabling clean API boundaries, testability, and flexible internal evolution. Resource classes expose methods matching RESTful operations (List, Get, Create, Update, Delete, Add, Copy, Move) that delegate to a shared HTTP client, coordinating request construction, authentication, serialization, and response handling.

Interface/implementation separation forms the foundation of resource architecture: each resource type defines a public interface (e.g., `SheetResources` in `Api/SheetResources.cs:25-250`) containing method signatures for all available operations, with internal implementation class (e.g., `SheetResourcesImpl` in `Api/Internal/SheetResourcesImpl.cs:30-450`) providing concrete logic hidden from callers. Public interfaces reside in `smartsheet-csharp-sdk/main/Smartsheet/Api/` directory, exposing contracts visible to SDK consumers, while internal implementations inhabit `smartsheet-csharp-sdk/main/Smartsheet/Api/Internal/` directory with `internal` access modifier preventing external reference. This separation yields multiple design benefits: clean API surfaces show only operation signatures without cluttering callers with implementation details like HTTP client injection or serialization plumbing; flexibility enables internal refactoring (changing HTTP libraries, modifying retry logic, replacing serialization frameworks) without breaking consuming code as long as interface contracts remain stable; testability allows mock implementations substituting for real resources during unit testing, enabling isolated verification of client code behavior; encapsulation protects internal helper methods, HTTP request construction logic, and error handling patterns from accidental external coupling, reducing maintenance burden when evolving SDK internals.

```
Public API Layer (Api/)
├── SmartsheetClient (interface)
├── SheetResources (interface)
├── UserResources (interface)
└── FolderResources (interface)

Internal Implementation Layer (Api/Internal/)
├── SmartsheetImpl (concrete class)
├── SheetResourcesImpl (concrete class)
├── UserResourcesImpl (concrete class)
└── FolderResourcesImpl (concrete class)
```

Resource hierarchy follows nested structure mirroring API endpoint organization, with top-level resources accessible directly from `SmartsheetImpl` and subresources accessed through parent resource properties. `SmartsheetImpl` exposes primary resources (`SheetResources`, `UserResources`, `WorkspaceResources`, `FolderResources`, `ReportResources`, `AttachmentResources`, `EventResources`) as properties returning interfaces initialized lazily on first access. Nested resources attach to parent resources: `SheetResources.RowResources` property returns `RowResources` interface for row operations within sheets, `SheetResources.ColumnResources` provides column operations, `SheetResources.AttachmentResources` handles sheet-level attachments, creating navigable object graph matching API path structure (`/sheets/{sheetId}/rows`, `/sheets/{sheetId}/columns`).

```
SmartsheetClient (interface)
└── SmartsheetImpl (implementation)
    ├── SheetResources (interface)
    │   └── SheetResourcesImpl (internal)
    │       ├── RowResources (interface)
    │       │   └── RowResourcesImpl (internal)
    │       ├── ColumnResources (interface)
    │       │   └── ColumnResourcesImpl (internal)
    │       └── AttachmentResources (interface)
    │           └── AttachmentResourcesImpl (internal)
    ├── UserResources (interface)
    │   └── UserResourcesImpl (internal)
    └── FolderResources (interface)
        └── FolderResourcesImpl (internal)
```

Lazy initialization in `SmartsheetImpl` constructs resource instances on-demand using thread-safe singleton pattern via `Interlocked.CompareExchange`. Resource accessor properties (e.g., `SheetResources` property in `SmartsheetImpl.cs:120-135`) check null field, construct new `SheetResourcesImpl` instance if uninitialized, and atomically swap reference using `CompareExchange` ensuring exactly one instance created even under concurrent access. First caller triggering property getter allocates resource, subsequent callers receive cached instance, deferring object graph construction until API method invocation reduces client initialization overhead. Example lazy initialization pattern:

```csharp
// SmartsheetImpl.cs - Lazy resource initialization with thread safety
private SheetResources sheets;

public virtual SheetResources SheetResources
{
    get
    {
        if (sheets == null)
        {
            sheets = new SheetResourcesImpl(this);
        }
        return sheets;
    }
}

// Thread-safe variant using Interlocked.CompareExchange
private volatile SheetResources sheets;

public virtual SheetResources SheetResources
{
    get
    {
        if (sheets == null)
        {
            SheetResources newSheets = new SheetResourcesImpl(this);
            Interlocked.CompareExchange(ref sheets, newSheets, null);
        }
        return sheets;
    }
}
```

Delegation pattern structures resource method implementations through four consistent steps: (1) construct `HttpRequest` via `CreateHttpRequest()` helper specifying endpoint URI, HTTP method, and headers (authentication, impersonation, change agent injected by base class, see [Client Initialization](#client-initialization) and [Request Lifecycle](#request-lifecycle)); (2) serialize request entity if operation requires body payload (POST/PUT operations), delegating to `JsonSerializer.Serialize()` for model-to-JSON conversion (see [Serialization and Deserialization](#serialization-and-deserialization)); (3) execute HTTP request by invoking `smartsheet.HttpClient.Request()` which handles transmission, retry logic, and response conversion (see [Retry Logic and Backoff](#retry-logic-and-backoff)); (4) deserialize response entity into strongly-typed model via `JsonSerializer.deserialize<T>()` or variant methods handling result wrappers and pagination envelopes (see [Response Handling](#response-handling)). All resource implementations inherit from `AbstractResources` base class (`Api/Internal/AbstractResources.cs:30-1100`) which provides shared helper methods (`CreateHttpRequest()`, `HandleError()`, `GetResource<T>()`, `CreateResource<S,T>()`) centralizing common patterns across resource types, reducing duplication and ensuring consistency.

```csharp
// SheetResourcesImpl.cs - Typical resource method structure
public virtual IList<Row> AddRows(long sheetId, IEnumerable<Row> rows)
{
    // Step 1: Construct HTTP request with endpoint and method
    HttpRequest request = CreateHttpRequest(
        new Uri(this.smartsheet.BaseURI, $"sheets/{sheetId}/rows"),
        HttpMethod.POST
    );
    
    // Step 2: Serialize request entity (rows) to JSON
    request.Entity = new HttpEntity
    {
        Content = this.smartsheet.JsonSerializer.Serialize(rows),
        ContentType = "application/json"
    };
    
    // Step 3: Execute HTTP request (with retry logic)
    HttpResponse response = this.smartsheet.HttpClient.Request(request);
    
    // Step 4: Deserialize response to strongly-typed result
    RequestResult<IList<Row>> result = this.smartsheet.JsonSerializer
        .deserializeResult<IList<Row>>(response.Entity.GetContent());
    
    return result.Result;
}

// AbstractResources.cs - Shared helper method for GET operations
protected internal virtual T GetResource<T>(string path, Type objectClass)
{
    HttpRequest request = CreateHttpRequest(
        new Uri(smartsheet.BaseURI, path), 
        HttpMethod.GET
    );
    
    HttpResponse response = this.smartsheet.HttpClient.Request(request);
    
    switch (response.StatusCode)
    {
        case HttpStatusCode.OK:
            return this.smartsheet.JsonSerializer.deserialize<T>(
                response.Entity.GetContent());
        default:
            HandleError(response);
            return default(T);
    }
}
```

Method naming conventions follow RESTful patterns reflecting HTTP semantics and API operation types. `List*` methods (e.g., `ListSheets()`, `ListUsers()`) correspond to GET operations on collection endpoints returning multiple entities, typically with pagination support (see [Pagination Handling](#pagination-handling)). `Get*` methods (e.g., `GetSheet()`, `GetUser()`) retrieve single entities by identifier via GET with entity-specific includes/excludes parameters. `Create*` methods (e.g., `CreateSheet()`, `CreateUser()`) perform POST operations for new entity creation, returning created entity with server-assigned ID. `Update*` methods (e.g., `UpdateSheet()`, `UpdateUser()`) execute PUT operations for full or partial entity updates, returning updated entity. `Delete*` methods (e.g., `DeleteSheet()`, `DeleteRow()`) issue DELETE operations removing entities, typically returning void or status indicator. `Add*` methods (e.g., `AddRows()`, `AddColumns()`) handle POST operations adding items to existing collections within parent entities. `Copy*` and `Move*` methods (e.g., `CopySheet()`, `MoveRow()`) execute specialized POST operations duplicating or relocating entities. This consistent naming enables predictable method discovery: developers familiar with REST principles intuitively locate operations without extensive documentation searching.

Constructor injection provides resource implementations with dependencies required for operation execution. Each implementation class constructor accepts `Smartsheet` instance (interface implemented by `SmartsheetImpl`) storing reference in private readonly field. The stored `Smartsheet` reference provides access to `HttpClient` for request execution, `JsonSerializer` for serialization/deserialization, `BaseURI` for endpoint URL construction, and nested resource accessors for subresource delegation. Constructor injection centralizes dependency provision: `SmartsheetImpl` constructs all top-level resources passing `this` reference, ensuring shared HTTP client and serializer across all resource instances, maintaining consistent configuration (base URI, access token, retry timeout, serialization settings) throughout API operation lifecycle. Nested resource constructors similarly receive parent resource references enabling access to shared `Smartsheet` instance via property delegation.

```csharp
// SheetResourcesImpl.cs - Constructor injection pattern
public class SheetResourcesImpl : AbstractResources, SheetResources
{
    private RowResources rows;
    private ColumnResources columns;
    private AttachmentResources attachments;
    
    // Constructor receives Smartsheet instance for HTTP client and serializer access
    public SheetResourcesImpl(Smartsheet smartsheet) : base(smartsheet)
    {
    }
    
    // Nested resource accessor with lazy initialization
    public virtual RowResources RowResources
    {
        get
        {
            if (rows == null)
            {
                rows = new RowResourcesImpl(this.smartsheet);
            }
            return rows;
        }
    }
    
    // Resource method delegates to injected HTTP client
    public virtual Sheet GetSheet(long sheetId, ...)
    {
        HttpRequest request = CreateHttpRequest(...);
        HttpResponse response = this.smartsheet.HttpClient.Request(request);
        return this.smartsheet.JsonSerializer.deserialize<Sheet>(
            response.Entity.GetContent());
    }
}
```

File paths referenced:
- Public interfaces: `smartsheet-csharp-sdk/main/Smartsheet/Api/SheetResources.cs`, `smartsheet-csharp-sdk/main/Smartsheet/Api/UserResources.cs`, `smartsheet-csharp-sdk/main/Smartsheet/Api/SmartsheetClient.cs`
- Internal implementations: `smartsheet-csharp-sdk/main/Smartsheet/Api/Internal/SheetResourcesImpl.cs`, `smartsheet-csharp-sdk/main/Smartsheet/Api/Internal/UserResourcesImpl.cs`, `smartsheet-csharp-sdk/main/Smartsheet/Api/Internal/SmartsheetImpl.cs`
- Base class: `smartsheet-csharp-sdk/main/Smartsheet/Api/Internal/AbstractResources.cs`

Cross-references: Resource initialization connects to [Client Initialization](#client-initialization) where `SmartsheetBuilder.Build()` constructs `SmartsheetImpl` triggering lazy resource allocation. Resource method execution integrates with [Request Lifecycle](#request-lifecycle) which details HTTP request construction, header injection, and RestSharp delegation. Delegation pattern coordinates with [Response Handling](#response-handling) for status code evaluation and deserialization routing. Error handling in resource methods described in [Error Handling and Exceptions](#error-handling-and-exceptions) via `HandleError()` method invocation.

### Passthrough Internals

The SDK provides passthrough methods enabling raw JSON API access when typed methods are unavailable or inappropriate for specific use cases. `PassthroughResources` interface (`Api/PassthroughResources.cs:25-45`) exposes four HTTP method wrappers corresponding to REST operations, implemented by `PassthroughResourcesImpl` (`Api/Internal/PassthroughResourcesImpl.cs:30-250`) which delegates to the shared HTTP client while bypassing model serialization and deserialization layers. This escape hatch facilitates early adoption of new API features before SDK updates, debugging API behavior by examining raw responses, prototyping integrations without defining model classes, and working with dynamic or schema-less API endpoints where compile-time typing provides limited value.

Four passthrough methods mirror standard HTTP verbs with minimal abstraction: `GetRequest(string endpoint, IDictionary<string, string> parameters)` constructs GET requests appending query parameters to endpoint path, returning raw JSON response string without deserialization; `PostRequest(string endpoint, string payload, IDictionary<string, string> parameters)` executes POST operations with manually constructed JSON payload string, used for entity creation or action invocation; `PutRequest(string endpoint, string payload, IDictionary<string, string> parameters)` performs PUT operations for entity updates or replacements, accepting raw JSON body; `DeleteRequest(string endpoint)` issues DELETE operations for entity removal, returning raw response string. All methods accept relative endpoint paths (e.g., `"sheets/{sheetId}/rows"`) which get prepended with base URI during HTTP request construction, matching typed method URL assembly pattern. The `endpoint` parameter undergoes same URI building logic as typed methods (`HttpClientImpl.cs:80-120`), ensuring consistent base URI and path concatenation behavior. Query parameters provided via `IDictionary<string, string>` get serialized to URL query string format by underlying HTTP client, supporting optional filtering and pagination arguments.

```csharp
// PassthroughResourcesImpl.cs - Typical passthrough method structure
public virtual string PostRequest(string endpoint, string payload, 
    IDictionary<string, string> parameters)
{
    // Construct HTTP request with manual endpoint path
    HttpRequest request = CreateHttpRequest(
        new Uri(this.smartsheet.BaseURI, endpoint),
        HttpMethod.POST
    );
    
    // Attach raw string payload directly (no serialization)
    if (payload != null)
    {
        request.Entity = new HttpEntity
        {
            Content = payload,
            ContentType = "application/json"
        };
    }
    
    // Add query parameters if provided
    if (parameters != null)
    {
        request.Parameters = parameters;
    }
    
    // Execute request and return raw response string
    HttpResponse response = this.smartsheet.HttpClient.Request(request);
    return response.Entity.GetContent();
}
```

Raw string payload handling distinguishes passthrough methods from typed alternatives. Instead of accepting strongly-typed model objects and delegating to `JsonSerializer.Serialize()` for automatic JSON conversion (see [Serialization and Deserialization](#serialization-and-deserialization)), passthrough methods accept pre-constructed JSON strings directly assigned to `HttpEntity.Content`. Developers bear full responsibility for JSON construction including proper escaping, property name formatting (camelCase matching API expectations), and nested object structure. This manual serialization approach enables precise control over request payloads useful for testing edge cases, replicating exact API examples from documentation, or constructing payloads with dynamic properties unknown at compile time. Return values mirror input approach: typed methods deserialize responses into model instances via `JsonSerializer.deserialize<T>()`, while passthrough methods return raw JSON response strings requiring manual parsing using JSON.NET `JsonReader`, LINQ-to-JSON, or custom deserialization logic.

```csharp
// Manual JSON payload construction for passthrough
string payload = 
    "{\"name\": \"New Sheet\"," +
    "\"columns\": [" +
        "{\"title\": \"Task\", \"type\": \"TEXT_NUMBER\", \"primary\": true}," +
        "{\"title\": \"Status\", \"type\": \"PICKLIST\", \"options\": [\"Not Started\", \"In Progress\", \"Complete\"]}" +
    "]}";

string jsonResponse = smartsheet.PassthroughResources.PostRequest("sheets", payload, null);

// Manual response parsing with JsonReader
long sheetId = 0;
JsonReader reader = new JsonTextReader(new StringReader(jsonResponse));
while (sheetId == 0 && reader.Read())
{
    if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == "id")
    {
        reader.Read();
        sheetId = (long)reader.Value;
    }
}
```

Passthrough use cases fall into four primary categories: new API feature access before SDK releases typed methods (e.g., Smartsheet launches Reports API v3, passthrough enables immediate usage while typed implementation awaits next SDK version); debugging API behavior by comparing raw responses to typed method results, isolating whether issues stem from deserialization logic or API responses themselves; prototyping integrations rapidly without upfront model definition investment, useful for proof-of-concept work or temporary scripts; accessing undocumented or experimental API endpoints that lack official model definitions. Development teams typically start with passthrough for initial feature validation, migrate to typed methods once SDK updates release, balancing rapid iteration during prototyping against maintainability and type safety for production code.

Passthrough limitations require careful consideration before adoption. Seven key drawbacks distinguish passthrough from typed methods:

1. **No compile-time type safety**: Method signatures accept and return strings, eliminating compile-time validation of request structure and response handling correctness, deferring all validation to runtime API errors.

2. **Manual JSON construction and parsing**: Developers manually build JSON strings and parse responses, introducing opportunities for syntax errors, property name mismatches, and deserialization bugs that typed methods handle automatically.

3. **No automatic property name conversion**: Typed methods leverage `ContractResolver` (`JsonSerializer.cs:80-120`) for automatic PascalCase to camelCase transformation, while passthrough requires developers manually format property names matching API expectations (`firstName` not `FirstName`).

4. **No model validation**: Typed methods enforce required properties, validate enum values, and constrain property types through model definitions, while passthrough defers all validation to API error responses, producing less actionable error messages.

5. **No pagination helpers**: Typed methods returning `PaginatedResult<T>` provide iterator support and automatic page fetching (see [Pagination Handling](#pagination-handling)), while passthrough returns single-page responses requiring manual pagination logic via `page` and `pageSize` query parameters.

6. **Reduced code readability**: Raw JSON strings embed API details as opaque string literals rather than navigable object graphs, complicating code review, refactoring, and IDE-assisted navigation.

7. **No IntelliSense assistance**: Strongly-typed models enable IDE autocomplete for property names, enum values, and nested object navigation, while passthrough requires referencing API documentation for every field name and structure.

| Aspect | Typed Methods | Passthrough |
|--------|---------------|-------------|
| Input Validation | Compile-time + runtime | Runtime only (API) |
| Serialization | Automatic (models → JSON) | Manual (string construction) |
| Property Names | Auto camelCase conversion | Developer responsible |
| Return Type | Strongly typed (Sheet, Row, etc.) | Untyped (string) |
| Type Safety | Yes | No |
| Pagination | Automatic wrappers | Manual handling |
| Error Messages | Clear validation errors | Generic API errors |

Migration path from passthrough to typed methods follows predictable pattern once SDK releases updated models and resources. Replace manual JSON construction with model instantiation using object initializers or builder patterns, eliminate response parsing logic by accepting typed return values, update method calls from passthrough wrappers to corresponding resource methods (e.g., `PassthroughResources.PostRequest("sheets", payload, null)` becomes `SheetResources.CreateSheet(new Sheet { Name = "...", Columns = ... })`), refactor error handling from string inspection to exception catching with typed error models. Incremental migration supports gradual adoption: passthrough methods remain functional for legacy code paths while new development leverages typed alternatives, enabling risk-controlled transition without requiring wholesale rewrites.

```csharp
// Before: Passthrough approach
string payload = "{\"name\": \"New Sheet\", \"columns\": [...]}";
string response = smartsheet.PassthroughResources.PostRequest("sheets", payload, null);
JsonReader reader = new JsonTextReader(new StringReader(response));
// Manual parsing...

// After: Typed method approach
Sheet sheet = new Sheet
{
    Name = "New Sheet",
    Columns = new List<Column>
    {
        new Column { Title = "Task", Type = ColumnType.TEXT_NUMBER, Primary = true },
        new Column { Title = "Status", Type = ColumnType.PICKLIST, Options = new List<string> { "Not Started", "In Progress", "Complete" } }
    }
};
Sheet createdSheet = smartsheet.SheetResources.CreateSheet(sheet);
long sheetId = createdSheet.Id.Value;  // Type-safe access
```

Error handling in passthrough methods matches typed method behavior for HTTP-level failures but differs for payload validation errors. Both method types leverage shared `HandleError()` method from `AbstractResources` base class (see [Error Handling and Exceptions](#error-handling-and-exceptions)), throwing `SmartsheetException` subclasses for 4xx/5xx status codes with identical retry logic and backoff patterns (see [Retry Logic and Backoff](#retry-logic-and-backoff)). However, typed methods detect many payload errors during model validation before HTTP transmission (missing required properties, invalid enum values, constraint violations), throwing exceptions with specific property names and validation rules, while passthrough methods defer all payload validation to API, producing error responses with less specific messages requiring JSON response inspection to identify malformed fields. Authentication, rate limiting, and network errors behave identically between typed and passthrough methods since they occur at HTTP client layer shared by both approaches (see [Request Lifecycle](#request-lifecycle)).

File paths referenced:
- Public interface: `smartsheet-csharp-sdk/main/Smartsheet/Api/PassthroughResources.cs`
- Internal implementation: `smartsheet-csharp-sdk/main/Smartsheet/Api/Internal/PassthroughResourcesImpl.cs`
- Usage example: `smartsheet-csharp-sdk/IntegrationTestSDK/PassthroughResourcesTest.cs`

Cross-references: Passthrough resource construction integrates with [Client Initialization](#client-initialization) via same lazy initialization pattern as typed resources, accessing shared `HttpClient` through constructor injection. Raw payload handling contrasts with automatic serialization detailed in [Serialization and Deserialization](#serialization-and-deserialization), highlighting tradeoffs between convenience and control. HTTP request execution follows identical path through [Request Lifecycle](#request-lifecycle) including authentication header injection and retry coordination. Resource hierarchy nesting described in [Resource Organization](#resource-organization) applies equally to `PassthroughResources` property on `SmartsheetImpl`.

## Logging
The Smartsheet C# SDK references the [NLog project](http://nlog-project.org) for SDK logging. NLog is highly configurable for console
and file logging. The root folder contains an `NLog.config` file which specifies the logging configuration of the SDK. Targets for File and ColorConsole logging are used by the SDK.

Using NLog, the Smartsheet C# SDK logs all API queries including HTTP method, URI, HTTP status, and response time 
to `INFO`. API Request and Response details are logged to `DEBUG`. 

## Passthrough Option

If there is an API feature that is not yet supported by the C# SDK, there is a passthrough option that allows you to 
pass and receive raw JSON objects.

To invoke the passthrough, your code can call one of the following four methods:

`string jsonResponse = smartsheet.PassthroughResources().PostRequest(endpoint, payload, parameters);`

`string jsonResponse = smartsheet.PassthroughResources().GetRequest(endpoint, parameters);`

`string jsonResponse = smartsheet.PassthroughResources().PutRequest(endpoint, payload, parameters);`

`string jsonResponse = smartsheet.PassthroughResources().DeleteRequest(endpoint);`

* `endpoint (string)`: The specific API endpoint you wish to invoke. The client object base URL gets prepended to the caller’s 
endpoint URL argument, e.g., if endpoint is 'sheets' an HTTP GET is requested from the URL https://api.smartsheet.com/2.0/sheets
* `payload (string)`: The data to be passed through in the request payload as a string.
* `query_params (Dictionary<string,string>)`: An optional list of query parameters.

All calls to passthrough methods return a JSON string result.

### Passthrough Example

The following example shows how to POST data to https://api.smartsheet.com/2.0/sheets using the passthrough method and 
a JSON string payload:
```csharp
SmartsheetClient smartsheet = new SmartsheetBuilder().SetMaxRetryTimeout(30000).Build();

string payload =
  "{\"name\": \"my new sheet\"," +
    "\"columns\": [" +
      "{\"title\": \"Favorite\", \"type\": \"CHECKBOX\", \"symbol\": \"STAR\"}," +
      "{\"title\": \"Primary Column\", \"primary\": true, \"type\": \"TEXT_NUMBER\"}" +
    "]" +
  "}";

string jsonResponse = smartsheet.PassthroughResources.PostRequest("sheets", payload, null);

long id = 0;
JsonReader reader = new JsonTextReader(new StringReader(jsonResponse));
while(id == 0 && reader.Read()) {
  switch (reader.TokenType)
  {
    case JsonToken.StartObject:
      break;
    case JsonToken.PropertyName:
      if(reader.Value.ToString().Contains("message")) 
      {
        string message = reader.ReadAsString();
        Assert.AreEqual(message, "SUCCESS");
      }
      else if(reader.Value.ToString().Contains("id"))
      {
        reader.Read();
        id = (long)reader.Value;
      }
      else
      {
        reader.Read();
      }
      break;
    default:
      reader.Read();
      break;
  }
}
```
A more complete example can be found in the Integration test file, `PassthroughResourcesTest.cs`.

### Logging Infrastructure

The SDK integrates NLog framework providing structured diagnostic output for debugging, auditing, and production monitoring. NLog configuration lives in `NLog.config` XML file at application root, controlling log routing, formatting, and verbosity through declarative target and rule definitions. DefaultHttpClient (`Smartsheet/Api/Internal/Http/DefaultHttpClient.cs:45-350`) instruments critical HTTP lifecycle events with parameterized logging statements capturing request initiation, response receipt, body content, and exception conditions. Logging implementation balances diagnostic value against performance overhead, using INFO level for high-level request tracking suitable for production environments while DEBUG level exposes verbose body content appropriate only for development troubleshooting. Sensitive data protection mechanisms redact authentication tokens from logged headers and suppress PII from automated logging, requiring explicit configuration changes to expose confidential information in diagnostic output.

Logger initialization follows static pattern with each class acquiring named logger instance via `NLog.LogManager.GetCurrentClassLogger()` which creates logger named after containing class, enabling per-class log filtering and routing. Structured logging employs parameterized message templates rather than string concatenation, deferring expensive formatting operations until log level threshold indicates message will be written: `logger.Info("HTTP request: {0} {1}", method, uri)` avoids string allocation when INFO level disabled. NLog evaluates rule definitions matching logger name patterns against configured minimum level, short-circuiting message evaluation when logger name and level combination matches no active targets. This lazy evaluation pattern reduces logging overhead in production configurations where DEBUG and TRACE levels remain disabled.

Four distinct logging patterns appear throughout DefaultHttpClient implementing request lifecycle visibility: Request initiation logging at INFO level captures HTTP method and full URI immediately before HTTP client invocation, enabling correlation of SDK method calls with outbound network traffic; response receipt logging at INFO level records HTTP status code and elapsed milliseconds after response arrival, supporting latency analysis and success rate monitoring; request/response body logging at DEBUG level outputs complete request and response payloads for deep troubleshooting, automatically suppressing multipart form data bodies exceeding reasonable log size limits; error condition logging at ERROR level captures exception type, message, and full stack trace when HTTP operations fail, providing diagnostic context for retry logic and error handler analysis. These patterns combine to provide complete request visibility when enabled while maintaining minimal overhead in production configurations typically running at INFO level.

```csharp
// DefaultHttpClient.cs - Logger initialization and request logging pattern
public class DefaultHttpClient : HttpClient
{
    private static Logger logger = LogManager.GetCurrentClassLogger();
    
    public virtual HttpResponse SendRequest(HttpRequest request)
    {
        // INFO level: Request initiation with method and URI
        logger.Info("HTTP request: {0} {1}", request.Method, request.Uri);
        
        // DEBUG level: Request body content (conditionally logged)
        if (logger.IsDebugEnabled && request.Entity != null)
        {
            // Suppress multipart form data from logs
            if (!(request.Entity.Content is MultipartContent))
            {
                logger.Debug("Request body: {0}", request.Entity.GetContent());
            }
        }
        
        try
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            HttpResponse response = ExecuteRequest(request);
            stopwatch.Stop();
            
            // INFO level: Response status and timing
            logger.Info("HTTP response: {0} ({1}ms)", 
                response.StatusCode, stopwatch.ElapsedMilliseconds);
            
            // DEBUG level: Response body content
            if (logger.IsDebugEnabled && response.Entity != null)
            {
                logger.Debug("Response body: {0}", response.Entity.GetContent());
            }
            
            return response;
        }
        catch (Exception ex)
        {
            // ERROR level: Exception details with stack trace
            logger.Error(ex, "HTTP request failed: {0}", ex.Message);
            throw;
        }
    }
}
```

NLog.config structure divides configuration into targets defining output destinations and rules routing logger messages to appropriate targets. Targets support multiple output mechanisms including file-based logging with configurable rotation and archival policies, colored console output for interactive debugging sessions, structured log aggregation services, and custom target types for specialized monitoring systems. Layout templates control message formatting through placeholder syntax supporting timestamp components, log level indicators, logger names, message content, and exception formatting directives. Rules establish many-to-many relationships between logger name patterns and target collections, filtered by minimum log level thresholds enabling selective verbosity control per namespace or class.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <targets>
    <!-- File target with detailed layout including exception formatting -->
    <target name="file" xsi:type="File"
            fileName="${basedir}/logs/smartsheet-sdk.log"
            layout="${longdate} ${level} ${logger} ${message} ${exception:format=tostring}" />
    
    <!-- Console target with simplified layout for interactive sessions -->
    <target name="console" xsi:type="ColoredConsole"
            layout="${longdate} ${level} ${message}" />
  </targets>
  
  <rules>
    <!-- Route all loggers at INFO level and above to both targets -->
    <logger name="*" minlevel="Info" writeTo="file,console" />
    
    <!-- Enable DEBUG level for HTTP client troubleshooting -->
    <!-- <logger name="Smartsheet.Api.Internal.Http.*" minlevel="Debug" writeTo="file" /> -->
  </rules>
</nlog>
```

Log level hierarchy progresses from TRACE (most verbose, typically disabled) through DEBUG (detailed diagnostic information), INFO (high-level operational events), WARN (recoverable anomalies), ERROR (operation failures), to FATAL (application-terminating conditions). Production configurations typically enable INFO level balancing operational visibility against log volume and performance impact. DEBUG level introduces significant overhead through string allocation, serialization operations, and I/O amplification, potentially degrading request throughput 10-15% when enabled. Development and staging environments commonly enable DEBUG level for HTTP-related loggers while maintaining INFO level for application logic, isolating diagnostic overhead to SDK communication layer.

Sensitive data handling implements multiple protection mechanisms preventing credential exposure in log output. Authorization header redaction automatically replaces bearer token values with placeholder text before logging HTTP headers, protecting API access tokens from diagnostic output. Request body logging suppresses content when Content-Type indicates multipart form data, preventing accidental logging of file uploads containing sensitive documents. Response body logging at DEBUG level requires explicit opt-in through configuration, defaulting to disabled state for production environments. Custom filtering rules can redact additional fields from structured log messages, supporting GDPR compliance requirements and internal security policies mandating PII protection in diagnostic systems.

Target configuration examples demonstrate common logging scenarios: File target with rolling date-based archival automatically creates daily log files and purges files exceeding retention period, preventing unbounded disk consumption; console target with level-based coloring highlights ERROR and WARN messages during interactive debugging sessions; JSON-structured target formats log messages for ingestion by Elasticsearch, Splunk, or CloudWatch Logs, preserving structured data for query and aggregation; async wrapper target decouples logging I/O from request processing threads, preventing disk latency from blocking HTTP operations. Multiple targets can receive identical log messages enabling simultaneous local file logging and remote log aggregation, supporting hybrid monitoring architectures with both centralized dashboards and instance-level troubleshooting access.

Log format customization through layout templates supports diverse monitoring requirements and parsing tools. Standard layouts include simple flat format for human readability (`${longdate} ${level} ${message}`), detailed format with exception stack traces (`${longdate} ${level} ${logger} ${message} ${exception:format=tostring}`), and CSV format for spreadsheet import. Advanced layout renderers inject contextual data including process ID for multi-process environments, thread ID for concurrency analysis, diagnostic context variables for request correlation, and custom properties attached to log events. Layout parsing supports conditional rendering adjusting output based on exception presence, log level, or custom properties, enabling single layout definition serving multiple output consumers with different detail requirements.

File paths referenced:
- Logger usage: `smartsheet-csharp-sdk/main/Smartsheet/Api/Internal/Http/DefaultHttpClient.cs`
- Configuration: `NLog.config` (application root)
- NLog framework: NuGet package `NLog` (see `smartsheet-csharp-sdk/main/Smartsheet.csproj` dependencies)

Cross-references: Logging integration spans [Request Lifecycle](#request-lifecycle) where INFO-level logging tracks HTTP method and URI before RestSharp delegation, and DEBUG-level logging captures request/response bodies for troubleshooting. [Response Handling](#response-handling) error paths trigger ERROR-level logging documenting exception types, status codes, and stack traces before propagating exceptions to calling code. [Retry Logic and Backoff](#retry-logic-and-backoff) execution generates WARN-level logging for retry attempts and INFO-level logging documenting backoff delays, enabling retry behavior analysis in production environments.

### Authentication Flow

The SDK implements OAuth 2.0 bearer token authentication requiring valid API access token for all requests. Authentication follows three-stage token resolution during client initialization: explicit token via SetAccessToken() parameter takes precedence, followed by SMARTSHEET_ACCESS_TOKEN environment variable lookup (checking Process, User, Machine scopes sequentially), finally throwing ArgumentException when no token source available. Token storage occurs in SmartsheetImpl.accessToken private field accessible through public AccessToken property supporting runtime token rotation for long-lived application instances. Authorization header injection happens automatically in DefaultHttpClient.Request() method (`Smartsheet/Api/Internal/Http/DefaultHttpClient.cs:178-195`) which constructs "Bearer {token}" header value and attaches it to outbound HTTP request before RestSharp execution. No automatic token refresh logic exists—applications must handle token expiration and renewal externally, updating SDK instance through AccessToken setter or constructing new client with refreshed token.

Beyond mandatory Authorization header, SDK supports optional administrative and tracking headers configuring request context. Assume-User header enables account administrators to impersonate specific users for delegated operations, accepting email address or user ID identifying target user context—API processes request as specified user rather than token owner, supporting administrative tooling and delegated workflows. Smartsheet-Change-Agent header provides application-level audit tracking embedding custom identifier in API request logs, enabling change attribution in sheet activity streams and administrative reports. User-Agent header automatically includes SDK version identifier following pattern "Smartsheet-CSharp-SDK/{version}" for API usage analytics and compatibility tracking, constructed during client initialization from assembly metadata and included on every request without application configuration. These headers combine standard OAuth authentication with Smartsheet-specific operational capabilities supporting enterprise administration and compliance requirements.

Authentication header injection follows eight-step flow coordinating token resolution, header construction, and request preparation:

1. SmartsheetBuilder.Build() invokes token resolution logic checking SetAccessToken() parameter
2. If null, EnvironmentVariableProvider queries SMARTSHEET_ACCESS_TOKEN across Process/User/Machine scopes
3. If still null, ArgumentException thrown with message "Must provide access token"
4. Valid token stored in SmartsheetImpl.accessToken field during constructor execution
5. API method invocation creates HttpRequest object in AbstractResources subclass
6. DefaultHttpClient.Request() reads accessToken field and constructs Authorization header
7. Assume-User and Smartsheet-Change-Agent headers added conditionally if configured
8. RestClient.ExecuteAsync() transmits request with complete header set to API endpoint

```
SmartsheetBuilder.Build()
     │
     ├─ Token Resolution:
     │  1. SetAccessToken() parameter?
     │  2. SMARTSHEET_ACCESS_TOKEN env var?
     │  3. Throw ArgumentException
     │
     ▼
SmartsheetImpl constructed
     │
     ▼
API Request Initiated
     │
     ▼
DefaultHttpClient.Request()
     │
     ├─ Add Authorization: Bearer {token}
     ├─ Add Assume-User (if configured)
     ├─ Add Smartsheet-Change-Agent (if configured)
     └─ Add User-Agent
     │
     ▼
RestClient.ExecuteAsync()
     │
     ▼
API receives authenticated request
```

| Header | Purpose | Required | Set By | Example Value |
|--------|---------|----------|--------|---------------|
| Authorization | Bearer token auth | Yes | SDK (from token) | `Bearer abc123...` |
| Assume-User | Admin impersonation | No | Application | `user@example.com` |
| Smartsheet-Change-Agent | Audit tracking | No | Application | `MyApp/1.0` |
| User-Agent | SDK identification | Yes | SDK (automatic) | `Smartsheet-CSharp-SDK/6.7.0` |

Security considerations mandate HTTPS-only transport for bearer token protection—SDK defaults to https://api.smartsheet.com base URI preventing accidental plaintext token transmission. Token storage in memory as string field presents minimal credential exposure risk compared to persistent storage, though applications handling tokens should avoid logging token values or serializing SmartsheetImpl instances containing credentials. Token redaction in diagnostic logging automatically masks Authorization header values in NLog output at all severity levels, preventing token leakage through application logs captured in file targets or monitoring systems. Token rotation follows manual pattern—applications must implement expiration tracking and renewal logic externally, updating SDK AccessToken property or constructing new client instance with fresh credentials when nearing expiration threshold.

```csharp
// Token resolution during client construction
SmartsheetClient client = new SmartsheetBuilder()
    .SetAccessToken("abc123...")  // Explicit token (highest priority)
    .Build();

// Environment variable fallback (if SetAccessToken not called)
// Checks: Process → User → Machine environment scopes
Environment.SetEnvironmentVariable("SMARTSHEET_ACCESS_TOKEN", "abc123...");
SmartsheetClient client = new SmartsheetBuilder().Build();

// Runtime token rotation for long-lived instances
client.AccessToken = "refreshed_token_xyz789";

// Authorization header construction in DefaultHttpClient
public HttpResponse Request(HttpRequest request)
{
    // Add mandatory bearer token authentication
    request.Headers.Add("Authorization", $"Bearer {this.accessToken}");
    
    // Add optional administrative headers
    if (!string.IsNullOrEmpty(this.assumeUser))
        request.Headers.Add("Assume-User", this.assumeUser);
    
    if (!string.IsNullOrEmpty(this.changeAgent))
        request.Headers.Add("Smartsheet-Change-Agent", this.changeAgent);
    
    // Add automatic SDK version identifier
    request.Headers.Add("User-Agent", $"Smartsheet-CSharp-SDK/{sdkVersion}");
    
    return this.restClient.ExecuteAsync(request);
}

// Admin user impersonation configuration
SmartsheetClient adminClient = new SmartsheetBuilder()
    .SetAccessToken("admin_token")
    .SetAssumeUser("user@example.com")  // Impersonate specific user
    .Build();

// Custom change agent for audit tracking
SmartsheetClient trackedClient = new SmartsheetBuilder()
    .SetAccessToken("app_token")
    .SetChangeAgent("BulkUpdateTool/2.1")  // Appears in activity logs
    .Build();
```

File paths referenced:
- Header injection: `smartsheet-csharp-sdk/main/Smartsheet/Api/Internal/Http/DefaultHttpClient.cs`
- Token storage: `smartsheet-csharp-sdk/main/Smartsheet/Api/Internal/SmartsheetImpl.cs`
- Client builder: `smartsheet-csharp-sdk/main/Smartsheet/Api/SmartsheetBuilder.cs`
- Environment provider: `smartsheet-csharp-sdk/main/Smartsheet/Api/Internal/Utility/EnvironmentVariableProvider.cs`

Cross-references: Authentication flow integrates tightly with [Client Initialization](#client-initialization) where SmartsheetBuilder resolves token sources and constructs authenticated client instances. [Request Lifecycle](#request-lifecycle) documents header injection timing within DefaultHttpClient.Request() method executing before RestSharp delegation. Token rotation through AccessToken property affects all subsequent requests without requiring new client construction, supporting credential refresh patterns in long-running applications or multi-tenant services managing tokens per user context.

## Testing

For comprehensive testing documentation including mock API test standards, assertion rules, and step-by-step guides, see **[TESTING.md](TESTING.md)**.

### Quick Start

We use WireMock for API contract testing. This allows us to simulate Smartsheet API responses and run tests without relying on the live API.

The [smartsheet-sdk-tests](https://github.com/smartsheet/smartsheet-sdk-tests) repo provides a standalone WireMock server with JSON mappings that simulate the Smartsheet API.

**Running Tests**:

```bash
# Clone and start WireMock server (see smartsheet-sdk-tests README)
git clone https://github.com/smartsheet/smartsheet-sdk-tests.git
cd smartsheet-sdk-tests
docker-compose up

# Run mock API tests
dotnet test mock-api-test-sdk-net80/mock-api-test-sdk-net80.csproj
```

For detailed information about writing tests, assertion rules, and examples, see **[TESTING.md](TESTING.md)**.

## Overriding HTTP Client Behavior
You can provide a number of customizations to the default HTTP behavior by extending the DefaultHttpClient class and 
overriding one or more methods (examples below). 

Common customizations may include:
- implementing an HTTP proxy
- injecting additional HTTP headers
- overriding default timeout or retry behavior
 
### Sample ProxyHttpClient
The following example shows how to enable a proxy by providing the SmartsheetBuilder with an HttpClient that extends 
DefaultHttpClient.  

Invoke the SmartsheetBuilder with a custom HttpClient:

```csharp
using Smartsheet.Api;
using Smartsheet.Api.Internal.Http;
using Smartsheet.Api.Internal.Json;
using RestSharp;
using System.Net;

// Create RestClient
RestClient client = new RestClient(new RestClientOptions(SmartsheetBuilder.DEFAULT_BASE_URI) {
    Proxy = new WebProxy("localhost", 8888)
});

// Initialize client with the custom RestClient
SmartsheetClient smartsheet = new SmartsheetBuilder()
    .SetHttpClient(new DefaultHttpClient(client, new JsonNetSerializer()))
    .Build();
``` 

### Sample RetryHttpClient
The following example shows how to override the default retry/timeout logic.  

Invoke the SmartsheetBuilder with a custom HttpClient:
```csharp
// Initialize client
SmartsheetClient smartsheet = new SmartsheetBuilder()
    .SetHttpClient(new RetryHttpClient())
    .Build();
```

```csharp
using System;
using System.IO;
using System.Threading;
using Smartsheet.Api;
using Smartsheet.Api.Models;
using Smartsheet.Api.Internal.Http;
using Newtonsoft.Json;

namespace sdk_csharp_sample
{
    class RetryHttpClient : DefaultHttpClient
    {
        /// <summary>
        /// Override this method to perform API requests for special cases
        /// </summary>
        /// <param name="previousAttempts"> number of previous attempts </param>
        /// <param name="totalElapsedTime"> the total elapsed time for the API request </param>
        /// <param name="response"> the last response from the API </param>
        /// <returns> true to retry, false to exit and return error to the caller </returns>
        public override bool ShouldRetry(int previousAttempts, long totalElapsedTime, HttpResponse response)
        {
            string contentType = response.Entity.ContentType;
            if (contentType != null && !contentType.StartsWith("application/json"))
            {
                // it's not JSON; don't try to parse it
                return false;
            }

            Error error;
            try
            {
                // Details about the Smartsheet API error condition
                error = jsonSerializer.deserialize<Error>(
                    response.Entity.GetContent());
            }
            catch (JsonSerializationException ex)
            {
                throw new SmartsheetException(ex);
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                throw new SmartsheetException(ex);
            }
            catch (IOException ex)
            {
                throw new SmartsheetException(ex);
            }

            switch (error.ErrorCode)
            {
                // The default shouldRetry, retries 4001, 4002, 4003, 4004 codes
                case 4001:
                case 4002:
                case 4003:
                case 4004:
                case 9999: // adding my fictional error code to the retry list
                    break;
                default:
                    return false;
            }

            // The default calcBackoff uses exponential backoff, add custom behavior by overriding calcBackoff
            long backoff = CalcBackoff(previousAttempts, totalElapsedTime, error);
            if (backoff < 0)
                return false;

            logger.Info(string.Format("HttpError StatusCode={0}: Retrying in {1} milliseconds", response.StatusCode, backoff));
            Thread.Sleep(TimeSpan.FromMilliseconds(backoff));
            return true;
        }
    }
}
```

## Event Reporting
The following sample demonstrates best practices for consuming the event stream from the Smartsheet Event Reporting
feature.

The sample uses the `smartsheet.EventResources.ListEvents` method to request a list of events from the stream. The
first request sets the `since` parameter with the point in time (i.e. event occurrence datetime) in the stream from 
which to start consuming events. The `since` parameter can be set with a datetime value that is either formatted as 
ISO 8601 (e.g. 2010-01-01T00:00:00Z) or as UNIX epoch (in which case the `numericDates` parameter must also be set to 
`true`. By default the `numericDates` parameter is set to `false`).

To consume the next list of events after the initial list of events is returned, set the `streamPosition` parameter 
with the `NextStreamPosition` property obtained from the previous request and don't set the `since` parameter with 
any values. This is because when using the `ListEvents` method, either the `since` parameter or the `streamPosition`
parameter should be set, but never both.

Note that the `MoreAvailable` property in a response indicates whether more events are immediately available for
consumption. If events are not immediately available, they may still be generating so subsequent requests should keep
using the same `NextStreamPosition` value until the next list of events is retrieved.

Many events have additional information available as part of the event. That information can be accessed using the 
Dictionary stored in the `AdditionalDetails` property. Information about the additional details provided can be found
[here.](https://smartsheet.redoc.ly/tag/eventsDescription)

```csharp
class Program
{
    // this example is looking specifically for new sheet events
    private static void PrintNewSheetEventsInList(IList<Event> events)
    {
        //  enumerate all events in the list of returned events
        foreach (Event _event in events)
        {
            // find all created sheets
            if (_event.ObjectType == EventObjectType.SHEET && _event.Action == EventAction.CREATE)
            {
                // additional details are available for some events, they can be accessed as a Dictionary
                // in the AdditionalDetails property
                if (_event.AdditionalDetails.ContainsKey("sheetName"))
                {
                    Console.WriteLine(_event.AdditionalDetails["sheetName"]);
                }
            }
        }
    }

    static void Main(string[] args)
    {
        // Initialize client
        SmartsheetClient smartsheet = new SmartsheetBuilder().Build();

        // begin listing events in the stream starting with the `since` parameter
        DateTime lastWeek = DateTime.Today.AddDays(-7);
        // this example looks at the previous 7 days of events by providing a since argument set to last week's date 
        EventResult eventResult = smartsheet.EventResources.ListEvents(lastWeek, null, 1000, false);
        PrintNewSheetEventsInList(eventResult.Data);

        // continue listing events in the stream by using the `StreamPosition`, if the previous response indicates 
        // that more data is available.
        while(eventResult.MoreAvailable == true)
        {
            eventResult = smartsheet.EventResources.ListEvents(null, eventResult.NextStreamPosition, 10000, true);
            PrintNewSheetEventsInList(eventResult.Data);
        }
    }    
}
```

## Working With Smartsheetgov.com Accounts

If you need to access Smartsheetgov you will need to specify the Smartsheetgov API URI as the base URI during creation of the Smartsheet client object. SmartsheetGov uses a base URI of https://api.smartsheetgov.com/2.0/. The base URI is defined as a constant in the SmartsheetBuilder class (i.e. `SmartsheetBuilder.GOV_BASE_URI`).

Invoke the SmartsheetBuilder with the base URI pointing to Smartsheetgov:

```csharp
using Smartsheet.Api;
using Smartsheet.Api.Models;

static void Sample()
{
    // Initialize client
    SmartsheetClient smartsheet = new SmartsheetBuilder()
        .SetBaseURI(SmartsheetBuilder.GOV_BASE_URI)
        // TODO: Set your API access in environment variable SMARTSHEET_ACCESS_TOKEN or else here
        // .SetAccessToken("ll352u9jujauoqz4gstvsae05")
        .Build();

    // List all sheets
    PaginatedResult<Sheet> sheets = smartsheet.SheetResources.ListSheets(
        null,               // IEnumerable<SheetInclusion> includes
        null,               // PaginationParameters
        null                // Nullable<DateTime> modifiedSince = null
    );
    Console.WriteLine("Found " + sheets.TotalCount + " sheets");

    long sheetId = (long) sheets.Data[0].Id;                // Default to first sheet

    // sheetId = 567034672138842;                         // TODO: Uncomment if you wish to read a specific sheet

    Console.WriteLine("Loading sheet id: " + sheetId);

    // Load the entire sheet
    var sheet = smartsheet.SheetResources.GetSheet(
        5670346721388420,           // long sheetId
        null,                       // IEnumerable<SheetLevelInclusion> includes
        null,                       // IEnumerable<SheetLevelExclusion> excludes
        null,                       // IEnumerable<long> rowIds
        null,                       // IEnumerable<int> rowNumbers
        null,                       // IEnumerable<long> columnIds
        null,                       // Nullable<long> pageSize
        null                        // Nullable<long> page
    );
    Console.WriteLine("Loaded " + sheet.Rows.Count + " rows from sheet: " + sheet.Name);
}

```

## Working With Smartsheet Regions Europe Accounts

If you need to access Smartsheet Regions Europe you will need to specify the Smartsheet.eu API URI as the base URI during creation of the Smartsheet client object. Smartsheet.eu uses a base URI of https://api.smartsheet.eu/2.0/. The base URI is defined as a constant in the SmartsheetBuilder class (i.e. `SmartsheetBuilder.EU_BASE_URI`).

Invoke the SmartsheetBuilder with the base URI pointing to Smartsheet.eu:

```csharp
using Smartsheet.Api;
using Smartsheet.Api.Models;

static void Sample()
{
    // Initialize client
    SmartsheetClient smartsheet = new SmartsheetBuilder()
        .SetBaseURI(SmartsheetBuilder.EU_BASE_URI)
        // TODO: Set your API access in environment variable SMARTSHEET_ACCESS_TOKEN or else here
        // .SetAccessToken("ll352u9jujauoqz4gstvsae05")
        .Build();
}
```

## Preserving Decimal Precision with DecimalObjectValue

By default, the SDK deserializes numeric cell values as `NumberObjectValue` which stores values as `double`, potentially losing precision for certain decimal values. To preserve full decimal precision, you can enable `DecimalObjectValue` mode.

### Enabling DecimalObjectValue

To enable `DecimalObjectValue` mode, use the `SetEnableDecimalObjectValue` method when building your Smartsheet client:

```csharp
using Smartsheet.Api;
using Smartsheet.Api.Models;

// Initialize client with DecimalObjectValue enabled
SmartsheetClient smartsheet = new SmartsheetBuilder()
    .SetEnableDecimalObjectValue(true)
    .Build();
```

### Important Considerations

**Breaking Change Warning:** Enabling `DecimalObjectValue` is a breaking change if your application currently casts cell values to `NumberObjectValue`. You will need to update your code to handle `DecimalObjectValue` instead.

**Thread Safety:** The `EnableDecimalObjectValue` setting modifies a shared static serializer. It is not thread-safe during reconfiguration and affects all Smartsheet client instances in your application.

**When to Use:** Enable this feature if you:
- Work with financial data or other values requiring exact decimal precision
- Need to preserve the exact decimal representation received from the Smartsheet API
- Can update your code to handle `DecimalObjectValue` instead of `NumberObjectValue`

### Example: Working with DecimalObjectValue

```csharp
// Initialize client with decimal precision enabled
SmartsheetClient smartsheet = new SmartsheetBuilder()
    .SetEnableDecimalObjectValue(true)
    .Build();

// Get a sheet
Sheet sheet = smartsheet.SheetResources.GetSheet(sheetId, null, null, null, null, null, null, null);

// Access cell values with decimal precision
foreach (Row row in sheet.Rows)
{
    foreach (Cell cell in row.Cells)
    {
        if (cell.ObjectValue is DecimalObjectValue decimalValue)
        {
            // Access the decimal value with full precision
            decimal value = decimalValue.Value;
            Console.WriteLine($"Decimal value: {value}");
        }
    }
}
```
