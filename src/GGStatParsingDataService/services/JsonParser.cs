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
