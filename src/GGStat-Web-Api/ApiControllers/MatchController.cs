using data;
using GGStat_Backend.controllers;
using Microsoft.AspNetCore.Mvc;

namespace GGStat_Backend.ApiControllers
{
	[Route("api/matches")]
	[ApiController]
	public class MatchesController : ControllerBase
	{
		[HttpGet("{matchId}/replay")]
		public async Task<IActionResult> GetReplayLink(string matchId)
		{
			var replayLink = await GetData.GetLinkAsync(matchId,Settings.Port);
			if (string.IsNullOrEmpty(replayLink))
			{
				return NotFound("Replay link not found.");
			}
			return Ok(new { replayLink });
		}
	}
}
