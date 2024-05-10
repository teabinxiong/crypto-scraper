using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Scraper.ProducerServer.ApplicationServices.Models
{
	public class GrpcConfigDto
	{
		public string Ip { get; private set; } 
		
		public int Port { get; private set; }  
		
		public CancellationToken Ct { get; private set; }

        public GrpcConfigDto(string ip, int port, CancellationToken ct)
        {
			Ip = ip;
			Port = port;
			Ct = ct;
        }
    }
}
