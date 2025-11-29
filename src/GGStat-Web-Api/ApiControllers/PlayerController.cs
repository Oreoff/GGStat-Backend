
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
		public async Task<PlayerData?> GetPlayer(string name)
		{
			return await apiRequestToDb.GetPlayer(name);
		}
	}
}
