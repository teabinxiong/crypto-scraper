using Crypto.Scraper.ProducerServer;
using Crypto.Scraper.ProducerServer.ApplicationServices;
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

Global.Logger = Log.Logger;

var svc = new BackgroudService();
svc.Start();

bool quit = false;

while (!quit)
{
	Thread.Sleep(1000);
	if (Console.KeyAvailable)
	{
		ConsoleKeyInfo key = Console.ReadKey();
		if (key.KeyChar == 'q' || key.KeyChar == 'Q')
		{
			Global.Logger.Information("Quit Program");

			svc.Stop();
		}
	}
}


host.Run();