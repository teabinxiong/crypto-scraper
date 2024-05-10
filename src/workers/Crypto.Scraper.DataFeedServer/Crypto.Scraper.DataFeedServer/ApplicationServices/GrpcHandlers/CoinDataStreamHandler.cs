using CoinDataServiceStream;
using Crypto.Scraper.ProducerServer.ApplicationServices.Models;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Scraper.ProducerServer.ApplicationServices.GrpcHandlers
{
	public class CoinDataStreamService : CoinData_Stream.CoinData_StreamBase
	{
		public CoinDataStreamService()
		{
		}

		public override Task GetData(CoinDataStreamRequest request, IServerStreamWriter<CoinDataStreamReply> responseStream, ServerCallContext context)
		{
			try
			{
				Data data;
				while (!context.CancellationToken.IsCancellationRequested)
				{
					while (Global.DataQueue[request.Name].TryTake(out data, 500))
					{
						if (data == null)
						{
							continue;
						}

						var quedata = Global.DataQueue;

						Data result = (Data)Convert.ChangeType(data, typeof(Data));
						Global.Logger.Information($"#################Data is {result}");

						if (!string.IsNullOrEmpty(result.Name))
						{
							var scheme = context.Peer;
							responseStream.WriteAsync(new CoinDataStreamReply
							{
								Port = scheme,
								Name = result.Name,
								Dt = result.Date.ToString(),
								Value = result.Value.ToString(),
							});
						}
					}

					Thread.Sleep(1000);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			return Task.CompletedTask;
		}
	}
}
