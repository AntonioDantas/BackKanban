using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BACK
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                      .ConfigureWebHostDefaults(webBuilder =>
                      {
                          webBuilder.UseStartup<Startup>();
                      })
                     .ConfigureAppConfiguration((hostinContext, config) =>
                     {
                         Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Information()
                         .WriteTo.File("Logs/log.txt")
                         .WriteTo.Console()
                         .CreateLogger();
                     })
                    .UseSerilog();
    }
}
