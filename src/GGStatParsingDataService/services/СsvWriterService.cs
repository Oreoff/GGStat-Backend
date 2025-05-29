using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using GGStatParsingDataService.models;
using System.Linq;
using System.Text;

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
	public class PlayerDataMapWithCountry : ClassMap<PlayerData>
	{
		public PlayerDataMapWithCountry()
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
			Map(p => p.matches).Convert(p =>
				p.Value == null ? string.Empty : string.Join(" | ", p.Value.matches.Select(m =>
					$"{m.match_id};{m.match_link};{m.result};{m.points};{m.timeAgo}," +
					$"{m.map};{m.duration};{m.player_race};{m.opponent_race};{m.opponent}")));
		}
	}

	public class CsvWriterService
	{
		public static async Task WriteToCsvAsync(List<PlayerData> data, string filePath, bool firstRow = false)
		{
			bool fileExists = File.Exists(filePath);
			
			using (var writer = new StreamWriter(filePath, append: true))
			using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
			{
				csv.Context.RegisterClassMap<PlayerDataMap>();
				if (!fileExists)
				{
					csv.WriteHeader<PlayerData>();
					csv.NextRecord();         
				}
				
				await csv.WriteRecordsAsync(data);
			}
		}
		public static async Task WriteToCSVWithCountry(PlayerData data, string filePath)
		{
			var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = false,
			};
			
			using (var writer = new StreamWriter(filePath, append: true))
			using (var csv = new CsvWriter(writer, cfg))
			{
				csv.Context.RegisterClassMap<PlayerDataMapWithCountry>();
				await csv.WriteRecordsAsync([data]);
			}
		}
	}
}