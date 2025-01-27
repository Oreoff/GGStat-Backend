

using Microsoft.AspNetCore.Mvc;
using GGStat_Backend.data;
using GGStat_Backend.models;
using Microsoft.EntityFrameworkCore;
using GGStat_Backend.controllers;

namespace GGStat_Backend
{
	[Route("api/players")]
	[ApiController]
	public class SetData : ControllerBase
	{
		private readonly PlayersDBContext _context;

		public SetData(PlayersDBContext context)
		{
			_context = context;
		}

		// Збереження даних в базу даних
		[HttpPost("save")]
		public async Task<IActionResult> SavePlayersToDatabase()
		{
			try
			{
				int offset = 5;
				var playersFromApi = await GetData.GetPlayersAsync(offset);

				// Очищення існуючих даних (опціонально)
				_context.PlayerDatas.RemoveRange(_context.PlayerDatas);

				// Додавання нових даних
				await _context.PlayerDatas.AddRangeAsync(playersFromApi);
				await _context.SaveChangesAsync();

				return Ok("Data has been saved to the database.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error saving data: {ex.Message}");
			}
		}

		// Зчитування даних з бази даних
		[HttpGet]
		public async Task<IActionResult> GetPlayersFromDatabase()
		{
			try
			{
				var players = await _context.PlayerDatas
					.Include(pd => pd.player) 
					.Include(pd => pd.country)
					.Include(pd => pd.rank)
					.Include(pd => pd.matches)
					.ThenInclude(m => m.chat) 
					.ToListAsync();

				return Ok(players);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error reading data: {ex.Message}");
			}
		}
	}
}