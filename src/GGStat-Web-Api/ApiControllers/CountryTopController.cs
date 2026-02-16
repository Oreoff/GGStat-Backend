using GGStat_Backend.controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GGStat_Backend.Data;
using MediatR;

namespace GGStat_Backend.ApiControllers
{
	[Route("api/country-top")]
	[ApiController]
	public class CountryTopController(IMediator mediator) : ControllerBase
	{
		
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] GetCountryTopQuery query)
		{
			var result = await mediator.Send(query);
			return Ok(result);
		}
	}
}
