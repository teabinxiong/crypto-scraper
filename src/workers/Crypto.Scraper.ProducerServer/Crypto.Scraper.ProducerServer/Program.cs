using Crypto.Scraper.ProducerServer;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
	.AddEnvironmentVariables();

IHost host = Host.CreateDefaultBuilder()
	.ConfigureServices((context, services) =>
	{
	})
	.UseSerilog()
	.Build();

Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Build())
	//.Enrich.FromLogContext()
	.MinimumLevel.Verbose()
	.CreateLogger();

Global.logger = Log.Logger;

host.Run();