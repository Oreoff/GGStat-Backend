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
		Task<List<PlayerData>> GetPlayerInfo(List<PlayerData> data);
		
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
		
		
		public async Task<List<PlayerData>> GetPlayerInfo(List<PlayerData> data)
		{
			var players = new List<PlayerData>();
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
						int gatewayId = GetGatewayId(_region);
						var url = BuildPlayerinfoUrl(_player, gatewayId,Settings.Port);
						Console.WriteLine(url);
						var player_json = await HttpParser.GetRequest(url,Settings.Port);

						var _country = await GetCountry(player_json);

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
							wins = player.wins,
							loses = player.loses,
							matches = await GetMatchHistory(_player, player_json)
						};

						players.Add(player_item);
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
			}
			return players;
		}

		private static string BuildPlayerinfoUrl(string player, int region,int port)
		{
		
			return
				$"http://host.docker.internal:{port}/web-api/v2/aurora-profile-by-toon/{player}/{region}?request_flags=scr_profile";
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
	}
}