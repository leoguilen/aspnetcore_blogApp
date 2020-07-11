using Medium.App.Options;
using Medium.Infrastructure.Data.Context.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace Medium.App
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .Build();

            // Pegando url de conexão com o Seq
            var seqOptions = new SeqOptions();
            config.GetSection(nameof(SeqOptions)).Bind(seqOptions);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(seqOptions.Connection)
                .CreateLogger();

            Log.Information("Application is starting...");
            Log.Information("Seq starting in {seqConn}", seqOptions.Connection);

            CreateHostBuilder(args)
                .Build()
                .MigrateDatabase()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog();
                });
        }
    }
}
