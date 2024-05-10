using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Scraper.ProducerServer.Models
{
	public sealed class Data
	{
		public string Name { get; set; }

		public override string? ToString()
		{
			return Name;
		}
	}
}
