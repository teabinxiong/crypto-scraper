using Crypto.Scaper.ClientApp;
using Crypto.Scaper.ClientApp.Services;


var cts = new CancellationTokenSource();

CoinDataStreamingReceiverService.Stream("binance-peg-xrp", cts.Token);
CoinDataStreamingReceiverService.Stream("bitcoin", cts.Token);

Console.WriteLine("Press any key to exit...");

bool quit = false;

while (!quit)
{
	Thread.Sleep(1000);
	if (Console.KeyAvailable)
	{
		ConsoleKeyInfo key = Console.ReadKey();
		if (key.KeyChar == 'q' || key.KeyChar == 'Q')
		{
			Console.WriteLine("Quit Program");
			Global.IsStop = true;
			cts.Cancel();

			foreach (var _mre in Global.mre)
			{
				_mre.WaitOne();
			}
		}
	}

}

Console.ReadKey();