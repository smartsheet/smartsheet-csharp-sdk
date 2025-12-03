using System;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace TestSmartsheetSDK
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing Smartsheet C# SDK with .NET Core 3.1");
            Console.WriteLine("==============================================");
            
            // Check if access token is provided
            if (args.Length == 0)
            {
                Console.WriteLine("\nUsage: dotnet run <access_token>");
                Console.WriteLine("\nThis test app will:");
                Console.WriteLine("1. Initialize the Smartsheet client");
                Console.WriteLine("2. Attempt to list the user's sheets");
                Console.WriteLine("\nPlease provide your Smartsheet access token as a command line argument.");
                return;
            }

            string accessToken = args[0];

            try
            {
                // Initialize the Smartsheet client
                Console.WriteLine("\n1. Initializing Smartsheet client...");
                SmartsheetClient smartsheet = new SmartsheetBuilder()
                    .SetAccessToken(accessToken)
                    .Build();
                Console.WriteLine("   ✓ Client initialized successfully");

                // Test: List user's sheets
                Console.WriteLine("\n2. Fetching user's sheets...");
                PaginatedResult<Sheet> sheets = smartsheet.SheetResources.ListSheets(
                    null,  // includes
                    null,  // pagination parameters
                    null   // modified since
                );
                
                Console.WriteLine($"   ✓ Successfully retrieved {sheets.TotalCount} sheet(s)");
                
                if (sheets.Data != null && sheets.Data.Count > 0)
                {
                    Console.WriteLine("\n   First 5 sheets:");
                    int count = 0;
                    foreach (var sheet in sheets.Data)
                    {
                        if (count >= 5) break;
                        Console.WriteLine($"   - {sheet.Name} (ID: {sheet.Id})");
                        count++;
                    }
                }
                else
                {
                    Console.WriteLine("   No sheets found in the account.");
                }

                Console.WriteLine("\n✓ Test completed successfully!");
                Console.WriteLine("\nThe SDK is working correctly with .NET Core 3.1");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error occurred: {ex.Message}");
                Console.WriteLine($"\nStack trace:\n{ex.StackTrace}");
                Environment.Exit(1);
            }
        }
    }
}
