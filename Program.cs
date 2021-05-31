using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace lookaroond
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) 
        {
             var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(a =>
                {
                    a.Sources.Clear();
                    a
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{environment}.json", true, true)
                        .AddJsonFile("appsettings.local.json", true, false)
                        .AddJsonFile("secrets.json", true, false)
                        .AddJsonFile("/config/config.override.json", true, true)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args)
                        ;
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseSerilog((a, l) =>
                        {
                         
                            l
                                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                                .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
                                .Enrich.FromLogContext()
                                .Enrich.WithProperty("env", environment)
                                .WriteTo.Console(LogEventLevel.Information,
                                    "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} {Message}{NewLine}{Exception}",
                                    theme: SystemConsoleTheme.Colored);
                            if (environment != "Production")
                            {
                                SelfLog.Enable(msg => Debug.WriteLine(msg));
                                SelfLog.Enable(Console.Error);
                            }
                        })
                        .UseKestrel(k =>
                        {
                            k.ListenAnyIP(8192); 
                            k.Limits.MaxRequestBodySize = 100*1024*1024; // or a given limit
                        });
                });
        }
    }
}
