using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Crypto.Scraper.ProducerServer.ApplicationServices.Servics
{
    public class DataProviderService
    {
        public bool StopThread = false;
        ManualResetEvent completeEvent = new ManualResetEvent(false);
        System.Timers.Timer timer = new System.Timers.Timer();

        public void MainDataThreadProc(object obj)
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

            // run tasks here

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
