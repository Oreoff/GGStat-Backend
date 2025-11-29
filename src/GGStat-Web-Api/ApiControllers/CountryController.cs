using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GGStat_Backend.Data;
namespace GGStat_Backend.ApiControllers
{
	[Route("api/countries")]
	[ApiController]
	public class CountryController(IApiRequestToDb apiRequestToDb):ControllerBase
	{
		[HttpGet]
		public async Task<List<string>> GetCountries()
		{
			return await apiRequestToDb.GetCountries();
		}
	}
	}
