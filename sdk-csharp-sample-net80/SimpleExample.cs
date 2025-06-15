// Add nuget reference to smartsheet-csharp-sdk (https://www.nuget.org/packages/smartsheet-csharp-sdk/)
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace sdk_csharp_simpleExample
{
    class SimpleExample
    {
        public static void Main(string[] args)
        {
            // Initialize client without setting HttpClient explicitly
            // When no HttpClient is provided, SmartsheetBuilder automatically uses DefaultHttpClient
            Console.WriteLine("Simple Example - Smartsheet API C# SDK");
            Console.WriteLine("========================================="); 
            SmartsheetClient smartsheet = new SmartsheetBuilder()
                .SetAccessToken("7qJcdNyfy2McIMHeeumgzClqHCxhunFJYt4Qz")       // TODO: Set your API access in environment variable SMARTSHEET_ACCESS_TOKEN or else here
                // No HttpClient set - DefaultHttpClient will be used automatically
                .Build();
            
            // The DefaultHttpClient provides standard HTTP functionality with built-in
            // handling for rate limiting, retry logic, and proper error handling

            // List all sheets
            PaginatedResult<Sheet> sheets = smartsheet.SheetResources.ListSheets(new List<SheetInclusion> { SheetInclusion.SHEET_VERSION });
            Console.WriteLine("Found " + sheets.TotalCount + " sheets");

            if (sheets.TotalCount > 0)
            {
                long sheetId = sheets.Data[0].Id!.Value;                // Default first sheet

                sheetId = 5192468317661060;                        

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

