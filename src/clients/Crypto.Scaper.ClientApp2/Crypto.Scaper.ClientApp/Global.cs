using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Scaper.ClientApp
{
	public class Global
	{
		public static List<ManualResetEvent> mre = new List<ManualResetEvent>();
		public static bool IsStop = false;
	}
}
