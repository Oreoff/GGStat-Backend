using GGStat_Backend.controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GGStat_Backend.models;
using GGStat_Backend.data;
using System.Linq;

namespace GGStat_Backend.ApiControllers
{
	[Route("api/search")]
	[ApiController]
	public class SearchController : ControllerBase
	{
		private readonly PlayersDBContext _context;

		public SearchController(PlayersDBContext context)
		{
			_context = context;
		}
		[HttpGet("{name}")]
		public async Task<List<string>> SearchPlayersByName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				return new List<string>();
			var nicknames = await _context.PlayerDatas
				.Where(pd => pd.name.ToLower().Contains(name.ToLower()))
				.Select(pd => pd.name)
				.Take(500)
				.ToListAsync();

			return nicknames;
		}
	}
}

