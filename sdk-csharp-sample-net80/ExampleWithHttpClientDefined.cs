// Add nuget reference to smartsheet-csharp-sdk (https://www.nuget.org/packages/smartsheet-csharp-sdk/)
using Smartsheet.Api;
using Smartsheet.Api.Models;
using Smartsheet.Api.Internal.Http;

namespace sdk_csharp_sample
{
    class ExampleWithHttpClientDefined
    {
        public static void Main(string[] args)
        {
            // Initialize client with explicitly defined HttpClient
            // This example shows how to set a custom HttpClient implementation
            Console.WriteLine("ExampleWithHttpClientDefined - Smartsheet API C# SDK");
            Console.WriteLine("========================================="); 
            SmartsheetClient smartsheet = new SmartsheetBuilder()
                .SetAccessToken("7qJcdNyfy2McIMHeeumgzClqHCxhunFJYt4Qz")       // TODO: Set your API access in environment variable SMARTSHEET_ACCESS_TOKEN or else here
                .SetHttpClient(new RetryHttpClient())                          // Explicitly setting a custom HttpClient
                .Build();

            // For this example, we'll use the client with the custom RetryHttpClient
            
            // List all sheets
            PaginatedResult<Sheet> sheets = smartsheet.SheetResources.ListSheets(new List<SheetInclusion> { SheetInclusion.SHEET_VERSION });
            Console.WriteLine("Found " + sheets.TotalCount + " sheets");

            if (sheets.TotalCount > 0)
            {
                long sheetId = (long)sheets.Data[0].Id;                // Default first sheet

                sheetId = 5192468317661060;                         // TODO: Uncomment if you wish to read a specific sheet

                Console.WriteLine("Loading sheet id: " + sheetId);

                // Load the entire sheet
                var sheet = smartsheet.SheetResources.GetSheet(sheetId);
                Console.WriteLine("Loaded " + sheet.Rows.Count + " rows from sheet: " + sheet.Name);

                // Display the first 5 rows
                foreach (Row row in sheet.Rows.Take(5))
                {
                    dumpRow(row, sheet.Columns);
                }
            }

            Console.WriteLine("Done (Hit enter)");                      // Keep console window open
            Console.ReadLine();
        }

        // Display row contents
        static void dumpRow(Row row, IList<Column> columns)
        {
            Console.WriteLine("Row # " + row.RowNumber + ":");
            foreach (var cell in row.Cells)
            {
                // Find column name by Id in column collection
                var columName = columns.First(column => column.Id == cell.ColumnId).Title;
                Console.WriteLine("    " + columName + ": " + cell.Value);
            }
        }
    }
}