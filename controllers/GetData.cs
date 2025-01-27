using GGStat_Backend.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ISO3166;
using Nager.Country;

namespace GGStat_Backend.controllers
{
    internal class GetData
    {
		public static async Task<string> GetLinkAsync(string match_id)
		{
			
			if (string.IsNullOrWhiteSpace(match_id))
				return " ";

			// Отримуємо JSON з API
			var json = await JsonParser.GetRequest($"http://127.0.0.1:{Settings.Port}/web-api/v1/matchmaker-gameinfo-playerinfo/{match_id}");
			var jsonDoc = JsonDocument.Parse(json);

			// Перевіряємо, чи існує поле "replays"
			if (jsonDoc.RootElement.TryGetProperty("replays", out var replaysElement) && replaysElement.ValueKind == JsonValueKind.Array)
			{
				foreach (var replay in replaysElement.EnumerateArray())
				{
					// Перевіряємо, чи існує поле "url" у кожному об'єкті масиву
					if (replay.TryGetProperty("url", out var urlElement))
					{
						var match_link = urlElement.GetString();
						if (!string.IsNullOrWhiteSpace(match_link))
						{
							return match_link; // Повертаємо перше знайдене посилання
						}
					}
				}
			}

			return ""; // Якщо посилання не знайдено
		}
		public static async Task<string> GetCountry(string player, int gateway_id)
		{
			var json = await JsonParser.GetRequest($"http://127.0.0.1:{Settings.Port}/web-api/v2/aurora-profile-by-toon/{player}/{gateway_id}?request_flags=scr_profile");
			var jsonDoc = JsonDocument.Parse(json);

			var alpha3Code = jsonDoc.RootElement.GetProperty("country_code").GetString();

			var countryProvider = new CountryProvider();
			var countries = countryProvider.GetCountries();
			var alpha2Code = countries.FirstOrDefault(c => c.Alpha3Code.ToString() == alpha3Code);
			if (alpha2Code != null) return alpha2Code.Alpha2Code.ToString();
			else return "Unknown";
		}
		public static async Task<List<Match>> GetMatchHistory(string player, int gateway_id)
		{
			string GetTime(long unixTime)
			{
				DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime);
				DateTime normalDate = dateTime.UtcDateTime;
				DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;
				TimeSpan timeElapsed = currentDateTime - dateTime;
				return $"{timeElapsed.Days} days, {timeElapsed.Hours} hours, {timeElapsed.Minutes} minutes ago";
			}

			// Отримання JSON
			var json = await JsonParser.GetRequest($"http://127.0.0.1:{Settings.Port}/web-api/v2/aurora-profile-by-toon/{player}/{gateway_id}?request_flags=scr_profile");
			var jsonDoc = JsonDocument.Parse(json);

			// Доступ до даних гри
			var games = jsonDoc.RootElement.GetProperty("game_results");
			List<Match> values = new List<Match>();

			foreach (var game in games.EnumerateArray())
			{
				var _match_id = game.GetProperty("match_guid").GetString();
				
				var attributes = game.GetProperty("attributes");
				var mapName = attributes.GetProperty("mapName").GetString();
				var createTime = game.GetProperty("create_time").GetString();
				
				var create_time = int.Parse(createTime);
				var timeAgo = GetTime(create_time);
				
				var players = game.GetProperty("players");
				string playerRace = "";
				string opponentRace = "";
				string opponentName = "";
				string result = "";
				var _match_link = await GetLinkAsync(_match_id);
				foreach (var playerInfo in players.EnumerateArray())
				{
					var playerAttributes = playerInfo.GetProperty("attributes");
					string toon = playerInfo.GetProperty("toon").GetString();
					string playerResult = playerInfo.GetProperty("result").GetString();

					if (toon == player)
					{
						playerRace = playerAttributes.GetProperty("race").GetString();
						result = playerResult;
					}
					else if (!string.IsNullOrEmpty(toon))
					{
						opponentRace = playerAttributes.GetProperty("race").GetString();
						opponentName = toon;
					}
				}


				var match = new Match
				{
					
					match_id = _match_id,
					match_link = _match_link,
					map = mapName,
					timeAgo = timeAgo,
					player_race = playerRace,
					opponent_race = opponentRace,
					opponent = opponentName,
					result = result,
					chat = { },
				};

				values.Add(match);
			}

			return values;
		}
		
		public static async Task<List<PlayerData>> GetPlayersAsync(int offset)
		{

			string GetRegion(int gateway_id)
			{
				if (gateway_id == 20) return "Europe";
				else if (gateway_id == 30) return "Korea";
				else if (gateway_id == 10) return "US West";
				else if (gateway_id == 11) return "US East";
				else if (gateway_id == 45) return "Asia";
				else return " ";
			}
			string CalcLeague(int points)
			{
				if (points < 1137) return "F";
				else if (points <= 1137 && points > 1426) return "E";
				else if (points <= 1426 && points > 1549) return "D";
				else if (points <= 1549 && points > 1697) return "C";
				else if (points <= 1697 && points > 2014) return "B";
				else if (points <= 2014 && points > 2370) return "A";
				else return "S";

			}
			var json = await JsonParser.GetRequest($"http://127.0.0.1:{Settings.Port}/web-api/v1/leaderboard/{Settings.LeaderboardId}?offset={offset}&length={Settings.BatchSize}");
			var jsonDoc = JsonDocument.Parse(json);
			var rows = jsonDoc.RootElement.GetProperty("rows");
			List<PlayerData> players = new List<PlayerData>();

			foreach (var row in rows.EnumerateArray())
			{
				var _country = "";
				var _points = 0;
					_country = await GetCountry(row[7].GetString(), row[2].GetInt32());
					_points = row[3].GetInt32();
				
				var player = new PlayerData
				{
					standing = row[0].GetInt32(),
					player = new Player
					{
						name = row[7].GetString(),
						alias = row[8].GetString(),
						region = GetRegion(row[2].GetInt32()),
						avatar = row[9].GetString(),
					},
					country = new CountryInfo
					{
						name = _country,
						flag = $"https://flagcdn.com/w40/{_country.ToLower()}.png",
					},
					rank = new Rank
					{
						points = _points,
						league = CalcLeague(_points),
					},
					race = row[10].GetString().Substring(0, 1).ToUpper(),
					wins = row[4].GetInt32(),
					loses = row[5].GetInt32(),
					matches = await GetMatchHistory(row[7].ToString(), row[2].GetInt32())
				};
				players.Add(player);
			}
			return players;
		}

	}
}
