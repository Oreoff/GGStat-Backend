using GGStat_Backend.controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using data;
using GGStat_Backend.Data;

namespace GGStat_Backend.ApiControllers
{
	[Route("api/search")]
	[ApiController]
	public class SearchController(IApiRequestToDb apiRequestToDb): ControllerBase
	{
		
		[HttpGet("{name}")]
		public async Task<List<string>> SearchPlayersByName(string name)
		{
			return await apiRequestToDb.SearchPlayersByName(name);
		}
		
	}
}

