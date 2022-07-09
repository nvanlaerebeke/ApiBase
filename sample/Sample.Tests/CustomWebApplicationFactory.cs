using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Sample.Tests
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //Can be used to for example initialize and pre-seed a DBContext

            //Set a base path to also make the api available here
            Environment.SetEnvironmentVariable("PATH_BASE", "/sample");
        }
    }
}
