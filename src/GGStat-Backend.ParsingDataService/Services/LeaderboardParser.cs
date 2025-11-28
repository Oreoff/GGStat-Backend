
using System.Text.Json;
using GGStatParsingDataService.Models;
using HttpWrappers;
using Microsoft.Extensions.Logging;

namespace GGStatParsingDataService.Services
{
	public interface ILeaderboardParser
	{
		Task<List<PlayerData>> GetPlayersAsync(int offset);

	}

	public class LeaderboardParser(ILogger<LeaderboardParser> logger): ILeaderboardParser
	{
		
		public async Task<List<PlayerData>> GetPlayersAsync(int offset)
		{
			var url = BuildUrlForLeaderboard(offset);
			logger.LogInformation("Getting players from {url}", url);
			var json = await HttpParser.GetRequest(url, Settings.Port);

			var jsonDoc = JsonDocument.Parse(json);

			var rows = jsonDoc.RootElement.GetProperty("rows");

			List<PlayerData> players = new List<PlayerData>();

			foreach (var row in rows.EnumerateArray())
			{
				var _bucket = row[12].GetInt32();
				var _country = "";
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
						code = _country,
						flag = GetFlagUrl(_country),
					},
					rank = new Rank
					{
						points = row[3].GetInt32(),
						league = CalcLeague(_bucket),
					},
					race = row[10].GetString() == null || row[10].GetString().Length < 1
						? string.Empty
						: row[10].GetString().Substring(0, 1).ToUpper(),
					wins = row[4].GetInt32(),
					loses = row[5].GetInt32(),
					matches = null,
				};
				players.Add(player);
			}
			return players;
		}

		private static string CalcLeague(int _bucket)
		{
			switch (_bucket)
			{
				case 1: return "F";
				case 2: return "E";
				case 3: return "D";
				case 4: return "C";
				case 5: return "B";
				case 6: return "A";
				case 7: return "S";
				default: return "N/A";
			}
		}

		private static string GetRegion(int gateway_id)
		{
			switch (gateway_id)
			{
				case 20: return "Europe";
				case 30: return "Korea";
				case 10: return "US West";
				case 11: return "US East";
				case 45: return "Asia";
				default: return "N/A";
			}
		}

		private static string BuildUrlForLeaderboard(int offset)
		{
			return
				$"http://host.docker.internal:{Settings.Port}/web-api/v1/leaderboard/{Settings.LeaderboardId}?offset={offset}&length={Settings.BatchSize}";
		}

		private static string GetFlagUrl(string _country)
		{
			return $"https://flagcdn.com/w40/{_country.ToLower()}.png";
		}
	}
}