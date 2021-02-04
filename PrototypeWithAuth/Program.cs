using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PrototypeWithAuth.Data;

namespace PrototypeWithAuth
{
    public class Program
    {
        public static void Main(string[] args)
        //possibly need to clear the cookies - amd create a new user
        {
            //var host = CreateHostBuilder(args);

            //using (var scope = host.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    try
            //    {
            //        var serviceProvider = services.GetRequiredService<IServiceProvider>();
            //        var configuration = services.GetRequiredService<IConfiguration>();
            //        SeedRoles.CreateRoles(serviceProvider, configuration).Wait();

            //    }
            //    catch (Exception exception)
            //    {
            //        var logger = services.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(exception, "An error occurred while creating roles");
            //    }

            CreateHostBuilder(args).Build().Run();     
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
