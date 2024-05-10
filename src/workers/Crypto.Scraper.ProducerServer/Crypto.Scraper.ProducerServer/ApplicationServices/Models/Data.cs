using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Scraper.ProducerServer.ApplicationServices.Models
{
    public sealed class Data
    {
        public string Name { get; set; }
        public long Date { get; set; }

        public decimal Value { get; set; }

        public Data()
        {
            
        }

        public override string ToString()
        {
            return $"Name={Name};Date={Date};Value={Value}";
        }
    }
}
