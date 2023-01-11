using Asset.Domain.Services;
using Asset.ViewModels.CityVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.API.Controllers.MobileController
{
    [Route("mobile/api/[controller]")]
    [ApiController]
    public class MCityController : ControllerBase
    {

        private ICityService _cityService;
        public MCityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        [Route("GetCitiesByGovernorateId/{govId}")]
        public ActionResult GetCitiesByGovernorateId(int govId)
        {
            var lstCities = _cityService.GetCitiesByGovernorateId(govId);
            if (lstCities.Count() == 0)
            {
                return Ok(new { data = lstCities, msg = "No Data", status = '0' });
            }
            else
                return Ok(new { data = lstCities, msg = "Success", status = '1' });
        }
    }
}
