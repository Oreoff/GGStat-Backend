

using GGStat_Backend.models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using GGStat_Backend.controllers;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GGStat_Backend
{
	[Route("api/players")]
	[ApiController]
	public class SetData : ControllerBase
	{

		private readonly string _filePath = "data/players.json";

		[HttpPost("save")]
		public async Task<IActionResult> SavePlayersToJSON()
		{
			try
			{
			
				int offset = 0; 
				var playersFromApi = await GetData.GetPlayersAsync(offset);

				string serializedData = JsonConvert.SerializeObject(playersFromApi, Formatting.Indented);

				Directory.CreateDirectory(Path.GetDirectoryName(_filePath));

				await System.IO.File.WriteAllTextAsync(_filePath, serializedData);

				return Ok("Data has been saved to JSON file.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error saving data: {ex.Message}");
			}
		}
		[HttpGet]
		public async Task<IActionResult> GetPlayersFromJSON()
		{
			try
			{
				// Перевірити, чи файл існує
				if (!System.IO.File.Exists(_filePath))
				{
					return NotFound("JSON file not found. Please save data first.");
				}

				// Зчитування JSON із файлу
				string jsonData = await System.IO.File.ReadAllTextAsync(_filePath);

				// Десеріалізація у список PlayerData
				var players = JsonConvert.DeserializeObject<List<PlayerData>>(jsonData);

				return Ok(players);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error reading data: {ex.Message}");
			}
		}
	}
}