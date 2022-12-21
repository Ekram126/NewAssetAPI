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
        public IEnumerable<IndexCityVM.GetData> GetCitiesByGovernorateId(int govId)
        {
            return _cityService.GetCitiesByGovernorateId(govId);
        }

    }
}
