using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Text.Json;
using GGStatParsingDataService.Models;
using HttpWrappers;
using Nager.Country;
using PortWrapper;

namespace GGStatParsingDataService.Services
{
	public interface IPlayerInfoParser
	{
		Task<List<PlayerData>> GetPlayerInfo(List<PlayerData> data, ICsvParserService csvParserService, string outputFilePath);

	}
	public class PlayerInfoParser:IPlayerInfoParser
	{
		
		private async Task<string> GetCountry(string json)
		{

			var jsonDoc = JsonDocument.Parse(json);

			var alpha3Code = jsonDoc.RootElement.GetProperty("country_code").GetString();

			var countryProvider = new CountryProvider();
			var countries = countryProvider.GetCountries();
			var alpha2Code = countries.FirstOrDefault(c => c.Alpha3Code.ToString() == alpha3Code);
			if (alpha2Code != null) return alpha2Code.Alpha2Code.ToString();
			return "Unknown";
		}

		
		private async Task<List<Match>> GetMatchHistory(string player, string json)
		{
			string playerRace = "";
			string opponentRace = "";
			string opponentName = "";
			string result = "";
			int rawDuration = 0;
			
			
			var jsonDoc = JsonDocument.Parse(json);

			var games = jsonDoc.RootElement.GetProperty("game_results");
			List<Match> values = new List<Match>();

			foreach (var game in games.EnumerateArray())
			{
				var _match_id = game.GetProperty("match_guid").GetString();

				var attributes = game.GetProperty("attributes");
				var mapName = attributes.GetProperty("mapName").GetString();
				var createTime = game.GetProperty("create_time").GetString();

				var create_time = int.Parse(createTime);
				var timeAgo = TimeParser.GetTime(create_time);
				var players = game.GetProperty("players");
				
				foreach (var playerInfo in players.EnumerateArray())
				{
					string toon = playerInfo.GetProperty("toon").GetString();
					string playerResult = playerInfo.GetProperty("result").GetString();
					
					if (toon == player)
					{
						var playerAttributes = playerInfo.GetProperty("attributes");
						playerRace = playerAttributes.GetProperty("race").GetString();
						
						if (playerInfo.TryGetProperty("stats", out var statsElement))
						{
							string statKey = $"{playerRace.ToLower()}_play_time";

							if (statsElement.TryGetProperty(statKey, out var playTimeProp))
							{
								rawDuration = int.Parse(playTimeProp.GetString());
							}
						}

						result = playerResult;
					}
					else if (!string.IsNullOrWhiteSpace(toon))
					{
						var oppAttributes = playerInfo.GetProperty("attributes");

						if (oppAttributes.TryGetProperty("race", out var raceProp))
							opponentRace = raceProp.GetString();
						else
							opponentRace = "unknown";

						opponentName = toon;
					}
				}

				string parsedDuration = TimeParser.ParseDuration(rawDuration);

				var match = new Match
				{
					match_id = _match_id,
					match_link = null,
					map = mapName,
					timeAgo = timeAgo,
					player_race = playerRace,
					opponent_race = opponentRace,
					opponent = opponentName,
					result = result,
					duration = parsedDuration,
					chat = { },
				};

				values.Add(match);
			}

			return values;
		}
		
		
		public async Task<List<PlayerData>> GetPlayerInfo(List<PlayerData> data, ICsvParserService csvParserService, string outputFilePath)
		{
			var players = new List<PlayerData>();
			var sortedData = data.OrderBy(x => x.standing).ToList();

			if (Settings.PlayerInfoOffset > 0)
			{
				int skipped = sortedData.Count(p => p.standing < Settings.PlayerInfoOffset);
				sortedData = sortedData.Where(p => p.standing >= Settings.PlayerInfoOffset).ToList();
				Console.WriteLine($"Skipping {skipped} players, starting from standing {Settings.PlayerInfoOffset}");
			}

			foreach (var player in sortedData)
			{
				try
				{
					var _player = player.player.name;
					var _region = player.player.region;
					int gatewayId = GetGatewayId(_region);
					var url = BuildPlayerinfoUrl(_player, gatewayId,Settings.Port);
					Console.WriteLine(url);
					var player_json = await HttpParser.GetRequest(url,Settings.Port);

					var _country = await GetCountry(player_json);
					var mmrStats = GetMmrStats(player_json, _player);
					var accounts = GetAccounts(player_json);

					var player_item = new PlayerData
					{
						standing = player.standing,
						player = player.player,
						country = new CountryInfo
						{
							code = _country,
							flag = GetFlagLink(_country)
						},
						rank = player.rank,
						race = player.race,
						wins = mmrStats.wins > 0 ? mmrStats.wins : player.wins,
						loses = mmrStats.losses > 0 ? mmrStats.losses : player.loses,
						max_mmr = mmrStats.maxMmr,
						current_mmr = mmrStats.currentMmr,
						accounts = accounts,
						matches = await GetMatchHistory(_player, player_json)
					};

					await csvParserService.WriteToCsvWithCountry([player_item], outputFilePath);
					players.Add(player_item);
					Console.WriteLine($"Player {_player} saved to CSV. ({player.standing})");
				}
				catch (Exception ex)
				{
					Console.WriteLine(
						$"Error processing player {player.player.name}: {ex.Message}. Skipping.");
				}
			}
			return players;
		}

		private static string BuildPlayerinfoUrl(string player, int region,int port)
		{
		
			return
				$"http://localhost:{port}/web-api/v2/aurora-profile-by-toon/{player}/{region}?request_flags=scr_profile";
		}

		private static int GetGatewayId(string region)
		{
			switch (region)
			{
				case "Europe" : return 20;
				case "Korea" : return 30;
				case "US East": return 11;
				case "US West": return 10;
				case "Asia": return 45;
				default: return 0;
			}
		}

		private static string GetFlagLink(string country)
		{
			return $"https://flagcdn.com/w40/{country.ToLower()}";
		}

		private static (int maxMmr, int currentMmr, int wins, int losses) GetMmrStats(string json, string playerToon)
		{
			var jsonDoc = JsonDocument.Parse(json);
			int maxMmr = 0;
			int currentMmr = 0;
			int wins = 0;
			int losses = 0;

			if (jsonDoc.RootElement.TryGetProperty("matchmaked_stats", out var stats) &&
			    jsonDoc.RootElement.TryGetProperty("matchmaked_current_season", out var currentSeasonEl))
			{
				int currentSeason = currentSeasonEl.GetInt32();

				foreach (var entry in stats.EnumerateArray())
				{
					if (entry.GetProperty("season_id").GetInt32() != currentSeason)
						continue;

					var toon = entry.GetProperty("toon").GetString();
					if (toon != playerToon)
						continue;

					maxMmr = entry.GetProperty("highest_rating").GetInt32();
					currentMmr = entry.GetProperty("rating").GetInt32();
					wins = entry.GetProperty("wins").GetInt32();
					losses = entry.GetProperty("losses").GetInt32();
					break;
				}
			}

			return (maxMmr, currentMmr, wins, losses);
		}

		private static string GetAccounts(string json)
		{
			var jsonDoc = JsonDocument.Parse(json);
			var toons = new List<string>();

			if (jsonDoc.RootElement.TryGetProperty("toons", out var toonsArray))
			{
				foreach (var toon in toonsArray.EnumerateArray())
				{
					var name = toon.GetProperty("toon").GetString();
					if (!string.IsNullOrWhiteSpace(name))
						toons.Add(name);
				}
			}

			return string.Join(" | ", toons);
		}
	}
}