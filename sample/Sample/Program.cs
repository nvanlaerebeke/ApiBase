using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Sample
{
#pragma warning disable RCS1102 // Make class static.

    public class Program
#pragma warning restore RCS1102 // Make class static.
    {
        [ExcludeFromCodeCoverage]
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Implement the CreateHostBuilder method
        /// Do not move this code to the Main method, required for integration tests
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Sample.API.Startup>());
    }
}
