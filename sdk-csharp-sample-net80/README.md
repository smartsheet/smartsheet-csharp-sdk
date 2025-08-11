# Smartsheet C# SDK Examples

This project contains multiple examples demonstrating how to use the Smartsheet C# SDK.

## Setup

1. Set your Smartsheet access token in the environment variable:
   ```bash
   export SMARTSHEET_ACCESS_TOKEN=your_token_here
   ```

2. Build the project:
   ```bash
   dotnet build
   ```

3. Run the examples:
   ```bash
   dotnet run
   ```

## Available Examples

### 1. Simple Example
Basic demonstration of listing sheets and reading row data using traditional pagination.

### 2. Token Pagination Example (NEW!)
Comprehensive demonstration of the new token-based pagination features for workspace operations:
- Basic token pagination with `maxItems` parameter
- Multi-page navigation using `lastKey` tokens
- Comparison between traditional offset-based and new token-based pagination
- Error handling for invalid parameters

### 3. HTTP Client Example
Shows how to use a custom HTTP client implementation with the SDK.

## Token Pagination Features

The new token pagination system provides several advantages:

- **Efficient paging**: Uses `lastKey` tokens instead of offset calculations
- **Consistent results**: Prevents issues with data changes during pagination
- **Flexible parameters**: Support for `maxItems`, `paginationType`, and `lastKey`
- **Backward compatibility**: Traditional pagination still works alongside token pagination

### Example Usage

```csharp
// Create token pagination parameters
TokenPaginationParameters tokenParams = new TokenPaginationParameters(null, 10);

// Get first page
TokenPaginatedResult<Workspace> result = smartsheet.WorkspaceResources.ListWorkspaces(tokenParams);

// Navigate to next page using lastKey
if (result.LastKey != null)
{
    TokenPaginationParameters nextPageParams = new TokenPaginationParameters(result.LastKey, 10);
    TokenPaginatedResult<Workspace> nextPage = smartsheet.WorkspaceResources.ListWorkspaces(nextPageParams);
}
```

## Screenshots and Sample Output

When you run the Token Pagination Example (option 2), you'll see output like:

```
1. Demonstrating Token-Based Pagination for Workspaces
======================================================
Fetching workspaces with token pagination (maxItems=10)...

✓ Retrieved 5 workspaces
✓ Last Key: abc123def456 (or null if last page)
✓ Pagination Type: token-based

Workspace Details:
------------------
  • ID: 12345, Name: 'Project Alpha'
  • ID: 12346, Name: 'Marketing Campaign'
  ...
```

This provides a practical demonstration of the new pagination features that can be referenced when troubleshooting user implementations.