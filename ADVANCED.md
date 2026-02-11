# Advanced Topics for the Smartsheet SDK for C#

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

## Testing

Mock API tests:

We use WireMock for API contract testing. This allows us to simulate Smartsheet API responses and run tests without relying on the live API.
The [smartsheet-sdk-tests](https://github.com/smartsheet/smartsheet-sdk-tests) repo provides a standalone WireMock server with JSON mappings that simulate the Smartsheet API.
Each mapping defines a request to match and a response to return.

Common test cases use catch-all path patterns (e.g., /errors/500-response).

We use two custom headers:

- x-test-name: Used for exact mapping match, allowing different mock responses for the same HTTP method and endpoint.
- x-request-id: A UUID generated for each request, used to verify request URLs and search for requests in WireMock admin history.

To run the mock API tests:
1. Clone the [smartsheet-sdk-tests](https://github.com/smartsheet/smartsheet-sdk-tests) repo and follow the instructions from the readme to start the mock server.
2. `dotnet test mock-api-test-sdk-net80/mock-api-test-sdk-net80.csproj`

To add new mock API tests:

1. Add a WireMock Mapping (JSON) in the [smartsheet-sdk-tests](https://github.com/smartsheet/smartsheet-sdk-tests):
```json
{
    "request": {
        "urlPathTemplate": "/2.0/users/{userId}/plans",
        "method": "GET",
        "headers": {
            "Authorization": {
                "matches": "Bearer .*"
            },
            "x-test-name": {
                "equalTo": "/users/list-user-plans/all-response-body-properties"
            },
            "x-request-id": {
                "matches" : ".*"
            }
        }
    },
    "response": {
        "statusMessage": "OK",
        "status": 200,
        "jsonBody": {
            "lastKey": "12345678901234569",
            "data": [
                {
                    "planId": 1234567890123456,
                    "seatType": "MEMBER",
                    "seatTypeLastChangedAt": "2025-01-01T00:00:00.123456789Z",
                    "provisionalExpirationDate": "2026-12-13T12:17:52.525696Z",
                    "isInternal": false
                }
            ]
        },
        "headers": {
            "Content-Type": "application/json"
        }
    }
}
```
2. Write a Test in the SDK:

- Always use x-test-name to target specific mock responses.
- Use x-request-id for traceability in WireMock admin.
- Keep mappings in the smartsheet-sdk-tests repository organized and descriptive

```csharp
    [TestMethod]
    public void TestListUserPlansAllResponseBodyProperties()
    {
        Guid requestId = Guid.NewGuid();
        SmartsheetClient smartsheet = HelperFunctions.SetupClient("/users/list-user-plans/all-response-body-properties", requestId.ToString());

        TokenPaginatedResult<UserPlan> response = smartsheet.UserResources.ListUserPlans(CommonTestConstants.TEST_USER_ID, TEST_LAST_KEY, TEST_MAX_ITEMS);

        Assert.IsNotNull(response);
    }
```

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
