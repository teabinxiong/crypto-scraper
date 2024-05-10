using CoinDataServiceStream;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Balancer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Scaper.ClientApp.Services
{
	public class CoinDataStreamingReceiverService
	{
			public static async void Stream(string coinID,  CancellationToken ct)
			{
				ManualResetEvent _mre = new ManualResetEvent(false);
				Global.mre.Add(_mre);

				var factory = new StaticResolverFactory(addr => new[]
				{
					new BalancerAddress("localhost", 8600),
					new BalancerAddress("localhost", 8601)
				});

				var services = new ServiceCollection();
				services.AddSingleton<ResolverFactory>(factory);

				using var channel = GrpcChannel.ForAddress(
					"static:///localhost",
					new GrpcChannelOptions
					{
						Credentials = ChannelCredentials.Insecure,
						ServiceProvider = services.BuildServiceProvider()
					});


			var client = new CoinData_Stream.CoinData_StreamClient(channel);
				var _request = new CoinDataStreamRequest();
				_request.Name = coinID;
				using var streamingCall = client.GetData(_request, cancellationToken: ct);

				try
				{
					await foreach (var streamingData in streamingCall.ResponseStream.ReadAllAsync(cancellationToken: ct))
					{
						Console.WriteLine($"Received Data From Grpc Server: Coin:{streamingData.Name}; value:{streamingData.Value}; at Time:{streamingData.Dt}");

						if (Global.IsStop == true)
						{
							_mre.Set();
							break;
						}
					}
				}
				catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
				{
					Console.WriteLine("Stream cancelled.");
				}
			}
		}			
}
