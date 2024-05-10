using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Scraper.ProducerServer.ApplicationServices.Services.Abstractions
{
    public abstract class WorkerProcess
    {
		protected ManualResetEvent completeEvent = new ManualResetEvent(false);
		private bool stopThread = false;
		public abstract void StartThreadProc(object obj);

        public void StopThread()
        {
            this.stopThread = true;
		}

		public bool IsThreadStopped()
		{
			return this.stopThread;
		}
	}
}
