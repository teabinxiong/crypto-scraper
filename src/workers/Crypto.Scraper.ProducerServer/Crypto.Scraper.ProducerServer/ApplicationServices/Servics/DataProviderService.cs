using Crypto.Scraper.ProducerServer.ApplicationServices.Servics.Abstractions;
using Crypto.Scraper.ProducerServer.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Crypto.Scraper.ProducerServer.ApplicationServices.Servics
{
    public class DataProviderService: WorkerProcess
	{
        public bool StopThread = false;
        ManualResetEvent completeEvent = new ManualResetEvent(false);
        System.Timers.Timer timer = new System.Timers.Timer();

        public override void MainDataThreadProc(object obj)
        {
            Global.ThreadCompleteEvents.Add(completeEvent);

            timer.Enabled = true;
            timer.Interval = 10000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Global.Logger.Information("Task Processing Started.");

            var key = "key1";
            var data = new Data()
            {
                Name = "Name 1"
            };

			// run tasks here
			if (!Global.DataQueue.ContainsKey(key))
			{
				Global.DataQueue.TryAdd(key, new BlockingCollection<Data>());
			}

			while (!Global.DataQueue[key].TryAdd(data, 500))
			{
				Thread.Sleep(500);

				Global.Logger.Information("waiting to add Data");
			}

			// stop processing if receive signal to stop
			if (StopThread)
            {
                timer.Stop();
                completeEvent.Set();
                return;
            }

            Global.Logger.Information("Task Processing ended.");
        }
    }
}
