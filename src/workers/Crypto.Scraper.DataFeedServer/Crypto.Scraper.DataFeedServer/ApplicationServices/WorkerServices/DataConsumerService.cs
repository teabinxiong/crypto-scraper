using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Scraper.ProducerServer.ApplicationServices.Models;
using Crypto.Scraper.ProducerServer.ApplicationServices.Services.Abstractions;

namespace Crypto.Scraper.ProducerServer.ApplicationServices.Services
{
    public sealed class DataConsumerService : WorkerProcess
	{

		public override void StartThreadProc(object obj)
		{
			Global.Logger.Information("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!ProcessDataThreadProc runnning");

			Global.ThreadCompleteEvents.Add(completeEvent);

			var key = "bitcoin";
			Data data;
			while (!IsThreadStopped())
			{
				if (!Global.DataQueue.ContainsKey(key))
				{
					Thread.Sleep(1000);
					continue;
				}

				while (!Global.DataQueue[key].TryTake(out data, 500))
				{
					Thread.Sleep(500);
					if (IsThreadStopped())
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
