using System.Net;
using System.Net.NetworkInformation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GGStatParsingDataService.Services
{
	public class Settings
	{
		public static int Port;
		public static int BatchSize = 25;
		public static int LeaderboardId = 12966;
		public static int MaxSize = 50;
	}
}

