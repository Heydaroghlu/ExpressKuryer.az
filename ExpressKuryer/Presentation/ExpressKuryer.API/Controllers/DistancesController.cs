using ExpressKuryer.Infrastructure.Services.Map;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpressKuryer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistancesController : ControllerBase
    {
      
        [HttpGet]
        [Route("FindPrice")]
        public async Task<IActionResult> FindPrice(string FromTo, string Here)
        {
            string fromto = await FindCoordinate.FindCor(FromTo);
            string here = await FindCoordinate.FindCor(Here);
            double latitude = 0;
            double longitude = 0;
            double latitude1 = 0;
            double longitude1 = 0;

            latitude = Convert.ToDouble(fromto.Split(',')[0]);
            longitude = Convert.ToDouble(fromto.Split(',')[1]);
            latitude1 = Convert.ToDouble(here.Split(',')[0]);
            longitude1 = Convert.ToDouble(here.Split(',')[1]);


            double distance = FindDistance.FindDist(latitude, longitude, latitude1, longitude1);
            int endDistance = Convert.ToInt32(distance);
            double end = endDistance * 10 * 0.06;
            if (end < 3)
            {
                end = 3;
            }
            end= Math.Floor(end * 10) / 10;

            return Ok(new {Distance=distance*1000,Price=end});
        }
    }
}
