using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.TypeConversion;

namespace services
{
	internal class GetDataFromCSV
	{


		public static string filePath = Path.Combine("/app/db", "players_with_countries.csv");

		public static List<PlayerData> SaveData()
		{
			if (!File.Exists(filePath))
			{
				Console.WriteLine("CSV file not found!");
				return new List<PlayerData>();
			}

			var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = false,
				Delimiter = ",",
				BadDataFound = context =>
				{
					Console.WriteLine($"Bad data found: {context.RawRecord}");
				},
				ShouldSkipRecord = record =>
				{
					return record.Record.All(string.IsNullOrWhiteSpace);
				}
			};

			using (var reader = new StreamReader(filePath))
			using (var csv = new CsvReader(reader, csvConfiguration))
			{
				csv.Context.RegisterClassMap<PlayerDataMap>();

				var records = csv.GetRecords<PlayerData>().ToList();
				foreach (var record in records)
				{
					Console.WriteLine(record.matches.Count);
				}

				return records;
			}
		}
	}

	public class PlayerDataMap : ClassMap<PlayerData>
	{
		public PlayerDataMap()
		{
			Map(p => p.standing).Index(0);
			Map(p => p.name).Index(1);
			Map(p => p.code).Index(2);
			Map(p => p.points).Index(3);
			Map(p => p.alias).Index(4);
			Map(p => p.flag).Index(5);
			Map(p => p.league).Index(6);
			Map(p => p.region).Index(7);
			Map(p => p.avatar).Index(8);
			Map(p => p.race).Index(9);
			Map(p => p.wins).Index(10);
			Map(p => p.loses).Index(11);
			Map(p => p.matches).Index(12).TypeConverter<MatchListConverter>();
		}
		public class MatchListConverter : DefaultTypeConverter
		{
			public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
			{
				if (string.IsNullOrWhiteSpace(text))
					return new List<Match>();
				text = System.Text.RegularExpressions.Regex.Replace(text, @"[\x00-\x1F\x7F]", "");

				return text.Split('|')
					.Select(m => 
					{

						var fields = m.Split(';');
						if (fields.Length > 3)
						{
							var timeAndMap = fields[4].Split(',', 2);
							string timeAgo = timeAndMap.Length > 0 ? timeAndMap[0] : "";
							string map = timeAndMap.Length > 1 ? timeAndMap[1] : "";
							if (fields.Length < 11)
							{
								Console.WriteLine($"Warning: Invalid match data format (missing fields): {m}");
								while (fields.Length < 11)
								{
									Array.Resize(ref fields, fields.Length + 1);
									fields[fields.Length - 1] = "";
								}
							}

							return new Match
							{
								match_id = fields[0],
								match_link = fields[1],
								result = fields[2],
								points = int.TryParse(fields[3], out int p) ? p : 0,
								timeAgo = timeAgo,
								map = map,
								duration = fields[5],
								player_race = fields[6],
								opponent_race = fields[7],
								opponent = fields[8],
								chat = null
							};
						}
						else return null; 
					})
					.Where(m => m != null)
					.ToList();
			}
						
			

			public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
			{
				var matches = value as List<Match>;
				return string.Join(" | ", matches.Select(m =>
					$"{m.match_id},{m.match_link},{m.result},{m.points},{m.timeAgo}," +
					$"{m.map},{m.duration},{m.player_race},{m.opponent_race},{m.opponent}"));
			}
		}
	}
}

