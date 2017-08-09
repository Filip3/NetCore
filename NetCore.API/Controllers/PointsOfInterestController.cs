﻿using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCore.API.Models;
using NetCore.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace NetCore.API.Controllers
{
    [Route("api/cities/")]
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> _logger;
        private IMailService _mailService;
        private ICityInfoRepository _cityInfoRepository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                if (!_cityInfoRepository.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found, when accessing points of interest.");

                    return NotFound();
                }

                var pointsOfInterestForCity = _cityInfoRepository.GetPointsOfInterestForCity(cityId);

                var pointsOfInterestForCityResults = AutoMapper.Mapper.Map<IEnumerable<PointOfInterestDTO>>(pointsOfInterestForCity);

                return Ok(pointsOfInterestForCityResults);
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
            if (!_cityInfoRepository.CityExists(cityId))
                return NotFound();

            var pointOfInterest = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterest == null)
                return NotFound();

            var pointOfInterestResult = AutoMapper.Mapper.Map<PointOfInterestDTO>(pointOfInterest);

            return Ok(pointOfInterestResult);
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

            if (!_cityInfoRepository.CityExists(cityId))
                return NotFound();

            var newPointOfInterest = AutoMapper.Mapper.Map<Entites.PointOfInterest>(pointOfInterest);

            _cityInfoRepository.AddPointOfInterestForCity(cityId, newPointOfInterest);

            if (!_cityInfoRepository.Save())
                return StatusCode(500, "A problem has occured while handling your request.");

            var createPointOfInterestToReturn = AutoMapper.Mapper.Map<Models.PointOfInterestDTO>(newPointOfInterest);
            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = createPointOfInterestToReturn.Id }, createPointOfInterestToReturn);
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

            if (!_cityInfoRepository.CityExists(cityId))
                return NotFound();

            var pointOfInterestForUpdate = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterestForUpdate == null)
                return NotFound();

            AutoMapper.Mapper.Map(pointOfInterest, pointOfInterestForUpdate);

            if (_cityInfoRepository.Save())
                return StatusCode(500, "A problem has occured while handling your request.");

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdatingDTO> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = Mapper.Map<PointOfInterestForUpdatingDTO>(pointOfInterestEntity);

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpDelete("{cityId}/pointofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExists(cityId))
                return NotFound();

            var pointOfInterestForDeleting = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestForDeleting == null)
                return NotFound();

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestForDeleting);

            if(!_cityInfoRepository.Save())
                return StatusCode(500, "A problem has occured while handling your request.");

            _mailService.Send("Point of interest deleted.",
                $"Point of interest {pointOfInterestForDeleting.Name} with id {pointOfInterestForDeleting.Id} was deleted.");

            return NoContent();
        }
    }
}
