
using GGStat_Backend.controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GGStat_Backend.ApiControllers
{
	[Route("api/players")]
	[ApiController]
	public class SetData(IMediator mediator) : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetPlayersFromDatabase(
			int offset = 0,
			int limit = 25,
			string country_code = "",
			bool IsUnique = false,
			[FromQuery(Name = "league")] List<string> league = null,
			string race = "")
		{
			try
			{
				var query = new GetLeaderboardQuery(
					offset,
					limit,
					country_code,
					league,
					race,
					IsUnique
				);

				var (players, totalCount) = await mediator.Send(query);

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