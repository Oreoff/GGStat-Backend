using GGStat_Backend.controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GGStat_Backend.Data;

namespace GGStat_Backend.ApiControllers
{
	[Route("api/country-top")]
	[ApiController]
	public class CountryTopController(IApiRequestToDb apiRequestToDb) : ControllerBase
	{
		
		[HttpGet]
		public async Task<List<CountryTop>> SetCountryTop()
		{
			return await apiRequestToDb.GetCountryTop();
		}
	}
}
