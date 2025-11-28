
using System.Text.Json;
using HttpWrappers;

namespace GGStat_Backend.controllers
{
    internal class GetData
    {
		public static async Task<string> GetLinkAsync(string match_id,int port)
		{
			if (string.IsNullOrWhiteSpace(match_id))
				return " ";
			var url = $"http://host.docker.internal:{port}/web-api/v1/matchmaker-gameinfo-playerinfo/{match_id}";
			Console.WriteLine(url);
			var json = await HttpParser.GetRequest($"http://host.docker.internal:{port}/web-api/v1/matchmaker-gameinfo-playerinfo/{match_id}",port);
			
			var jsonDoc = JsonDocument.Parse(json);
			if (jsonDoc.RootElement.TryGetProperty("replays", out var replaysElement) && replaysElement.ValueKind == JsonValueKind.Array)
			{
				foreach (var replay in replaysElement.EnumerateArray())
				{
	
					if (replay.TryGetProperty("url", out var urlElement))
					{
						var match_link = urlElement.GetString();
						if (!string.IsNullOrWhiteSpace(match_link))
						{
							return match_link;
						}
					}
				}
			}
			return ""; 
		}
		
	}
}
