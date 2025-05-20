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
			List <PlayerData> list = new List<PlayerData>();
			string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName;
			string filePath = Path.Combine(solutionDirectory, "db", "players.csv");
			Settings.Port = await Settings.GetPort();
			while (offset <= 50)
			{
				int retryCount = 0;
				bool success = false;

				while (!success && retryCount < 5)
				{
					try
					{
						var players = await GetData.GetPlayersAsync(offset);
						foreach( PlayerData p in players ) {
							list.Add(p);
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
			if (list != null && list.Count > 0)
			{
				await CsvWriterService.WriteToCsvAsync(list, filePath);
				Console.WriteLine($"{list.Count} players saved to CSV.");
			}
			Console.WriteLine("Data has been successfully collected.");
		}
		public static async Task WriteCountriesToCSV(List<PlayerData> players)
		{

			string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName;
			string filePath = Path.Combine(solutionDirectory, "db", "players_with_countries.csv");			
				int retryCount = 0;
				bool success = false;

				while (!success && retryCount < 5)
				{
					try
					{
						if (players != null && players.Count > 0)
						{
							await CsvWriterService.WriteToCSVWithCountry(players, filePath);
							Console.WriteLine($"{players.Count} players saved to CSV.");
						}
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
			Console.WriteLine("Data has been successfully collected.");
		}

			
		
		public static string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName;
		public static string filePath = Path.Combine(solutionDirectory, "db", "players.csv");
		public static async Task<List<PlayerData>> SaveData()
		{
			
			var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = true,
				BadDataFound = context =>
				{
					Console.WriteLine($"Bad data found: {context.RawRecord}");
				},
			};

			using (var reader = new StreamReader(filePath))
			using (var csv = new CsvReader(reader, csvConfiguration))
			{
				csv.Context.RegisterClassMap<PlayerDataMap>();

				var records = csv.GetRecords<PlayerData>().ToList();
				foreach (var record in records)
				{
					Console.WriteLine(record.matches.Count);
				}
				Console.WriteLine("Data succesfully written");

				return records;
			}
		}
		public static async Task<List<PlayerData>> FindCountry(List<PlayerData> data)
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
			foreach (var player in data)
			{
				int retryCount = 0;
				bool success = false;

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
						success = true;
					}
					catch (Exception ex)
					{
						retryCount++;
						Console.WriteLine($"Error processing player {player.player.name} (attempt {retryCount}/5): {ex.Message}");

						if (retryCount == 5)
						{
							Console.WriteLine($"Failed to process player {player.player.name} after 5 attempts. Skipping.");
						}
						else
						{
							await Task.Delay(1000);
						}
					}
				}
			}

			return players;
		}


	}
	}

	


