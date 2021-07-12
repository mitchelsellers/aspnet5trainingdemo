# Logging Setup

## Install these NuGet Packages

```
Install-Package Serilog.AspNetCore
Install-Package Serilog.Sinks.File
Install-Package Serilog.Settings.Configuration
```

Add this to the appsettings.json, replacing existing logging section

```
,
  "Serilog": {
    "Using": [
      "Serilog.Sinks.File"
    ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.EntityFrameworkCore.Database.Command": "Information"
      }
    },
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "\\Logs\\SampleSite-.log",
          "rollingInterval": "Day"
        }
      }
    ],
    "Properties": {
      "Application": "SampleApp"
    }
  }
  ```

### Validate Properties on the AppSettings.json  

This should be set to "Copy if Newer" under the "Copy to Output Directory"


## Replace Program.cs
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SampleWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //NOTE: You do not need user secrets here, just loading configuration early for logging
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            try
            {
                Log.Information("Starting Web Host");
                CreateHostBuilder(args).Build().Run();
                Log.Information("Host Shutting Down");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host Terminated Unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog() //Do this here, to catch builder errors!
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}

## Update Startup.cs
Add the following line of code before the `app.UseRouting();` call

```
app.UseSerilogRequestLogging(); //Added here to not capture static request items
```

## Update GitIgnore

Add /Logs to prevent committing logs