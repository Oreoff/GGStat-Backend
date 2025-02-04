using GGStat_Backend.controllers;
using GGStat_Backend.data;
using Microsoft.AspNetCore.Mvc;

namespace GGStat_Backend.ApiControllers
{
	[Route("api/matches")]
	[ApiController]
	public class MatchesController : ControllerBase
	{
		private readonly PlayersDBContext _context;

		public MatchesController(PlayersDBContext context)
		{
			_context = context;
		}

		[HttpGet("{matchId}/replay")]
		public async Task<IActionResult> GetReplayLink(string matchId)
		{
			var replayLink = await GetData.GetLinkAsync(matchId);
			if (string.IsNullOrEmpty(replayLink))
			{
				return NotFound("Replay link not found.");
			}
			return Ok(new { replayLink });
		}
	}
}
