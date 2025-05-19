using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using GGStatParsingDataService.models;
using System.Linq;

namespace GGStatParsingDataService.services
{
	public class PlayerDataMap : ClassMap<PlayerData>
	{
		public PlayerDataMap()
		{
			Map(p => p.standing);
			Map(p => p.player.name);
			Map(p => p.player.alias);
			Map(p => p.player.region);
			Map(p => p.player.avatar);
			Map(p => p.country.code);
			Map(p => p.country.flag);
			Map(p => p.rank.points);
			Map(p => p.rank.league);
			Map(p => p.race);
			Map(p => p.wins);
			Map(p => p.loses);
			Map(p => p.matches);
		
	}
	}

	public class CsvWriterService
	{
		public static async Task WriteToCsvAsync(List<PlayerData> data, string filePath)
		{
			using (var writer = new StreamWriter(filePath))
			using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
			{
				csv.Context.RegisterClassMap<PlayerDataMap>();
				csv.WriteHeader<PlayerData>();
				csv.NextRecord();
				await csv.WriteRecordsAsync(data);
			}
		}
	}
}