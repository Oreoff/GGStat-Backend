using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerDataMap : ClassMap<PlayerData>
{
	public PlayerDataMap()
	{
		Map(p => p.standing);
		Map(p => p.player.name);
		Map(p => p.country.code);
		Map(p => p.player.alias);
		Map(p => p.player.region);
		Map(p => p.player.avatar);
		Map(p => p.country.flag);
		Map(p => p.rank.points);
		Map(p => p.rank.league);
		Map(p => p.race);
		Map(p => p.wins);
		Map(p => p.loses);
		Map(p => p.matches);

	}


}

namespace GGStatParsingDataService.services
{
	internal class CSVWorker
	{
	public static async Task WriteLeaderboardToCSV()
		{
			int offset = 0;

			string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName;
			string filePath = Path.Combine(solutionDirectory, "db", "players.csv");
			Settings.Port = await Settings.GetPort();
			while (offset < 25)
			{
				int retryCount = 0;
				bool success = false;

				while (!success && retryCount < 5)
				{
					try
					{
						var players = await GetData.GetPlayersAsync(offset);

						if (players != null && players.Count > 0)
						{
							await CsvWriterService.WriteToCsvAsync(players, filePath);
							Console.WriteLine($"{players.Count} players saved to CSV.");
						}

						offset += Settings.BatchSize;
						success = true;
					}
					catch (Exception ex)
					{
						retryCount++;
						Console.WriteLine($"Error occurred (attempt {retryCount}/5): {ex.Message}");

						if (retryCount == 5)
						{
							Console.WriteLine("Maximum retry attempts reached. Exiting program.");
							Environment.Exit(1);
						}
						else
						{
							await Task.Delay(1000);
						}
					}
				}

			}

			Console.WriteLine("Data has been successfully collected.");
		}
		public static string filePath = Path.Combine("/app/db", "players.csv");

		public static List<PlayerData> SaveData()
		{
			var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = true,
				BadDataFound = context =>
				{
					Console.WriteLine($"Bad data found: {context.RawRecord}");
				},
			};

			using (var reader = new StreamReader(CSVWorker.filePath))
			using (var csv = new CsvReader(reader, csvConfiguration))
			{
				csv.Context.RegisterClassMap<PlayerDataMap>();

				var records = csv.GetRecords<PlayerData>().ToList();
				foreach (var record in records)
				{
					Console.WriteLine(record.matches.Count);
				}

				return records;
			}
		}
	}

	
}

