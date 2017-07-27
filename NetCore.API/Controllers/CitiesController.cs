using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCore.API.Services;
using NetCore.API.Models;

namespace NetCore.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            var cityEntites = _cityInfoRepository.GetCities();

            var results = new List<CityWithOutPointsOfInterestDTO>();

            foreach(var cityEntity in cityEntites)
            {
                results.Add(new CityWithOutPointsOfInterestDTO()
                {
                    Id = cityEntity.Id,
                    Name = cityEntity.Name,
                    Description = cityEntity.Description
                });
            }

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = _cityInfoRepository.GetCity(id, includePointsOfInterest);

            if (city == null)
                return NotFound();

            if(includePointsOfInterest)
            {
                var cityResult = new CityDTO()
                {
                    Id = city.Id,
                    Description = city.Description,
                    Name = city.Name
                };

                foreach(var pointOfInterest in city.PointsOfinterest)
                {
                    cityResult.PointsOfInterest.Add(new PointOfInterestDTO()
                    {
                        Id = pointOfInterest.Id,
                        Name = pointOfInterest.Name,
                        Description = pointOfInterest.Description
                    });
                }

                return Ok(cityResult);
            }

            var cityWithOutPointsOfInterestDTO = new CityWithOutPointsOfInterestDTO()
            {
                Id = city.Id,
                Description = city.Description,
                Name = city.Name
            };

            return Ok(cityWithOutPointsOfInterestDTO);
            //var result = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == id);

            //if (result == null)
            //    return NotFound();

            //return Ok(result);
        }
    }
}
