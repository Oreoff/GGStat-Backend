using GGStat_Backend.controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GGStat_Backend.models;
using GGStat_Backend.data;

namespace GGStat_Backend.ApiControllers
{
	[Route("api/country-top")]
	[ApiController]
	public class CountryTopController : ControllerBase
	{
		private readonly PlayersDBContext _context;

		public CountryTopController(PlayersDBContext context)
		{
			_context = context;
		}
		[HttpGet]
		public async Task<List<CountryTop>> SetCountryTop()
		{
			var players = await _context.PlayerDatas
				.Select(pd => new
				{
					pd.name,
					pd.region,
					pd.avatar,
					pd.code,
					pd.flag,
					pd.points,
					pd.alias
				})
				.ToListAsync();

			var topPlayers = players
				.GroupBy(p => p.code)
				.Select(g => g.OrderByDescending(p => p.points).First())
				.Select(p => new CountryTop
				{
					name = p.name,
					region = p.region,
					alias = p.alias,
					avatar = p.avatar,
					code = p.code,
					flag = p.flag,
					points = p.points
				})
				.ToList();

			return topPlayers;
		}
	}
}
