
namespace HttpWrappers
{
	public class HttpParser
	{
		private static HttpClient Client;
		static HttpParser()
		{ 
			Client = new HttpClient();
			Client.DefaultRequestHeaders.TryAddWithoutValidation ("Accept", "*/*");
			
			Client.DefaultRequestHeaders.TryAddWithoutValidation ("User-Agent", "SimpleCheckout/1.1.0/ClassicGames (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) "); ;
			Client.DefaultRequestHeaders.TryAddWithoutValidation ("Accept-Encoding", "gzip, deflate");
			Client.DefaultRequestHeaders.TryAddWithoutValidation ("Accept-Language", "en-US,en;q=0.8");
		}
		public static async Task<string> GetRequest(string url, int port)
		{
			try
			{
				Client.DefaultRequestHeaders.TryAddWithoutValidation("Referer", $"http://127.0.0.1:{port}/gamedata/webui/dist/LeaderboardPanel/LeaderboardPanel.html?locale=enUS&port={port}&guid=19895"); ;
				HttpResponseMessage response = await Client.GetAsync(url);
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
