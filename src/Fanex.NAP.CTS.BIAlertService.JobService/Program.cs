using Fanex.NAP.CTS.BIAlertService.Shares;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Fanex.NAP.CTS.BIAlertService.JobService
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();

                LogExtension.LogInfo(string.Format("{0} has been started at {1}", typeof(Program).FullName, DateTime.Now));

                host.Run();
            }
            catch (Exception ex)
            {
                ex.LogError();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.SetBasePath(env.ContentRootPath);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                    config.AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:5000");

                    webBuilder
                    .UseStartup<Startup>();
                });
        }

    }
}

