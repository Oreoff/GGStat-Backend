using System.Net.NetworkInformation;
using System.Net;

namespace GGStat_Backend.controllers
{
	public class Settings
	{
		public static int Port;
		public static int BatchSize = 25;
		public static int StartBatchNumber = 121;
		public static int LeaderboardId = 12966;
		public static int DelayInMs = 300;
		public static string OutputFilePath = @"C:\temp\test_global.txt";
		public static async Task<int> GetPort()
		{
			using var httpClient = new HttpClient();

			var loopbackPorts = IPGlobalProperties
	.GetIPGlobalProperties()
	.GetActiveTcpListeners()
	.Where(ep =>
		ep.Address.Equals(IPAddress.Loopback) ||
		ep.Address.Equals(IPAddress.IPv6Loopback))
	.Select(ep => ep.Port)
	.ToHashSet();

			var localConnections = IPGlobalProperties
				.GetIPGlobalProperties()
				.GetActiveTcpConnections()
				.Where(conn =>
					(conn.LocalEndPoint.Address.Equals(IPAddress.Loopback) &&
					loopbackPorts.Contains(conn.LocalEndPoint.Port)))
				.ToList();
			var i = 0;

			foreach (var port in localConnections.Select(x => x.LocalEndPoint.Port).Distinct())
			{
				Console.WriteLine(port + " " + i.ToString());
				i++;
				string url = $"http://127.0.0.1:{port}/web-api/v1/leaderboard/{Settings.LeaderboardId}?offset=0&length={Settings.BatchSize}";

				try
				{
					var ct = new CancellationTokenSource(3000);
					var response = await httpClient.GetAsync(url, ct.Token);
					if (response.IsSuccessStatusCode)
					{
						Console.WriteLine($"StarCraft local web UI found on port {port}");
						return port;
					}
				}
				catch (HttpRequestException)
				{

				}
				catch (TaskCanceledException)
				{
				}
			}

			Console.WriteLine(" Could not find SCR web UI server.");
			return 0;
		}
	}
}
