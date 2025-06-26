using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGStatParsingDataService.models;

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
			List<PlayerData> list = new List<PlayerData>();
			string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName;
			string filePath = Path.Combine(solutionDirectory, "db", "players.csv");
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}

			bool isFirstBatch = true;
			Settings.Port = await Settings.GetPort();

			while (offset < 100)
			{
				int retryCount = 0;
				bool success = false;

				while (!success && retryCount < 7)
				{
					try
					{
						var players = await GetData.GetPlayersAsync(offset);
						list.AddRange(players);
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

				if (list is { Count: > 0 })
				{
					await CsvWriterService.WriteToCsvAsync(list, filePath, isFirstBatch);
					isFirstBatch = false;
					Console.WriteLine($"{list.Count} players saved to CSV.");
				}

				list.Clear();
			}

			Console.WriteLine("Data has been successfully collected.");
		}

		public static string solutionDirectory =
			Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName;

		public static string filePath = Path.Combine(solutionDirectory, "db", "players.csv");

		public static async Task<List<PlayerData>> ReadData()
		{
			var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = true,
				BadDataFound = context => { Console.WriteLine($"Bad data found: {context.RawRecord}"); },
			};

			using (var reader = new StreamReader(filePath))
			using (var csv = new CsvReader(reader, csvConfiguration))
			{
				csv.Context.RegisterClassMap<PlayerDataMap>();
				var records = csv.GetRecords<PlayerData>().ToList();
				Console.WriteLine("Data succesfully loaded.");
				return records;
			}
		}

		public static async Task FindCountry(List<PlayerData> data)
		{
			var players = new List<PlayerData>();

			string GetRegion(int gateway_id)
			{
				if (gateway_id == 20) return "Europe";
				else if (gateway_id == 30) return "Korea";
				else if (gateway_id == 10) return "US West";
				else if (gateway_id == 11) return "US East";
				else if (gateway_id == 45) return "Asia";
				else return " ";
			}

			string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent
				.FullName;
			string filePath = Path.Combine(solutionDirectory, "db", "players_with_countries.csv");

			foreach (var player in data.OrderBy(x => x.standing).ToList())
			{
				int retryCount = 0;
				bool success = false;

				PlayerData current = null;
				while (!success && retryCount < 5)
				{
					try
					{
						var _player = player.player.name;
						var _region = player.player.region;

						var player_json = await JsonParser.GetRequest(
							$"http://127.0.0.1:{Settings.Port}/web-api/v2/aurora-profile-by-toon/{_player}/{_region}?request_flags=scr_profile"
						);

						var _country = await GetData.GetCountry(player_json);

						var player_item = new PlayerData
						{
							standing = player.standing,
							player = new Player
							{
								name = player.player.name,
								alias = player.player.alias,
								region = GetRegion(Int32.Parse(_region)),
								avatar = player.player.avatar
							},
							country = new CountryInfo
							{
								code = _country,
								flag = $"https://flagcdn.com/w40/{_country.ToLower()}.png",
							},
							rank = player.rank,
							race = player.race,
							wins = player.wins,
							loses = player.loses,
							matches = await GetData.GetMatchHistory(_player, player_json)
						};

						players.Add(player_item);
						current = player_item;
						success = true;
					}
					catch (Exception ex)
					{
						retryCount++;
						Console.WriteLine(
							$"Error processing player {player.player.name} (attempt {retryCount}/5): {ex.Message}");

						if (retryCount == 5)
						{
							Console.WriteLine(
								$"Failed to process player {player.player.name} after 5 attempts. Skipping.");
						}
						else
						{
							await Task.Delay(1000);
						}
					}
				}

				Console.WriteLine(current);
				await CsvWriterService.WriteToCSVWithCountry(current, filePath);
			}
		}
	}
}


