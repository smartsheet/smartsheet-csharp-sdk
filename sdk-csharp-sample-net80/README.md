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

3. Run a specific example (see instructions below for each file):
   ```bash
   # Run the default program (currently set to SimpleExample)
   dotnet run
   
   # Or run a specific .cs file by setting it as the startup file
   dotnet run --project sdk-csharp-sample-net80.csproj
   ```

## How to Run Individual Examples

Each .cs file contains a separate example with its own Main method. To run a specific example:

### Method 1: Modify the project file
Edit `sdk-csharp-sample-net80.csproj` and change the `<StartupObject>` to point to the desired class:

```xml
<PropertyGroup>
  <StartupObject>sdk_csharp_simpleExample.SimpleExample</StartupObject>
</PropertyGroup>
```


## Available Examples

### Running Each Example:

**To run SimpleExample.cs:**
- Set `<StartupObject>sdk_csharp_simpleExample.SimpleExample</StartupObject>` in project file
- Or run: `dotnet run` (if it's the default)

**To run ExampleWithHttpClientDefined.cs:**
- Set `<StartupObject>sdk_csharp_sample.ExampleWithHttpClientDefined</StartupObject>` in project file
- Then run: `dotnet run`

**To run TokenPaginationExample.cs:**
- Set `<StartupObject>sdk_csharp_tokenPaginationExample.TokenPaginationExample</StartupObject>` in project file  
- Then run: `dotnet run`

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