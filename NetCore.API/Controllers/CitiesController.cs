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
            var results = AutoMapper.Mapper.Map<IEnumerable<CityWithOutPointsOfInterestDTO>>(cityEntites);

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
                var cityResult = AutoMapper.Mapper.Map<CityDTO>(city);

                return Ok(cityResult);
            }

            var cityWithOutPointsOfInterestDTO = AutoMapper.Mapper.Map<CityWithOutPointsOfInterestDTO>(city);

            return Ok(cityWithOutPointsOfInterestDTO);
        }
    }
}
