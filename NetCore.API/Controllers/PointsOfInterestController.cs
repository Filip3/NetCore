using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCore.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.API.Controllers
{
    [Route("api/cities/")]
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> _logger;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                //throw new Exception("Sample Exception");

                var resultCity = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

                if (resultCity == null)
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found, when accessing points of interest.");

                    return NotFound();
                }

                return Ok(resultCity.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Log critical excpetion while getting points of interest fo city with id {cityId}.", ex);

                return StatusCode(500, "A problem occurred while processing the request");
            }
        }


        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
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

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDTO pointOfInterest)
        {
            if (pointOfInterest == null)
                return BadRequest();

            if (pointOfInterest.Description == pointOfInterest.Name)
                ModelState.AddModelError("Description", "The provided Name should ne different from the Description");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultCity = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

            if (resultCity == null)
                return NotFound();

            // will be improved
            var maxPointsOfInterest = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(x => x.Id);

            // can be improved with mapping, for example with AutoMapper
            var newPointOfInterest = new PointOfInterestDTO()
            {
                Id = ++maxPointsOfInterest,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            resultCity.PointsOfInterest.Add(newPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = newPointOfInterest.Id }, newPointOfInterest);
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, [FromBody] PointOfInterestForCreationDTO pointOfInterest)
        {
            if (pointOfInterest == null)
                return BadRequest();

            if (pointOfInterest.Description == pointOfInterest.Name)
                ModelState.AddModelError("Description", "The provided Name should ne different from the Description");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultCity = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

            if (resultCity == null)
                return NotFound();

            var pointOfInterestForUpdate = resultCity.PointsOfInterest.FirstOrDefault(x => x.Id == id);

            if (pointOfInterestForUpdate == null)
                return NotFound();

            pointOfInterestForUpdate.Name = pointOfInterest.Name;
            pointOfInterestForUpdate.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id, [FromBody] JsonPatchDocument<PointOfInterestForUpdatingDTO> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest();
            
            var resultCity = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

            if (resultCity == null)
                return NotFound();

            var pointOfInterestForUpdate = resultCity.PointsOfInterest.FirstOrDefault(x => x.Id == id);

            if (pointOfInterestForUpdate == null)
                return NotFound();

            var pointOfInterestToPatch = new PointOfInterestForUpdatingDTO()
            {
                Name = pointOfInterestForUpdate.Name,
                Description = pointOfInterestForUpdate.Description
            };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
                ModelState.AddModelError("Description", "The provided Name should ne different from the Description");

            TryValidateModel(pointOfInterestToPatch);
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            pointOfInterestForUpdate.Name = pointOfInterestToPatch.Name;
            pointOfInterestForUpdate.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{cityId}/pointofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var resultCity = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

            if (resultCity == null)
                return NotFound();

            var pointOfInterestForDeleting = resultCity.PointsOfInterest.FirstOrDefault(x => x.Id == id);

            if (pointOfInterestForDeleting == null)
                return NotFound();

            resultCity.PointsOfInterest.Remove(pointOfInterestForDeleting);

            return NoContent();
        }
    }
}
