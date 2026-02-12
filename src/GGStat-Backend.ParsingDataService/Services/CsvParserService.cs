using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Text;
using GGStatBackend.Infrastructure;
using GGStatParsingDataService.Models;
using PortWrapper;

namespace GGStatParsingDataService.Services
{
	public interface ICsvParserService
	{
		Task<List<PlayerData>> ReadData();

		Task WriteLeaderboardToCsvAsync(List<PlayerData> data, string filePath, bool firstRow = false);
		
		Task WriteToCsvWithCountry(List<PlayerData> data, string filePath);
	}
	public class CsvParserService:ICsvParserService
	{
	
		

		public async Task<List<PlayerData>> ReadData()
		{
			var filePath = FileDirectoryParser.GetDirectoryForLeaderboard();
			
			var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = true,
				MissingFieldFound = null,
				HeaderValidated = null,
				BadDataFound = context => { Console.WriteLine($"Bad data found: {context.RawRecord}"); },
			};

			using (var reader = new StreamReader(filePath, System.Text.Encoding.UTF8, true, 65536))
			using (var csv = new CsvReader(reader, csvConfiguration))
			{
				csv.Context.RegisterClassMap<PlayerDataMap>();
				var records = new List<PlayerData>();
				await foreach (var record in csv.GetRecordsAsync<PlayerData>())
				{
					records.Add(record);
				}
				Console.WriteLine($"Data succesfully loaded. {records.Count} records.");
				return records;
			}
		}
		
		public async Task WriteLeaderboardToCsvAsync(List<PlayerData> data, string filePath, bool firstRow = false)
		{
			if (data == null || data.Count == 0) return;

			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = firstRow 
			};
			Console.WriteLine(filePath);
			
			using (var writer = new StreamWriter(filePath, append: !firstRow, encoding: new UTF8Encoding(firstRow)))
			using (var csv = new CsvWriter(writer, config)) 
			{
				csv.Context.RegisterClassMap<PlayerDataMap>();

				if (firstRow)
				{
					csv.WriteHeader<PlayerData>();
					await csv.NextRecordAsync(); 
				}

				await csv.WriteRecordsAsync(data);
			}
		}


		public async Task WriteToCsvWithCountry(List<PlayerData> data, string filePath)
		{
			foreach (var item in data)
			{
				var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
				{
					HasHeaderRecord = false,
				};
			
				using (var writer = new StreamWriter(filePath, append: true))
				using (var csv = new CsvWriter(writer, cfg))
				{
					csv.Context.RegisterClassMap<PlayerDataMapWithCountry>();
					await csv.WriteRecordsAsync([item]);
				}
			}
			
		}
	}
}