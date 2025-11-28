

using Microsoft.AspNetCore.Mvc;
using GGStat_Backend.data;
using GGStat_Backend.models;
using Microsoft.EntityFrameworkCore;
using GGStat_Backend.controllers;
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
		[HttpGet]
		public async Task<IActionResult> GetPlayersFromDatabase(
	int offset = 0,
	int limit = 25,
	string country_code = "",
	[FromQuery(Name = "league")] List<string> league = null,
	string race = "")
		{
			try
			{
				var query = _context.PlayerDatas.AsQueryable();

				if (!string.IsNullOrEmpty(country_code))
				{
					if (country_code.StartsWith("!"))
					{
						var excludeCode = country_code.Substring(1); 
						query = query.Where(p => p.code != excludeCode);
					}
					else
					{
						query = query.Where(p => p.code == country_code);
					}
				}
				if (!string.IsNullOrEmpty(race))
				{
					query = query.Where(p => p.race == race);
				}

				if (league != null && league.Count() > 0)
				{
					query = query.Where(p => league.Contains(p.league));
				}
				var totalCount = await query.CountAsync();
				var players = await query
					.OrderByDescending(p => p.points)
					.Skip(offset)
					.Take(limit)
					.ToListAsync();

				return Ok(new
				{
					players,
					totalCount
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error reading data: {ex.Message}");
			}
		}
	}
}