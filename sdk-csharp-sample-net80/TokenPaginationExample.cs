// Add nuget reference to smartsheet-csharp-sdk (https://www.nuget.org/packages/smartsheet-csharp-sdk/)
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace sdk_csharp_tokenPaginationExample
{
    class TokenPaginationExample
    {
        public static void RunExample(string[] args)
        {
            Console.WriteLine("Token Pagination Example - Smartsheet API C# SDK");
            Console.WriteLine("=================================================");
            
            // Initialize client - Set your access token in environment variable SMARTSHEET_ACCESS_TOKEN
            SmartsheetClient smartsheet = new SmartsheetBuilder()
                .SetAccessToken(Environment.GetEnvironmentVariable("SMARTSHEET_ACCESS_TOKEN") ?? "your_token_here")
                .Build();

            try
            {
                Console.WriteLine("\n1. Demonstrating Token-Based Pagination for Workspaces");
                Console.WriteLine("======================================================");
                
                DemonstrateBasicTokenPagination(smartsheet);
                
                Console.WriteLine("\n2. Demonstrating Multi-Page Token Pagination");
                Console.WriteLine("============================================");
                
                DemonstrateMultiPageTokenPagination(smartsheet);
                
                Console.WriteLine("\n3. Comparing Traditional vs Token Pagination");
                Console.WriteLine("============================================");
                
                CompareTraditionalVsTokenPagination(smartsheet);
                
            }
            catch (SmartsheetException ex)
            {
                Console.WriteLine($"Smartsheet API Error: {ex.Message}");
                if (ex.Message.Contains("Access Token"))
                {
                    Console.WriteLine("Please set SMARTSHEET_ACCESS_TOKEN environment variable with a valid token");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nDemo completed. Press Enter to exit...");
            Console.ReadLine();
        }

        static void DemonstrateBasicTokenPagination(SmartsheetClient smartsheet)
        {
            Console.WriteLine("Fetching workspaces with token pagination (maxItems=100)...\n");
            
            // Create token pagination parameters
            TokenPaginationParameters tokenParams = new TokenPaginationParameters(null, 100);
            
            // Get first page using token pagination
            TokenPaginatedResult<Workspace> result = smartsheet.WorkspaceResources.ListWorkspaces(tokenParams);
            
            Console.WriteLine($"✓ Retrieved {result.Data?.Count ?? 0} workspaces");
            Console.WriteLine($"✓ Last Key: {result.LastKey ?? "null (last page)"}");
            Console.WriteLine($"✓ Pagination Type: token-based");
            
            // Display workspace details
            if (result.Data != null && result.Data.Count > 0)
            {
                Console.WriteLine("\nWorkspace Details:");
                Console.WriteLine("------------------");
                foreach (var workspace in result.Data.Take(5)) // Show first 5
                {
                    Console.WriteLine($"  • ID: {workspace.Id}, Name: '{workspace.Name}'");
                }
                
                if (result.Data.Count > 5)
                {
                    Console.WriteLine($"  ... and {result.Data.Count - 5} more workspaces");
                }
            }
        }

        static void DemonstrateMultiPageTokenPagination(SmartsheetClient smartsheet)
        {
            Console.WriteLine("Demonstrating multi-page token pagination (maxItems=100)...\n");
            
            int pageNumber = 1;
            int totalWorkspaces = 0;
            string? lastKey = null;
            
            do
            {
                // Create pagination parameters with lastKey from previous page
                TokenPaginationParameters tokenParams = new TokenPaginationParameters(lastKey, 100);
                
                // Get page
                TokenPaginatedResult<Workspace> result = smartsheet.WorkspaceResources.ListWorkspaces(tokenParams);
                
                Console.WriteLine($"Page {pageNumber}:");
                Console.WriteLine($"  ├─ Retrieved: {result.Data?.Count ?? 0} workspaces");
                Console.WriteLine($"  ├─ Last Key: {result.LastKey ?? "null (final page)"}");
                
                if (result.Data != null && result.Data.Count > 0)
                {
                    foreach (var workspace in result.Data)
                    {
                        Console.WriteLine($"  │   • {workspace.Name} (ID: {workspace.Id})");
                        totalWorkspaces++;
                    }
                }
                
                // Update lastKey for next iteration
                lastKey = result.LastKey;
                pageNumber++;
                
                Console.WriteLine($"  └─ Total so far: {totalWorkspaces} workspaces\n");
                
                // Break if we've fetched enough pages for demo
                if (pageNumber > 3 || lastKey == null) break;
                
                // Small delay for demo purposes
                System.Threading.Thread.Sleep(500);
                
            } while (lastKey != null);
            
            Console.WriteLine($"✓ Total workspaces found: {totalWorkspaces}");
            Console.WriteLine($"✓ Pages processed: {pageNumber - 1}");
        }

        static void CompareTraditionalVsTokenPagination(SmartsheetClient smartsheet)
        {
            Console.WriteLine("Comparing traditional offset vs token-based pagination...\n");
            
            // 1. Traditional offset-based pagination
            Console.WriteLine("Traditional Pagination (PaginationParameters):");
            var traditionalParams = new PaginationParameters(false, 100, 0); // page size 5, offset 0
            var traditionalResult = smartsheet.WorkspaceResources.ListWorkspaces(traditionalParams);
            
            Console.WriteLine($"  ├─ Type: Offset-based");
            Console.WriteLine($"  ├─ Page Size: {traditionalParams.PageSize}");
            Console.WriteLine($"  ├─ Retrieved: {traditionalResult.Data?.Count ?? 0} workspaces");
            Console.WriteLine($"  ├─ Total Count: {traditionalResult.TotalCount ?? 0}");
            Console.WriteLine($"  └─ Has More: {traditionalResult.TotalCount > (traditionalParams.Page * traditionalParams.PageSize)}");
            
            Console.WriteLine();
            
            // 2. Token-based pagination
            Console.WriteLine("Token-Based Pagination (TokenPaginationParameters):");
            var tokenParams = new TokenPaginationParameters(null, 100, "token");
            var tokenResult = smartsheet.WorkspaceResources.ListWorkspaces(tokenParams);
            
            Console.WriteLine($"  ├─ Type: Token-based");
            Console.WriteLine($"  ├─ Max Items: {tokenParams.MaxItems}");
            Console.WriteLine($"  ├─ Retrieved: {tokenResult.Data?.Count ?? 0} workspaces");
            Console.WriteLine($"  ├─ Last Key: {tokenResult.LastKey ?? "null"}");
            Console.WriteLine($"  └─ Has More: {tokenResult.LastKey != null}");
            
            Console.WriteLine("\n📝 Key Differences:");
            Console.WriteLine("   • Traditional: Uses offset/limit, provides total count");
            Console.WriteLine("   • Token-based: Uses lastKey, more efficient for large datasets");
            Console.WriteLine("   • Token approach prevents issues with data changes during pagination");
        }
    }
}