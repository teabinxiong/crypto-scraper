using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Scraper.ProducerServer.ApplicationServices.Servics.Abstractions
{
    public abstract class WorkerProcess
    {
        public abstract void MainDataThreadProc(object obj);
    }
}
