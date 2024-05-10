using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Scraper.ProducerServer.ApplicationServices
{
	public sealed class DataProcessingService
	{
		private DataProcessingService()
		{
		}

		public static DataProcessingService New()
		{
			return new DataProcessingService();
		}

		public void Start()
		{
			Global.logger.Information("Start Data Processing Service");

			Global.logger.Information("Press q or Q to quit");
		}

		public void Stop()
		{
			Console.WriteLine("Quit Data Processing Service");

			Global.logger.Information("Quit program!");
		}
	}
}
