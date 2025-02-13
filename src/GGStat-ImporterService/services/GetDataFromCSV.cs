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

		public static string filePath = Path.Combine("/app/db", "players.csv");

		public static List<PlayerData> SaveData()
		{
			if (!File.Exists(filePath))
			{
				Console.WriteLine("❌ CSV файл не знайдено!");
				return new List<PlayerData>();
			}

			var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = true,
				BadDataFound = context =>
				{
					Console.WriteLine($"Bad data found: {context.RawRecord}");
				},
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
			Map(p => p.standing);
			Map(p => p.player.name);
			Map(p => p.country.code);
			Map(p => p.player.alias);
			Map(p => p.player.region);
			Map(p => p.player.avatar);
			Map(p => p.country.flag);
			Map(p => p.rank.points);
			Map(p => p.rank.league);
			Map(p => p.race);
			Map(p => p.wins);
			Map(p => p.loses);
			Map(p => p.matches).TypeConverter<MatchListConverter>();

		}
		public class MatchListConverter : DefaultTypeConverter
		{
			public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
			{
				if (string.IsNullOrWhiteSpace(text))
					return new List<Match>();

				// Ensure no unwanted characters (like control characters)
				text = System.Text.RegularExpressions.Regex.Replace(text, @"[\x00-\x1F\x7F]", "");

				return text.Split('|')
					.Select(m =>
					{
						var fields = m.Split(',');
						if (fields.Length < 11)
						{
							// Log a warning for incomplete data
							Console.WriteLine($"Warning: Invalid match data format (missing fields): {m}");
							// Assign default values to missing fields
							while (fields.Length < 11)
							{
								Array.Resize(ref fields, fields.Length + 1);
								fields[fields.Length - 1] = ""; // Default empty value for missing fields
							}
						}

						// Return the match object
						return new Match
						{
							match_id = fields[0],
							match_link = fields[1],
							result = fields[2],
							points = int.TryParse(fields[3], out int p) ? p : 0,
							timeAgo = fields[4],
							map = fields[5],
							duration = fields[6],
							player_race = fields[7],
							opponent_race = fields[8],
							opponent = fields[9],
							chat = null
						};
					})
					.Where(m => m != null) // Remove invalid matches
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

