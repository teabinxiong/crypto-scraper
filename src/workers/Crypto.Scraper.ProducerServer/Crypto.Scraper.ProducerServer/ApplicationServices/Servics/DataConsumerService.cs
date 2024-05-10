using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Scraper.ProducerServer.ApplicationServices.Servics.Abstractions;
using Crypto.Scraper.ProducerServer.Models;

namespace Crypto.Scraper.ProducerServer.ApplicationServices.Servics
{
    public sealed class DataConsumerService : WorkerProcess
	{
		public bool StopThread = false;

		public override void MainDataThreadProc(object obj)
		{
			Global.Logger.Information("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!ProcessDataThreadProc runnning");
			ManualResetEvent completeEvent = new ManualResetEvent(false);

			Global.ThreadCompleteEvents.Add(completeEvent);

			var key = "key1";
			Data data;
			while (!StopThread)
			{
				if (!Global.DataQueue.ContainsKey(key))
				{
					Thread.Sleep(1000);
					continue;
				}

				while (!Global.DataQueue[key].TryTake(out data, 500))
				{
					Thread.Sleep(500);
					if (StopThread)
					{
						Global.Logger.Information("ProcessDataThreadProc exiting");
						completeEvent.Set();
						return;
					}
				}

				Data result = (Data)Convert.ChangeType(data, typeof(Data));
				Global.Logger.Information($"#################Data is {result}");
			}


			completeEvent.Set();
			Global.Logger.Information("ProcessDataThreadProc exiting");
		}
	}
}
