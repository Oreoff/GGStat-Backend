using GGStat_Backend.controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GGStat_Backend.models;
using GGStat_Backend.data;
using System.Linq;

namespace GGStat_Backend.ApiControllers
{
	[Route("api/player-page")]
	[ApiController]
	public class PlayerController : ControllerBase
	{

		private readonly PlayersDBContext _context;

		public PlayerController(PlayersDBContext context)
		{
			_context = context;
		}
		[HttpGet("{name}")]
		public async Task<PlayerData?> GetPlayer(string name)
		{
			var player = await _context.PlayerDatas
				.Include(pd => pd.matches)
					.ThenInclude(m => m.chat)
				.FirstOrDefaultAsync(p => p.name == name);

			return player;
		}
	}
}
