

using Microsoft.AspNetCore.Mvc;
using GGStat_Backend.data;
using GGStat_Backend.models;
using Microsoft.EntityFrameworkCore;
using GGStat_Backend.controllers;
using Microsoft.AspNetCore.Mvc;
namespace GGStat_Backend.ApiControllers
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

        [HttpPost("save")]
		public async Task<IActionResult> SavePlayersToDatabase()
		{
			try
			{
				int offset = 0;
				var playersFromApi = await GetData.GetPlayersAsync(offset);


				var allPlayers = await _context.PlayerDatas.ToListAsync();
				_context.PlayerDatas.RemoveRange(allPlayers);
				await _context.SaveChangesAsync();
				_context.ChangeTracker.Clear();


				await _context.PlayerDatas.AddRangeAsync(playersFromApi);
				await _context.SaveChangesAsync();

				return Ok("Database has been cleared and new data inserted successfully.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error resetting and saving data: {ex.Message}");
			}
		}
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