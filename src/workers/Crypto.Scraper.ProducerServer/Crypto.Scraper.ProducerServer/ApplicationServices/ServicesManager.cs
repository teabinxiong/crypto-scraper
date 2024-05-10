using Crypto.Scraper.ProducerServer.ApplicationServices.Servics;
using Crypto.Scraper.ProducerServer.Models;
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
		public DataProviderService dataProviderService;

		CancellationTokenSource cts = new CancellationTokenSource();

		public ServicesManager()
		{
			dataProviderService = new DataProviderService();
		}

		public void StartAllThread()
		{
			ThreadPool.QueueUserWorkItem(dataProviderService.MainDataThreadProc, cts.Token);
		}

		public void StopAllThread()
		{
			Global.Logger.Information("StopAllThread");

			dataProviderService.StopThread = true;

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
