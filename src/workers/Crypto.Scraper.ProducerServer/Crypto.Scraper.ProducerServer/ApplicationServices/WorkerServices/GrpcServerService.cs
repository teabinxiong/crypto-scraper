using Crypto.Scraper.ProducerServer.ApplicationServices.GrpcHandlers;
using Crypto.Scraper.ProducerServer.ApplicationServices.Models;
using Crypto.Scraper.ProducerServer.ApplicationServices.Services.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Scraper.ProducerServer.ApplicationServices.Services
{
	public class GrpcServerService : WorkerProcess
	{
		public override void StartThreadProc(object obj)
		{
			var grpcConfig  = (GrpcConfigDto)(obj);
			var stoppingToken = grpcConfig.Ct;

			IHost host = Host.CreateDefaultBuilder()
				.ConfigureWebHost(builder =>
				{
					builder.ConfigureKestrel(options =>
					{
						options.Listen(System.Net.IPAddress.Parse(grpcConfig.Ip), grpcConfig.Port, listenOptions =>
						{
							listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
						});
					})
					.UseKestrel()
					.UseStartup<GrpcServerStartup>();

				})
				.UseSerilog()
				.Build();

			    host.StartAsync(stoppingToken).ConfigureAwait(false).GetAwaiter().GetResult();
		}


		public class GrpcServerStartup
		{
			public void ConfigureServices(IServiceCollection services)
			{
				services.AddGrpc();
			}

			public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
			{
				app.UseRouting();

				app.UseEndpoints(endpoints =>
				{
					endpoints.MapGrpcService<CoinDataStreamService>();
				});
			}
		}
	}
}
