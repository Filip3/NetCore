using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCore.API.Entites;

namespace NetCore.API.Controllers
{
    public class DummyController : Controller
    {
        private CityInfoContext _ciContext;

        public DummyController(CityInfoContext ciContext)
        {
            _ciContext = ciContext;
        }

        [HttpGet]
        [Route("api/test")]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}
