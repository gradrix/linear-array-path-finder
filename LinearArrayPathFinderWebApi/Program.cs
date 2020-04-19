using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace LinearArrayPathFinderWebApi
{
    public class Program
    {
        public const string ServerUrl = "http://localhost:8080";

        public static void Main(string[] args)
        {
            Console.WriteLine($"Starting WebApi server at {ServerUrl}..");
            Console.WriteLine("Endpoints:\n");
            Console.WriteLine($"To find path:\nPOST: {ServerUrl}/FindPath/\nJSON INPUT EXAMPLE: [1,2,0,3,0,2,0]\n");
            Console.WriteLine($"To batch find paths:\nPOST: {ServerUrl}/BulkFindPath/\nJSON INPUT EXAMPLE: [[1,2,0,3,0,2,0],[1,2,0,2,0,2,0],[1,2,0,1,0,-1]]\n");
            Console.WriteLine($"To retrieve all saved attempts:\nGET: {ServerUrl}/GetResults/All\n");
            Console.WriteLine($"To retrieve saved attempt by Id:\nGET: {ServerUrl}/GetResults/ById/1\n");
            Console.WriteLine($"To retrieve saved attempt by Input:\nGET: {ServerUrl}/GetResults/ByInput/1,2,0,3,0,2,0\n");
            Console.WriteLine($"Press Ctrl+C to stop WebApi Server");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(ServerUrl);
                });
    }
}
