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
		public async Task <List<CountryTop>> SetCountryTop()
		{
			List<CountryTop> list = new();
			var originalList =  await _context.PlayerDatas
					.Include(pd => pd.matches)
					.ThenInclude(m => m.chat)
					.ToListAsync();
			list = originalList.GroupBy(pd => pd.code) 
		.Select(group => group.OrderByDescending(pd => pd.points).First())
		.Select(topPlayer => new CountryTop
		{
			name = topPlayer.name,
			region = topPlayer.region,
			avatar = topPlayer.avatar,
			code = topPlayer.code,
			flag = topPlayer.flag,
			points = topPlayer.points
		})
		.ToList();
	return list;
		}
	}
}
