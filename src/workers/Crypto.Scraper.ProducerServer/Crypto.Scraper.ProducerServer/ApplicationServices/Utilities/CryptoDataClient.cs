using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Scraper.ProducerServer.ApplicationServices.Utilities
{
	public sealed class CryptoDataClient
	{
		private readonly string _baseUrl;
		private readonly string _apiKey;
		public CryptoDataClient(string baseUrl, string apiKey)
		{
			_baseUrl = baseUrl;
			_apiKey = apiKey;
		}

		public async Task<string?> GetSimplePriceAsync(string coinType, string currency, string precision)
		{
			var date = DateTime.Now;
			var options = new RestClientOptions(_baseUrl) //"https://api.coingecko.com"
			{
				MaxTimeout = -1,
			};
			var client = new RestClient(options);
			var request = new RestRequest("/api/v3/simple/price?ids=" + coinType + "&vs_currencies=" + currency + "& precision=" + precision, Method.Get);
			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("x-cg-demo-api-key", _apiKey);
			RestResponse response = await client.ExecuteAsync(request);
		
			return response.Content;
		}

	}
}
