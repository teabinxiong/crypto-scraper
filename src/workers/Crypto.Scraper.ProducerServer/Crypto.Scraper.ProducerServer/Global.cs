using Crypto.Scraper.ProducerServer.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Scraper.ProducerServer
{
	public class Global
	{
		public static Serilog.ILogger Logger;

		public static List<ManualResetEvent> ThreadCompleteEvents = new List<ManualResetEvent>();

		public static ConcurrentDictionary<string, BlockingCollection<Data>> DataQueue = new ConcurrentDictionary<string, BlockingCollection<Data>>();
	}
}
