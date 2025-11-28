using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.TypeConversion;
using GGStat_Backend.Data;
using GGStatBackend.Infrastructure;

namespace GGStat_Backend.ImporterService
{
	public interface ICsvParser
	{ 
		List<PlayerData> GetDataFromCsv();
	}
	public class CsvParser:ICsvParser
	{
		public List<PlayerData> GetDataFromCsv()
		{
			var filePath = FileDirectoryParser.GetDirectoryForPlayerInfoToDocker();
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
	}


