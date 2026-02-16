
using data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GGStat_Backend.Data;

namespace GGStat_Backend.ApiControllers
{
	[Route("api/player-page")]
	[ApiController]
	public class PlayerController(IApiRequestToDb apiRequestToDb) : ControllerBase
	{
		[HttpGet("{name}")]
		public async Task<IActionResult> Get(string name)
		{
			var (player, alters) = await apiRequestToDb.GetPlayer(name);

			if (player == null)
				return NotFound();

			return Ok(new
			{
				player,
				alterAccounts = alters
			});
		}
	}
}
