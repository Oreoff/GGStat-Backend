

using data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GGStat_Backend.controllers;
using GGStat_Backend.Data;

namespace GGStat_Backend.ApiControllers
{
    [Route("api/players")]
    [ApiController]
    public class SetData(IApiRequestToDb apiRequestToDb) : ControllerBase
    {
        
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
				var(players,totalCount) = await apiRequestToDb.GetLeaderboard(offset,limit,country_code,league,race);
				return Ok(new
				{
					players,
					totalCount,
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error reading data: {ex.Message}");
			}
		}
	}
}