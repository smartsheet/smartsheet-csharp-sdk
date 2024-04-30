# Smartsheet SDK for C# [![Build Status](https://github.com/smartsheet/smartsheet-csharp-sdk/actions/workflows/main.yml/badge.svg)](https://github.com/smartsheet/smartsheet-csharp-sdk/actions/workflows/main.yml) [![NuGet](https://img.shields.io/nuget/v/smartsheet-csharp-sdk.svg)](https://www.nuget.org/packages/smartsheet-csharp-sdk/)

C# SDK to simplify connecting to the [Smartsheet API](https://smartsheet.redoc.ly) from .NET applications.
   
## System Requirements

The SDK supports C# version 4.0 or later and targets .NET 6.0.

## Installation

To add the SDK to a .Net project

```dos
dotnet add package smartsheet-csharp-sdk
```

## Example Usage
To call the API, you will need an *access token*, which looks something like this example: ll352u9jujauoqz4gstvsae05. You can find the access token in the UI at Account > Personal Settings > API Access.

The following is a brief sample that shows you how to:

* Initialize the client
* List all sheets
* Load one sheet

```csharp
using Smartsheet.Api;
using Smartsheet.Api.Models;

static void Sample()
{
    SmartsheetClient smartsheet = new SmartsheetBuilder()
        // TODO: Set your API access in environment variable SMARTSHEET_ACCESS_TOKEN or else here
        // .SetAccessToken("JKlMNOpQ12RStUVwxYZAbcde3F5g6hijklM789")
        .Build();

    PaginatedResult<Sheet> sheets = smartsheet.SheetResources.ListSheets(null, null, null);
    Console.WriteLine("Found " + sheets.TotalCount + " sheets");

    long sheetId = (long) sheets.Data[0].Id;

    Console.WriteLine("Loading sheet id: " + sheetId);

    var sheet = smartsheet.SheetResources.GetSheet(sheetId);
    Console.WriteLine("Loaded " + sheet.Rows.Count + " rows from sheet: " + sheet.Name);
}
```
A simple, but complete sample application project is here: https://github.com/smartsheet-samples/csharp-read-write-sheet

## Advanced Topics
For details about logging, testing, how to use a passthrough option, and how to override HTTP client behavior, see [Advanced Topics](ADVANCED.md).

## Documentation
The full Smartsheet API documentation is here: https://smartsheet.redoc.ly.

The generated SDK class documentation is here: [https://smartsheet.github.io/smartsheet-csharp-sdk/api/index.html](https://smartsheet.github.io/smartsheet-csharp-sdk/api/index.html).

## Release Notes

All releases and release notes are available on [Github](https://github.com/smartsheet/smartsheet-csharp-sdk/releases) or the [NuGet repository](https://www.nuget.org/packages/smartsheet-csharp-sdk/).

## Developer Agreement

Review the [Developer Program Agreement](https://www.smartsheet.com/legal/developer-program-agreement).

## Acknowledgements

We would like to thank the following people for their contributions to this project:

* Eric Yang - [ericyan1](https://github.com/EricYan1)
* Tim Wells - [timwellswa](https://github.com/timwellswa)
* Kevin Fansler - [kfansler](https://github.com/kfansler)
* Brett Batie - [brettbatie](https://github.com/brettbatie)
* Steve Weil - [seweil](https://github.com/seweil)
