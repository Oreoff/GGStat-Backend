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
			var filePath = FileDirectoryParser.GetDirectoryForLeaderboardToDocker();
			
			var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = true,
				BadDataFound = context => { Console.WriteLine($"Bad data found: {context.RawRecord}"); },
			};

			using (var reader = new StreamReader(filePath))
			using (var csv = new CsvReader(reader, csvConfiguration))
			{
				csv.Context.RegisterClassMap<PlayerDataMap>();
				var records = csv.GetRecords<PlayerData>().ToList();
				Console.WriteLine("Data succesfully loaded.");
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