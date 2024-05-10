using Crypto.Scraper.ProducerServer.ApplicationServices.Models;
using Crypto.Scraper.ProducerServer.ApplicationServices.Services.Abstractions;
using Crypto.Scraper.ProducerServer.ApplicationServices.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Crypto.Scraper.ProducerServer.ApplicationServices.Services
{
    public class DataProviderService: WorkerProcess
	{
        System.Timers.Timer timer = new System.Timers.Timer();

        public override void StartThreadProc(object obj)
        {
			 ManualResetEvent completeEvent = new ManualResetEvent(false);
            Global.ThreadCompleteEvents.Add(completeEvent);

            timer.Enabled = true;
            timer.Interval = 20000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Global.Logger.Information("Task Processing Started.");

            var cryptoDataClient = new CryptoDataClient(Global.BaseUrl, Global.CryptoApiKey);


			var apiResult = cryptoDataClient.GetSimplePriceAsync("bitcoin,binance-peg-xrp", "myr", "full").GetAwaiter().GetResult();

			var jsonBody = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(apiResult);

			DateTime currentTime = DateTime.UtcNow;
			long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();

			foreach (var item in jsonBody)
			{
				var coin = new Data();
				coin.Name = item.Key;
				coin.Value = (decimal)item.Value.myr;
				coin.Date = unixTime;

				if (IsThreadStopped())
				{
					timer.Stop();
					completeEvent.Set();
					return;
				}

				if (!Global.DataQueue.ContainsKey(item.Key))
				{
					Global.DataQueue.TryAdd(item.Key, new BlockingCollection<Data>());
				}

				while (!Global.DataQueue[item.Key].TryAdd(coin, 500))
				{
					Thread.Sleep(500);

					Global.Logger.Information("waiting to add Data");
				}

				Global.Logger.Information($"Data added to the queue; data={coin}");
			}
			
            Global.Logger.Information("Task Processing ended.");
        }
    }
}
