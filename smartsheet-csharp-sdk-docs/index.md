# Smartsheet C# SDK

Welcome to the Smartsheet C# SDK API documentation. This library provides a simple and intuitive interface for interacting with the Smartsheet API using C#.

## Overview

The Smartsheet C# SDK allows you to integrate Smartsheet functionality into your .NET applications. It provides strongly-typed models and a comprehensive set of resources for managing sheets, rows, columns, attachments, and more.

## Installation

Install the SDK via NuGet Package Manager:

```bash
Install-Package smartsheet-csharp-sdk
```

Or using the .NET CLI:

```bash
dotnet add package smartsheet-csharp-sdk
```

## Quick Start

```csharp
using Smartsheet.Api;
using Smartsheet.Api.Models;

// Initialize the client
SmartsheetClient smartsheet = new SmartsheetBuilder()
    .SetAccessToken("YOUR_ACCESS_TOKEN")
    .Build();

// Get a sheet
Sheet sheet = smartsheet.SheetResources.GetSheet(sheetId, null, null, null, null, null, null, null);

// List all sheets
PaginatedResult<Sheet> sheets = smartsheet.SheetResources.ListSheets(null, null, null);
```

## Key Features

- **Comprehensive API Coverage**: Full support for Smartsheet API operations
- **Strongly Typed Models**: Type-safe access to all Smartsheet objects
- **Builder Pattern**: Intuitive builders for creating and updating objects
- **Error Handling**: Robust exception handling for API errors
- **OAuth Support**: Built-in OAuth 2.0 authentication flow

## API Reference

Browse the complete API reference to explore all available classes, methods, and models:

- [Smartsheet.Api](api/Smartsheet.Api.yml) - Core API interfaces and resources
- [Smartsheet.Api.Models](api/Smartsheet.Api.Models.yml) - Data models and types
- [Smartsheet.Api.OAuth](api/Smartsheet.Api.OAuth.yml) - OAuth authentication

## Resources

- [GitHub Repository](https://github.com/smartsheet/smartsheet-csharp-sdk)
- [Smartsheet API Documentation](https://developers.smartsheet.com)
- [NuGet Package](https://www.nuget.org/packages/smartsheet-csharp-sdk)

## Support

For issues, questions, or contributions, please visit our [GitHub Issues](https://github.com/smartsheet/smartsheet-csharp-sdk/issues) page.
