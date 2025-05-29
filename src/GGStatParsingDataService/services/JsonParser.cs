using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGStatParsingDataService.services
{
	internal class JsonParser
	{
		private static readonly HttpClient client = new HttpClient();
		static JsonParser()
		{
			client.DefaultRequestHeaders.TryAddWithoutValidation ("Accept", "*/*");
			client.DefaultRequestHeaders.TryAddWithoutValidation ("User-Agent", "SimpleCheckout/1.1.0/ClassicGames (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) ");
			client.DefaultRequestHeaders.TryAddWithoutValidation("Referer", $"http://127.0.0.1:{Settings.Port}/gamedata/webui/dist/LeaderboardPanel/LeaderboardPanel.html?locale=enUS&port={Settings.Port}&guid=19895"); ;
			client.DefaultRequestHeaders.TryAddWithoutValidation ("Accept-Encoding", "gzip, deflate");
			client.DefaultRequestHeaders.TryAddWithoutValidation ("Accept-Language", "en-US,en;q=0.8");
		}
		public static async Task<string> GetRequest(string url)
		{
			try
			{
				HttpResponseMessage response = await client.GetAsync(url);
				response.EnsureSuccessStatusCode();
				string responseBody = await response.Content.ReadAsStringAsync();
				return responseBody;
			}
			catch (HttpRequestException e)
			{
				return $"Error: {e.Message}";
			}
		}
	}
}
