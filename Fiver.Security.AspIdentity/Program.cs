using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Fiver.Security.AspIdentity
{
   public class Program
   {
      public static void Main(string[] args)
      {
         Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.RollingFile("log-{Date}.log", fileSizeLimitBytes: 1048576, shared: true)
            .CreateLogger();

         try
         {
            Log.Information("Starting web host");
            var webHost = BuildWebHost(args);

            using (var scope = webHost.Services.CreateScope())
            {
               var services = scope.ServiceProvider;
               try
               {
                  Seeder.Initialize(services);
               }
               catch (Exception ex)
               {
                  Log.Error(ex, "An error occurred seeding the DB.");
               }
            }

            webHost.Run();
         }
         catch (Exception ex)
         {
            Log.Fatal(ex, "Host terminated unexpectedly");
         }
         finally
         {
            Log.CloseAndFlush();
         }
      }

      public static IWebHost BuildWebHost(string[] args)
      {
         return WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseSerilog()
            .Build();
      }
   }
}