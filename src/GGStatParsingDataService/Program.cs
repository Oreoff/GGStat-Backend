
using GGStatParsingDataService.services;

internal class Program
{
	private static async Task Main(string[] args)
	{
		await CSVWorker.WriteLeaderboardToCSV();

		Settings.Port = await Settings.GetPort();
		var data = await CSVWorker.ReadData();
		await CSVWorker.FindCountry(data);
	}
}