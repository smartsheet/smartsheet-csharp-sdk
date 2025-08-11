using System;

namespace sdk_csharp_sample_net80
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Smartsheet C# SDK Examples");
            Console.WriteLine("==========================");
            Console.WriteLine();
            Console.WriteLine("Choose an example to run:");
            Console.WriteLine("1. Simple Example (Basic sheet operations)");
            Console.WriteLine("2. Token Pagination Example (New workspace pagination features)");
            Console.WriteLine("3. HTTP Client Example");
            Console.WriteLine();
            Console.Write("Enter your choice (1-3): ");
            
            string? choice = Console.ReadLine();
            
            Console.WriteLine();
            
            switch (choice)
            {
                case "1":
                    Console.WriteLine("Running Simple Example...");
                    Console.WriteLine();
                    sdk_csharp_simpleExample.SimpleExample.RunExample(args);
                    break;
                
                case "2":
                    Console.WriteLine("Running Token Pagination Example...");
                    Console.WriteLine();
                    sdk_csharp_tokenPaginationExample.TokenPaginationExample.RunExample(args);
                    break;
                
                case "3":
                    Console.WriteLine("Running HTTP Client Example...");
                    Console.WriteLine();
                    sdk_csharp_HttpClientExample.ExampleWithHttpClientDefined.RunExample(args);
                    break;
                
                default:
                    Console.WriteLine("Invalid choice. Running Token Pagination Example as default...");
                    Console.WriteLine();
                    sdk_csharp_tokenPaginationExample.TokenPaginationExample.RunExample(args);
                    break;
            }
        }
    }
}