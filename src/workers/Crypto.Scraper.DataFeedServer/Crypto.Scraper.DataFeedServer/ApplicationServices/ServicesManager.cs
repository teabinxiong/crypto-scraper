﻿using Crypto.Scraper.ProducerServer.ApplicationServices.Models;
using Crypto.Scraper.ProducerServer.ApplicationServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Scraper.ProducerServer.ApplicationServices
{
    public sealed class ServicesManager
	{
		private readonly DataProviderService dataProviderService;
		private readonly DataConsumerService dataConsumerService;
		private readonly GrpcServerService grpcServerService;
		CancellationTokenSource cts = new CancellationTokenSource();

		public ServicesManager()
		{
			dataProviderService = new DataProviderService();
			dataConsumerService = new DataConsumerService();
			grpcServerService = new GrpcServerService();
		}

		public void StartAllThread()
		{
			ThreadPool.QueueUserWorkItem(dataProviderService.StartThreadProc, cts.Token);
			//ThreadPool.QueueUserWorkItem(dataConsumerService.StartThreadProc, cts.Token);

			// start 2 Grpc Servers
			ThreadPool.QueueUserWorkItem(delegate { grpcServerService.StartThreadProc(new GrpcConfigDto("127.0.0.1", 8600, cts.Token)); });
			ThreadPool.QueueUserWorkItem(delegate { grpcServerService.StartThreadProc(new GrpcConfigDto("127.0.0.1", 8601, cts.Token)); });
		}

		public void StopAllThread()
		{
			Global.Logger.Information("StopAllThread");

			dataConsumerService.StopThread();

			dataProviderService.StopThread();

			cts.Cancel();

			Global.Logger.Information("Wait for All theread to exit....");

			foreach (ManualResetEvent stopEvent in Global.ThreadCompleteEvents)
			{
				stopEvent.WaitOne();
			}

			Global.Logger.Information("Gracfully shutdown");
		}
	}
}
