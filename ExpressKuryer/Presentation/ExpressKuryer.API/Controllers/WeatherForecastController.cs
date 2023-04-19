using ExpressKuryer.Application.Profiles;
using ExpressKuryer.Application.Repositories;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Persistence.Configurations;
using ExpressKuryer.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace ExpressKuryer.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{

		[HttpGet]
		public IActionResult Index()
		{



			return Ok();
		}

	}

}