using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GGStat_Backend.data;

namespace GGStat_Backend.ApiControllers
{
	[Route("api/countries")]
	[ApiController]
	public class CountryController:ControllerBase
	{
			private readonly PlayersDBContext _context;

			public CountryController(PlayersDBContext context)
			{
				_context = context;
			}
		[HttpGet]
		public async Task<List<string>> GetCountries()
		{
			return _context.PlayerDatas.Select(c => c.code).Distinct().ToList();
		}
	}
	}
