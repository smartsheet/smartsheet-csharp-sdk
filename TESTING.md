# Testing Guide

This guide establishes the mandatory patterns for mock API testing in the Smartsheet C# SDK.

## Table of Contents

- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Running Mock API Tests](#running-mock-api-tests)
- [Mock API Test Standards](#mock-api-test-standards)
  - [Standardized Test Cases](#standardized-test-cases)
  - [Key Principles](#key-principles)
  - [Test Suite Structure](#test-suite-structure)
- [Additional Rules](#additional-rules)
- [Helper Functions](#helper-functions)

---

## Getting Started

### Prerequisites

Mock API tests require WireMock running locally on port 8082. The WireMock server provides simulated API responses for contract testing without hitting the live Smartsheet API.

For WireMock setup instructions, mapping documentation, and details on `x-request-id` and `x-test-name` header usage, see the [smartsheet-sdk-tests](https://github.com/smartsheet/smartsheet-sdk-tests) repository.

For complete test examples, see [mock-api-test-sdk-net80/reports/](mock-api-test-sdk-net80/reports/).

### Running Mock API Tests

| Command | Purpose |
|---------|---------|
| `dotnet test mock-api-test-sdk-net80/mock-api-test-sdk-net80.csproj` | Run all tests |
| `dotnet test mock-api-test-sdk-net80/mock-api-test-sdk-net80.csproj --filter "FullyQualifiedName~TestCreateReport"` | Run specific test suite |
| `dotnet test mock-api-test-sdk-net80/mock-api-test-sdk-net80.csproj --filter "FullyQualifiedName~TestCreateReportGeneratedUrlIsCorrect"` | Run specific test |

---

## Mock API Test Standards

### Standardized Test Cases

Every endpoint must implement these test cases (using PascalCase naming for C#):

**Required Tests:**

1. **`Test<Operation>GeneratedUrlIsCorrect`**
   - Asserts request method
   - Asserts URL path
   - Asserts query parameters (even if empty — use `CollectionAssert.AreEquivalent` with an empty dictionary)
   - Does NOT assert request or response body

2. **`Test<Operation>AllResponseBodyProperties`**
   - Asserts request body always (for POST/PUT/PATCH, assert the serialized object; for GET/DELETE, assert empty/null body)
   - Asserts response body with all properties
   - Does NOT assert method, URL, or query parameters

3. **`Test<Operation>Error400Response`**
   - Asserts ONLY that SDK throws `SmartsheetException` with expected client error message

4. **`Test<Operation>Error500Response`**
   - Asserts ONLY that SDK throws `SmartsheetException` with expected server error message

**Optional Tests:**

- **`Test<Operation>RequiredResponseBodyProperties`** — Include ONLY if a corresponding WireMock mapping exists for the required-properties variant. Asserts request body and minimal response body.
- **Endpoint-specific tests** — Additional tests for unique endpoint behaviors (e.g., scope variants, pagination edge cases).

### Key Principles

#### Full-Object Assertions

Assert objects as a whole, never property-by-property. This ensures extra or missing properties cause test failures.

- **Query parameters:** Assert the entire query parameter dictionary using `CollectionAssert.AreEquivalent`
- **Request body:** Assert the entire serialized request body using `JsonConvert.SerializeObject`
- **Response body:** Assert the entire deserialized response object using `JsonConvert.SerializeObject` comparison or direct `Assert.AreEqual` (for types with overridden `Equals`)

#### Test Constants

- **Cross-file constants:** Use `CommonTestConstants` for IDs shared across test files (e.g., `TEST_USER_ID`, `TEST_REPORT_ID`)
- **File-scoped constants:** Define expected responses, request bodies, and query params as `private const` or `private static readonly` fields at the top of each test class

#### WireMock Integration

Each test uses custom headers for WireMock integration:

- **`x-request-id`:** UUID for request tracking (retrieve via `WiremockHelper.FindWiremockRequestAsync`)
- **`x-test-name`:** Targets a specific WireMock mapping (e.g., `/reports/create-report/all-response-body-properties`)

See [smartsheet-sdk-tests](https://github.com/smartsheet/smartsheet-sdk-tests) for WireMock mapping conventions and header usage details.

### Test Suite Structure

- **One test file per endpoint:** `mock-api-test-sdk-net80/<resource>/Test<OperationName>.cs`
- **One constants file per resource:** `mock-api-test-sdk-net80/<resource>/CommonTestConstants.cs` (or the shared `users/CommonTestConstants.cs` for cross-resource IDs)
- **Gold standard examples:** See Reports tests in `mock-api-test-sdk-net80/reports/`

---

## Additional Rules

- **DateTime parsing:** Always use `DateTimeStyles.RoundtripKind` when parsing ISO 8601 strings to preserve UTC timezone and precision
- **Async tests:** Use `public async Task` (not `void`) for any test that calls `WiremockHelper.FindWiremockRequestAsync` — this includes all URL correctness tests and POST/PUT/PATCH response body tests that validate the request body
- **Assertion format:** Use `JsonConvert.SerializeObject` for comparing request/response bodies. Define expected request bodies as `JsonConvert.SerializeObject(new Dictionary<string, object>{...})` and compare against `foundRequest.Body`. Compare response objects with `JsonConvert.SerializeObject(expectedObject)` vs `JsonConvert.SerializeObject(result)`

---

## Helper Functions

**`HelperFunctions` (`mock-api-test-sdk-net80/HelperFunctions.cs`):**

- **`SetupClient(string testName, string requestId)`** — Creates a `SmartsheetClient` pointing at WireMock (`http://localhost:8082/2.0/`) with `x-test-name` and `x-request-id` headers set

**`WiremockHelper` (`mock-api-test-sdk-net80/WiremockHelper.cs`):**

- **`FindWiremockRequestAsync(string requestId)`** — Queries WireMock admin API and returns a `LogModel` with `AbsoluteUrl`, `Method`, `Body`, and `Headers` for the request matching the given `x-request-id`

**`CommonTestConstants` (`mock-api-test-sdk-net80/users/CommonTestConstants.cs`):**

- Shared IDs used across test classes: `TEST_USER_ID`, `TEST_PLAN_ID`, `TEST_SHEET_ID`, `TEST_WORKSPACE_ID`, `TEST_REPORT_ID`
