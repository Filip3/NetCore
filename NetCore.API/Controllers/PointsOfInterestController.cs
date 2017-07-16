using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.API.Controllers
{
    [Route("api/cities/")]
    public class PointsOfInterestController : Controller
    {
        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            var resultCity = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

            if (resultCity == null)
                return NotFound();

            return Ok(resultCity.PointsOfInterest);
        }


        [HttpGet("{cityId}/pointsofinterest/{id}")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var resultCity = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

            if (resultCity == null)
                return NotFound();

            var resultPointOfInterest = resultCity.PointsOfInterest.FirstOrDefault(x => x.Id == id);

            if (resultPointOfInterest == null)
                return NotFound();

            return Ok(resultPointOfInterest);
        }
    }
}
