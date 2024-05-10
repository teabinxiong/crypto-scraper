
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Scraper.ProducerServer.ApplicationServices
{
	public sealed class BackgroudService
	{
		private ServicesManager _servicesManager;
		public BackgroudService()
		{
			_servicesManager = new ServicesManager();
		}

		public void Start()
		{
			Global.Logger.Information("Start Data Processing Service");

			_servicesManager.StartAllThread();

			Global.Logger.Information("Press q or Q to quit");
		}

		public void Stop()
		{
			Console.WriteLine("Quit Data Processing Service");

			Global.Logger.Information("Quit program!");
			_servicesManager.StopAllThread();
		}
	}
}
